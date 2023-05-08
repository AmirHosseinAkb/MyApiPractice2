using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common;
using Entities.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Services.Services
{
    public class JwtService:IJwtService
    {
        private readonly SiteSettings _siteSettings;
        public JwtService(IOptionsSnapshot<SiteSettings> siteSettings)
        {
            _siteSettings = siteSettings.Value;
        }
        public string Generate(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_siteSettings.JwtSettings.SecretKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature);

            var descriptor = new SecurityTokenDescriptor()
            {
                Issuer = _siteSettings.JwtSettings.Issuer,
                Audience = _siteSettings.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore =DateTime.Now.AddMinutes(_siteSettings.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(_siteSettings.JwtSettings.ExpirationMinutes),
                Subject = new ClaimsIdentity(_getClaims(user)),
                SigningCredentials = signingCredentials
            };
            var tokenhandler = new JwtSecurityTokenHandler();
            var securityToken = tokenhandler.CreateToken(descriptor);
            var jwt = tokenhandler.WriteToken(securityToken);
            return jwt;
        }


        private IEnumerable<Claim> _getClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.MobilePhone, "09222192282")
            };
            return claims;
        }
    }
}
