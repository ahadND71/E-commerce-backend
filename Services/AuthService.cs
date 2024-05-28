using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Dtos;

namespace Backend.Services
{
    public class AuthService
    {
        // Get JWT settings from environment variables


        public AuthService()
        {
            Console.WriteLine($"hhhhhhhh{Environment.GetEnvironmentVariable("Jwt__Key")}");


        }


        public string GenerateJwtToken(LoginUserDto loginRequest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key") ?? throw new
            InvalidOperationException("JWT Key is missing in environment variables.");
            var key = Encoding.ASCII.GetBytes(jwtKey);

            var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer") ?? throw new
InvalidOperationException("WT Issuer is missing in environment variables.");
            var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience") ?? throw
            new InvalidOperationException("WT Issuer is missing in environment variables.");


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, loginRequest.UserId.ToString()),
                    new Claim(ClaimTypes.Role, loginRequest.IsAdmin ? "Admin" : "Customer"),
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}