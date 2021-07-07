using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAppWebApi.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Current Password is Required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage ="New Password is Required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm New Password is Required")]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}
