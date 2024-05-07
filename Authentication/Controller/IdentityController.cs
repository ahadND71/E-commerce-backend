using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using api.Authentication.Helper;
using api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace api.Authentication.Controller
{
    public class IdentityController : ControllerBase
    {
        private const string _tokenSecret = "ForTheLoveOfGodStoreAndLoadThisSecurely";
        private static readonly TimeSpan _tokenLifeTime = TimeSpan.FromHours(8);
        private readonly AppDbContext _dbContext;
        public IdentityController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("api/token")]
        public IActionResult GenerateToken([FromBody] TokenGenerationRequest request)
        {
            bool isValidUser = ValidateUser(request.UserId, request.Email, request.Password);
            if (!isValidUser)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_tokenSecret);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, request.Email),
                new (JwtRegisteredClaimNames.Email, request.Email),
                new ("userid", request.UserId.ToString())
            };

            foreach (var claimPair in request.CustomClaims)
            {
                var jsonElement = (JsonElement)claimPair.Value;
                var valueType = jsonElement.ValueKind switch
                {
                    JsonValueKind.True => ClaimValueTypes.Boolean,
                    JsonValueKind.False => ClaimValueTypes.Boolean,
                    JsonValueKind.Number => ClaimValueTypes.Double,
                    _ => ClaimValueTypes.String
                };
                var claim = new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType);
                claims.Add(claim);
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_tokenLifeTime),
                Issuer = "https://localhost:7097",
                Audience = "https://localhost:7097",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(tokenString);
        }

        private bool ValidateUser(Guid id, string email, string password)
        {
            var user = _dbContext.Admins.FirstOrDefault(a => a.AdminId == id && a.Email == email) ??
            (dynamic?)_dbContext.Customers.FirstOrDefault(c => c.CustomerId == id && c.Email == email);
            if (user == null)
            {
                return false;
            }
            var passwordHasher = new PasswordHasher<dynamic>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success;
        }

    }
}
