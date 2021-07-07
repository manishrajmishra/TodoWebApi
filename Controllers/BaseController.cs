using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAppWebApi.Controllers
{
    public class BaseController : Controller
    {
        public string UserId { get; set; } = string.Empty;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var token = context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.ToString();
            token = token.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                UserId = jwtToken.Claims.Where(x => x.Type == "unique_name").Select(y => y.Value.ToString()).FirstOrDefault();
            }
        }
    }
}
