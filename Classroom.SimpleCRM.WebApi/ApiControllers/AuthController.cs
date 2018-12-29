using System.Threading.Tasks;
using Classroom.SimpleCRM.WebApi.Filters;
using Classroom.SimpleCRM.WebApi.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Classroom.SimpleCRM.WebApi.ApiControllers
{
    public class AuthController : Controller
    {
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return new ValidationFailedResult(ModelState);
            }

            var user = await Authenticate(credentials.EmailAddress, credentials.Password);
            if (user == null)
            {
                return new ValidationFailedResult("Invalid username or password.");
            }

            var userModel = await GetUserData(user);
            return Ok(userModel);
        }
    }
}