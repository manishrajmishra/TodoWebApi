using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAppWebApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public string Gender { get; set; }

        public string Dob { get; set; }

        public string Hobbies { get; set; }

        public string BloodGroup { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string ProfilePicture { get; set; }

        public string ConfirmationCode { get; set; }
    }
}
