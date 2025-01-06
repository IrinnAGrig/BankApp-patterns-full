
using Backend_BankingApp.Utilities.Proxy;
using BankingAppBackend.Model;
using BankingAppBackend.Requests.UserRequest;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankingAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
       {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

          }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginData loginData)
        {
            if (loginData == null)
            {
                return BadRequest("Invalid login data.");
            }
            if (string.IsNullOrWhiteSpace(loginData.Email))
            {
                return Unauthorized("Email error on sending.");
            }

            var user = await _mediator.Send( new FindByEmailAsync(loginData.Email));
            if (user == null)
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", loginData.Email);
                return Unauthorized("Invalid email or password.");
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginData.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", loginData.Email);
                return Unauthorized("Invalid email or password.");
            }

            _logger.LogInformation("User {Email} logged in successfully.", loginData.Email);
            return Ok(user);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpData signUpData)
        {
            if (signUpData == null)
            {
                return BadRequest("Invalid signup data.");
            }

            var result = await _mediator.Send(new CreateUserAsync(signUpData));
            if (result.status)
            {

                _logger.LogInformation("User {Email} signed up successfully.", signUpData.Email);
                return Ok(result.user); 
            }

            _logger.LogWarning("Errors: {Errors}", signUpData.Email);
            return BadRequest();
        }


          [HttpPut("change-password/{id}")]
          public async Task<IActionResult> ChangePassword([FromRoute] string id, [FromBody] ChangePasswordData changePasswordData, [FromServices] IUserService userService)
          {
               var result = await userService.ChangePasswordAsync(id, changePasswordData.OldPassword, changePasswordData.NewPassword);

               if (result)
               {
                    _logger.LogInformation("Password changed successfully for user: {Id}", id);
                    return Ok(new ErrorInfo { HasError = false, Error = string.Empty });
               }

               return BadRequest(new ErrorInfo { HasError = true, Error = "failed-update" });
          }


          [HttpPost("editprofile")]
          public async Task<IActionResult> EditProfile([FromBody] User userData, [FromServices] IUserService userService)
          {
               var result = await userService.EditProfileAsync(userData.Id, userData);

               if (result)
               {
                    _logger.LogInformation("Profile updated successfully for user: {Id}", userData.Id);
                    return Ok(result);
               }

               return BadRequest();
          }
    }
}
