using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace webtruyentranh.Utility
{
    public class Email_Utility
    {
        public static bool send_emailconfirm(String userEmail, string comfirmlink)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("heautoemail@gmail.com");
            mail.To.Add(new MailAddress(userEmail));
            mail.Subject = "Comfirm your email";
            mail.IsBodyHtml = true;
            mail.Body = @"<html>
                      <body>
                      <p></p>
                      <p>click the link under to verify your email</p>
                      <p>"+comfirmlink+ "</p></body></html>";

            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Credentials = new System.Net.NetworkCredential("heautoemail@gmail.com", "Nadeshiko5246!");
            client.Port = 587;
            client.EnableSsl = true;

            try
            {
                client.Send(mail);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }
    }
}
