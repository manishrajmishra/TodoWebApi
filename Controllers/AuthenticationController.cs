using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TodoAppWebApi.Models;

namespace TodoAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AuthenticationController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        [HttpPost]
        [Route("password-update")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            List<Response> responses = new List<Response>();
            var userId = UserId;
            var user = await userManager.FindByNameAsync(userId);

            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            responses.Add(new Response() { Status = "Password Updation Failed", CurrentPassword = "The Current Password is incorrect" });
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status400BadRequest, responses);

            return Ok(new Response { Status = "Success", Message = "User password has been changed successfully!" });
        }

        [Authorize]
        [HttpGet]
        [Route("detail")]
        public async Task<IActionResult> Details()
        {
            List<UserProfile> user_details = new List<UserProfile>();

            var userName = UserId;
            var user = await userManager.FindByNameAsync(userName);


            user_details.Add(new UserProfile()
            {
                Id = user.Id,
                UserName = UserId,
                Email = user.Email,
                Name = user.Name,
                Gender = user.Gender,
                Dob = user.Dob,
                Hobbies = user.Hobbies,
                BloodGroup = user.BloodGroup,
                Country = user.Country,
                State = user.State,
                City = user.City,
                Address = user.Address,
                Description = user.Description,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture

            });

            if(user_details == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Failed", Message = "Unauthorized" });
            }

            return Ok(new 
            {
                user_details,
                Status = "Success",
                Message = "LoggedIn Successfully"
            });
        }

        [Authorize]
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] UserProfile model)
        {
            
            var userName = UserId;
            var user = await userManager.FindByNameAsync(userName);


            user.UserName = model.UserName;
            user.Email = model.Email;
            user.Name = model.Name;
            user.Gender = model.Gender;
            user.Dob = model.Dob;
            user.Hobbies = model.Hobbies;
            user.BloodGroup = model.BloodGroup;
            user.Country = model.Country;
            user.State = model.State;
            user.City = model.City;
            user.Address = model.Address;
            user.Description = model.Description;
            user.PhoneNumber = model.PhoneNumber;
            if(model.coverPhoto != null)
            {
                string folder = "/upload/user/";

                folder += Guid.NewGuid().ToString() + "_" + model.coverPhoto.FileName;

                user.ProfilePicture = folder;

                string serverFolder = webHostEnvironment.WebRootPath + folder;

                await model.coverPhoto.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Failed", Message = "Error Occured" });
            }

            List<UserProfile> user_details = new List<UserProfile>
            {
                new UserProfile()
                {
                    Id = user.Id,
                    UserName = UserId,
                    Email = user.Email,
                    Name = user.Name,
                    Gender = user.Gender,
                    Dob = user.Dob,
                    Hobbies = user.Hobbies,
                    BloodGroup = user.BloodGroup,
                    Country = user.Country,
                    State = user.State,
                    City = user.City,
                    Address = user.Address,
                    Description = user.Description,
                    PhoneNumber = user.PhoneNumber,
                    ProfilePicture = user.ProfilePicture

                }
            };

            return Ok(new
            {
                user_details,
                Status = "Success",
                Message = "User's Profile Updated Successfully"
            });
        }
    }
}
