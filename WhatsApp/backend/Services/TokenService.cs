using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string userId)
        {
            var jwtConfig = _configuration.GetSection("JwtSettings");
            var key = jwtConfig["Key"];
            var expirationTimeInMinutes = int.TryParse(jwtConfig["ExpirationTimeInMinutes"], out int expTime) ? expTime : 1440;
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var tokenLifetime = TimeSpan.FromMinutes(expirationTimeInMinutes);

            // claims is about what the token will contain/create
            var claims = new List<Claim>
        {
            // jwt id = jti
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, userId),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(tokenLifetime),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}