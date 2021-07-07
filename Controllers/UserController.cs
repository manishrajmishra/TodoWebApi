using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoAppWebApi.Interfaces;
using TodoAppWebApi.Models;

namespace TodoAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IConfiguration configuration;
        private readonly IMailService mailService;

        public UserController(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMailService mailService, ApplicationDbContext applicationDbContext)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.mailService = mailService;
            this.applicationDbContext = applicationDbContext;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            List<Response> response1 = new List<Response>();
            List<Response> response2 = new List<Response>();
            List<Response> response3 = new List<Response>();

            response1.Add(new Response() { Status = "Failed", Email = "The Email is already taken", UserName = "The Username is already taken" });
            response2.Add(new Response() { Status = "Failed", Email = "The Email is already taken" });
            response3.Add(new Response() { Status = "Failed", UserName = "The Username is already taken" });

            var userExist = await userManager.FindByNameAsync(model.UserName);
            var emailExist = await userManager.FindByEmailAsync(model.Email);

            if (emailExist != null && userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, response1);

            else if (emailExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, response2);

            else if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, response3);

            

            ApplicationUser user = new ApplicationUser()
            {
                Name = model.Name,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,

            };

            

            var confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);

            var verificationCode = WebEncoders.Base64UrlEncode(encodedEmailToken);

            string url = $"{configuration["frontendDomain"]}/login?token={verificationCode}";

            user.ConfirmationCode = verificationCode;

            var result = await userManager.CreateAsync(user, model.Password);


           
            if (result.Succeeded)
            {
                mailService.SendEmail(user.Email, url);

                return Ok(new Response { Status = "Success", Message = "User Created and Email Sent for Verification to Login" });

            }

            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Failed", Message = "Error Occured" });
        }


        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<IActionResult> ConfirmEmail([FromForm] string verificationToken)
        {

            var result = await applicationDbContext.Users.Where(t => t.ConfirmationCode == verificationToken).SingleOrDefaultAsync();


            if (result != null)
            {
                result.ConfirmationCode = null;
                await applicationDbContext.SaveChangesAsync();
                return Ok(new
                {
                    Status = "Success",
                    Message = "Account is Verified"
                });

            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Failed", Message = "Please enter a valid verification code" });
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                Console.WriteLine(model.Email);
                Console.WriteLine(model.Password);

                var code = user.ConfirmationCode;
                if (code == null)
                {

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.NormalizedUserName),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    var authSigngKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));

                    var credentials = new SigningCredentials(authSigngKey, SecurityAlgorithms.HmacSha256);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Issuer = configuration["Jwt:ValidIssuer"],
                        Audience = configuration["Jwt:ValidAudience"],
                        Subject = new ClaimsIdentity(authClaims),
                        Expires = DateTime.Now.AddMinutes(3600),
                        SigningCredentials = credentials,
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();

                    var token = tokenHandler.CreateToken(tokenDescriptor);



                    var uniqueToken = tokenHandler.WriteToken(token);

                    return Ok(new
                    {
                        token = uniqueToken,
                        token_type = "bearer",
                        expires_in = token.ValidTo,
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Failed", Message = "Account is not Verified" });
                }
            }
            return StatusCode(StatusCodes.Status401Unauthorized, new Response { Status = "Failed", Message = "Unauthorized" });
        }
    }
}
