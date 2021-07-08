using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;
using WebTruyenTranhDataAccess.Models;
using Microsoft.AspNetCore.Http;
namespace webtruyentranh
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<ComicContext>(opt =>
            opt.UseSqlServer(Configuration.GetConnectionString("Default"))
            .LogTo(Console.WriteLine)
           .EnableSensitiveDataLogging()
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddIdentity<Account, IdentityRole<long>>(options =>
                        options.SignIn.RequireConfirmedAccount = true

                        ).AddTokenProvider<DataProtectorTokenProvider<Account>>(TokenOptions.DefaultProvider)

            .AddEntityFrameworkStores<ComicContext>();
            services.ConfigureApplicationCookie(config => {
                config.LoginPath = "/Authentication/index";
                config.AccessDeniedPath = new PathString("/profile/getme");
               });
            services.AddAuthentication().AddGoogle(option =>
            {
                option.ClientSecret = "5ecQw-i5kAVUHw6hQl2xnkxm";
                option.ClientId = "243247622743-ilkhev1tqbd4pt8qq720fvmla1abdu53.apps.googleusercontent.com";
                option.Scope.Add("profile");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                name: "authentication",
                pattern: "{controller=Authentication}/{action=index}"
                );
            endpoints.MapControllerRoute(
              name: "profile",
              pattern: "{controller=Profile}/{action=Getme}"
              );
            endpoints.MapControllerRoute(

             name: "profile",
             pattern: "{controller=Dashboard}/{action=Sumary}"
             );
            endpoints.MapControllerRoute(
                 name: "admin",
                pattern: "{controller=Admin}/{action=Index}"
                );

            });
            CreateUserRoles(serviceProvider).Wait();

             async Task CreateUserRoles(IServiceProvider serviceProvider)
            {
            
                var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();
                var UserManager = serviceProvider.GetRequiredService<UserManager<Account>>();
                string[] roleNames = {"SuperAdmin", "Admin","Member" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await RoleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
             
                        roleResult = await RoleManager.CreateAsync(new IdentityRole<long>(roleName));
                    }
                }

             
                var poweruser = new Account
                {
                    UserName = "admin12345",
                    Email = "admin@hemail.com",
                    EmailConfirmed = true,
                };
                
                string userPWD = "Admin@123456789";
                var _user = await UserManager.FindByEmailAsync(poweruser.Email);

                if (_user == null)
                {
                    
                   
                    var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                    if (createPowerUser.Succeeded)
                    {
                        Debug.WriteLine("create admin sucessed");

                        //here we tie the new user to the role
                        await UserManager.AddToRoleAsync(poweruser, "SuperAdmin");
                    }
                    Debug.WriteLine("create admin no suc");
                }
            }
        }
    }
}