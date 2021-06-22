using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class Role
    {
        public Role(long id, string roleName, string normalizedName, string concurrencyStamp)
        {
            Id = id;
            RoleName = roleName;
            NormalizedName = normalizedName;
            ConcurrencyStamp = concurrencyStamp;
        }

        public long Id { get; set; }
        public string RoleName { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}