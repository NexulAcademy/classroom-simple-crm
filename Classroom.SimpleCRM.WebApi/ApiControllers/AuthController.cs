using System.Threading.Tasks;
using Classroom.SimpleCRM.WebApi.Filters;
using Classroom.SimpleCRM.WebApi.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Classroom.SimpleCRM.WebApi.ApiControllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<CrmIdentityUser> _userManager;

        public AuthController(UserManager<CrmIdentityUser> userManager)
        {
            _userManager = userManager;
        }

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
        private async Task<CrmIdentityUser> Authenticate(string emailAddress, string password)
        {
            if (string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(password))
                return await Task.FromResult<CrmIdentityUser>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(emailAddress);

            if (userToVerify == null) return await Task.FromResult<CrmIdentityUser>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(userToVerify);
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<CrmIdentityUser>(null);
        }
    }
}