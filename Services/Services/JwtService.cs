using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Services.Services
{
    public class JwtService:IJwtService
    {
        //TODO:Break 15 minutes ....
        public JwtService()
        {
            
        }
        public string Generate(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes("NEWRANDOMSECRETKEY");
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature);

            var descriptor = new SecurityTokenDescriptor()
            {
                Issuer = "Licensify.ir",
                Audience = "Licensify.ir",
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(5),
                Expires = DateTime.Now.AddDays(1),
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
