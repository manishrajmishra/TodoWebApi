using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAppWebApi.Models
{
    public  class UserProfile
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public  string Gender { get; set; }

        public  string Dob { get; set; }

        public  string Hobbies { get; set; }

        public  string BloodGroup { get; set; }

        public  string Country { get; set; }

        public  string State { get; set; }

        public  string City { get; set; }

        public  string Address { get; set; }

        public  string Description { get; set; }

        public  string ProfilePicture { get; set; }

        public string PhoneNumber { get; set; }

        public IFormFile coverPhoto { get; set; }


    }
}
