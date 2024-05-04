using AuthentGuard.API.Database;
using AuthentGuard.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthentGuard.API.Services
{
    public class AuthService
    {
        private readonly SimplyDbContext _dbContext;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;

        public AuthService(SimplyDbContext dbContext, ILogger<AuthService> logger, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
        }

        public string Authenticate(string email, string password)
        {
            // Retrieve JWT configuration values from appsettings
            var secretKey = _configuration["Jwt:SecretKey"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            // Validate the user's credentials
            if (IsValidUser(email, password))
            {
                // Create JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, email),
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = issuer,
                    Audience = audience,
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                UpdateLastLogin(email);

                _logger.LogInformation($"User {email} authenticated successfully.");

                return tokenHandler.WriteToken(token);
            }
            else
            {
                // Log failed authentication attempt
                _logger.LogWarning($"Authentication failed for user {email}.");
                return null;
            }
        }

        public RegistrationResult Register(Register newRegister)
        {
            // Check if the email or username is already registered
            if (IsUserUnique(newRegister.Email))
            {
                // Hash the password before storing it
                string hashedPassword = HashPassword(newRegister.Password);

                // Store the new user in the database
                Register newUser = new Register
                {
                    Email = newRegister.Email,
                    Password = hashedPassword,
                    AgrredToTerms = newRegister.AgrredToTerms,
                    CreationDateUnixTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
                };
                _dbContext.Registers.Add(newRegister);
                _dbContext.SaveChanges();

                // Log successful registration
                _logger.LogInformation($"User {newRegister.Email} registered successfully.");

                return new RegistrationResult { Success = true };
            }
            else
            {
                // Log registration failure due to existing email or username
                _logger.LogWarning($"Registration failed. User with email '{newRegister.Email}' or username '{newRegister.Email}' already exists.");
                return new RegistrationResult { Success = false, Message = "User already exists" };
            }
        }

        public bool ValdidateToken(string token)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidUser(string email, string password)
        {
            // Retrieve the user from the database
            var user = _dbContext.Registers.FirstOrDefault(u => u.Email == email);

            // Check if the user exists and the password matches
            if (user != null)
            {
                // Verify the password hash
                string hashedPassword = HashPassword(password);
                return user.Password == hashedPassword;
            }
            return false;
        }

        private string HashPassword(string password)
        {
            // Implement a secure password hashing algorithm (e.g., bcrypt)
            // For demonstration, we're using a simple SHA256 hash
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        private bool IsUserUnique(string email)
        {
            // Check if the email or username is already registered
            return !_dbContext.Registers.Any(u => u.Email == email);
        }

        public void UpdateLastLogin(string email)
        {
            // Update the LastLoginUnixTimestamp of the user
            var user = _dbContext.Registers.FirstOrDefault(u => u.Email == email);
            user.LastLoginUnixTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            _dbContext.SaveChanges();
        }
    }
}
