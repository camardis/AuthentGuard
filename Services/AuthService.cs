using AuthentGuard.Database;
using AuthentGuard.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthentGuard.Services
{
    public class AuthService
    {
        private readonly SimplyDbContext _dbContext;
        private readonly ILogger<AuthService> _logger;
        private readonly string _secretKey = "your_secret_key";
        private readonly string _issuer = "your_issuer";
        private readonly string _audience = "your_audience";

        public AuthService(SimplyDbContext dbContext, ILogger<AuthService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public string Authenticate(string username, string password)
        {
            // Implement authentication logic, check against a database, etc.
            if (IsValidUser(username, password))
            {
                //var tokenHandler = new JwtSecurityTokenHandler();
                //var key = Encoding.ASCII.GetBytes(_secretKey);

                //var tokenDescriptor = new SecurityTokenDescriptor
                //{
                //    Subject = new ClaimsIdentity(new Claim[]
                //    {
                //        new Claim(ClaimTypes.Name, username),
                //    }),
                //    Expires = DateTime.UtcNow.AddHours(1),
                //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                //    Issuer = _issuer,
                //    Audience = _audience,
                //};

                //var token = tokenHandler.CreateToken(tokenDescriptor);
                //return tokenHandler.WriteToken(token);
                _logger.LogInformation($"User {username} authenticated successfully");
                return username + ":" + password;
            }

            return null;
        }

        public RegistrationResult Register(RegisterModel model)
        {
            // Implement user registration logic, store user in the database, etc.
            // Return a result object indicating success or failure
            // You may need to validate unique constraints, password policies, etc.

            RegisterModel newMember = new RegisterModel();
            newMember.UserName = model.UserName;
            newMember.Password = model.Password;
            newMember.Email = model.Email;
            newMember.CreationDateUnixTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            _dbContext.RegisterModel.Add(newMember);
            _dbContext.SaveChanges();

            _logger.LogInformation($"User {model.UserName} registered successfully");

            // For simplicity, assuming success
            return new RegistrationResult { Success = true };
        }

        private bool IsValidUser(string username, string password)
        {
            string dbPassword = _dbContext.RegisterModel.Where(u => u.UserName == username).Select(u => u.Password).FirstOrDefault();
            if (password == dbPassword)
            {
                return true;
            }
            return false;
        }

        private bool IsUniqueUser(string username)
        {
            // Implement your user uniqueness validation logic here
            // For simplicity, we assume a unique user for demonstration purposes
            return true;
        }

        private bool IsValidPassword(string password)
        {
            // Implement your password validation logic here
            // For simplicity, we assume a valid password for demonstration purposes
            return true;
        }

        private bool IsValidEmail(string email)
        {
            // Implement your email validation logic here
            // For simplicity, we assume a valid email for demonstration purposes
            return true;
        }

    }
}
