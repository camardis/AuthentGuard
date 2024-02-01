using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthentGuard.Services
{
    public class AuthService
    {
        public AuthService()
        {
            
        }

        private readonly string _secretKey = "your_secret_key";
        private readonly string _issuer = "your_issuer";
        private readonly string _audience = "your_audience";

        public string Authenticate(string username, string password)
        {
            // Add your authentication logic here (e.g., check against a database)

            if (IsValidUser(username, password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, username),
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _issuer,
                    Audience = _audience,
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            return null;
        }

        private bool IsValidUser(string username, string password)
        {
            // Implement your user validation logic here
            // For simplicity, we assume a valid user for demonstration purposes
            return true;
        }

    }
}
