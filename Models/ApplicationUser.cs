using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GamingE_Commerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Eircode { get; set; }
        public string MyProperty { get; set; }
        public string ProfileImageName { get; set; }  //Unused -> delete after interview
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; } //Unused
        public string ImagePath { get; set; } //
    }
}
