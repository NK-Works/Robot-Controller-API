/* This code is made by Anneshu Nag, Student ID: 2210994760 */
using System.Text;
using System.Text.Encodings.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using robot_controller_api.Persistence;
using Isopoh.Cryptography.Argon2;

namespace robot_controller_api.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserDataAccess _userServiceRepo;
        // private readonly IPasswordHasher<User> _passwordHasher;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserDataAccess userServiceRepo)
            : base(options, logger, encoder, clock)
        {
            _userServiceRepo = userServiceRepo;
            // _passwordHasher = new PasswordHasher<User>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()  // Added async
        {
            var endpointWithNoAuth = Context.GetEndpoint();

            if (endpointWithNoAuth?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return AuthenticateResult.NoResult();
            }
            
            // Add authentication challenge header
            base.Response.Headers.Add("WWW-Authenticate", @"Basic realm=""Access to the robot controller.""");
            
            // Get the Authorization header
            var authHeader = base.Request.Headers["Authorization"].ToString();

            // Check if Authorization header is missing or not in the correct format
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic "))
            {
                return AuthenticateResult.Fail("Missing or invalid Authorization header.");
            }

            try
            {
                // Extract base64 encoded credentials from Authorization header
                var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                
                // Decode base64 string to obtain username and password
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

                // Split email and password by ":" to retrieve user credentials
                // var parts = credentials.Split(':');
                // var useremail = parts[0];
                // var password = parts[1];

                var useremail = credentials.Split(':')[0]; // More efficient method (directly storing without using a variable)
                var password = credentials.Split(':')[1];

                // Retrieve user from the database using the provided username
                var user = await _userServiceRepo.GetUserByEmailAsync(useremail);

                /* Microsoft Password Hasher */
                // Check if user exists and if the password is correct or not (Microsoft dafualt Password Hasher Implementation)
                // if (user == null || !_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password).Equals(PasswordVerificationResult.Success))
                // {
                //     Response.StatusCode = 401;
                //     return AuthenticateResult.Fail("Invalid username or password.");
                // }    
                
                /* Bcrypt Password Hasher */
                var passVerificResult = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                
                if (!passVerificResult)
                {
                    Response.StatusCode = 401;
                    return AuthenticateResult.Fail("Authenticaiton Failed: Incorrect Password.");
                }

                /* Argon2 Password Hasher */
                // Verify password using Argon2
                // var passVerificResult = Argon2.Verify(user.PasswordHash, password);

                // if (!passVerificResult)
                // {
                //     Response.StatusCode = 401;
                //     return AuthenticateResult.Fail("Authentication Failed: Incorrect Password.");
                // }

                var claims = new List<Claim>
                {
                    new Claim("name", $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Role, user.Role ?? null),  
                    new Claim(ClaimTypes.Email, user.Email)
                    // Add new claims as per your wish
                };

                // Create and return authenticated user principal
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);

            }
            catch (Exception ex)
            {
                // Log any exceptions
                Logger.LogError(ex, "An error occurred during authentication.");
                return AuthenticateResult.Fail("Authentication Failed: User doesn't exist.");
            }
        }
    }
}
