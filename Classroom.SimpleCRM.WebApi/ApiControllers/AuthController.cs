using System.Linq;
using System.Threading.Tasks;
using Classroom.SimpleCRM.WebApi.Auth;
using Classroom.SimpleCRM.WebApi.Filters;
using Classroom.SimpleCRM.WebApi.Models;
using Classroom.SimpleCRM.WebApi.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Classroom.SimpleCRM.WebApi.ApiControllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<CrmIdentityUser> _userManager;
        private readonly IJwtFactory _jwtFactory;

        public AuthController(UserManager<CrmIdentityUser> userManager,
            IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
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

        private async Task<UserSummaryViewModel> GetUserData(CrmIdentityUser user)
        {
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count == 0)
            {
                roles.Add("prospect");
            }

            // generate the jwt for the local user...
            var jwt = await _jwtFactory.GenerateEncodedToken(user.UserName,
                _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id.ToString()));
            var userModel = new UserSummaryViewModel
            {   //JWT could inject all these properties instead of creating a model,
                //but a model is a little easier to access from client code without
                //decoding the token. When this user model starts to contain arrays
                //of complex data, including it all in the JWT value can get complicated.
                Id = user.Id,
                Name = user.Name,
                EmailAddress = user.Email,
                JwtToken = jwt,
                Roles = roles.ToArray(), //each role could be a separate claim in the JWT
                AccountId = 0 //TODO: load this from registration data
            };
            return userModel;
        }
    }
}