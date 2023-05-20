using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace recipeList
{
    public class JwtManager
    {
        private readonly IConfiguration _configuration;
        private readonly AuthOptions _authOptions;

        public JwtManager(IConfiguration configuration, AuthOptions authOptions)
        {
            _configuration = configuration;
            _authOptions = authOptions;
        }

        public string GenerateToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authOptions.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _authOptions.Issuer,
                Audience = _authOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authOptions.SecretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _authOptions.Issuer,
                ValidAudience = _authOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var _);
                return claimsPrincipal;
            }
            catch
            {
                // В случае недействительного или истекшего токена можно выбросить исключение или вернуть null.
                return null;
            }
        }
    }
}
