using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models

{
    public class Profile
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Display name cannot be blank")]
        [MaxLength(20, ErrorMessage = "Please use up to 20 characters.")]
        [Column(TypeName = "nvarchar(20)")]
        public string DisplayName { get; set; }

        [MaxLength(255, ErrorMessage = "Please use up to 255 characters.")]
        [Column(TypeName = "nvarchar(255)")]
        public string Description { get; set; }

        [MaxLength(512, ErrorMessage = "Url length is to long")]
        public string Avartar { get; set; }

        [MaxLength(512, ErrorMessage = "Url length is to long")]
        public string ExternalLink { get; set; }

        public DateTime DateJoined { get; set; }

        public Account Account { get; set; }
        public long AccountId { get; set; }

        public Profile()
        {
        }

        public Profile(long id, string displayName, string description,
                        string avartar, string externalLink, DateTime dateJoined)
        {
            Id = id;
            DisplayName = displayName;
            Description = description;
            Avartar = avartar;
            ExternalLink = externalLink;
            DateJoined = dateJoined;
        }
    }
}