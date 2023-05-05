﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities.User;
using Microsoft.IdentityModel.Tokens;

namespace Services.Services
{
    public class JwtService:IJwtService
    {
        public string Generate(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes("x");
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature);
            var descriptor = new SecurityTokenDescriptor()
            {
                Issuer = "Licensify.ir",
                Audience = "Licensify.ir",
                NotBefore = DateTime.Now.AddMinutes(10),
                Expires = DateTime.Now.AddDays(1),
                IssuedAt = DateTime.Now,
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(_getClaims(user))
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            var jwt = tokenHandler.WriteToken(securityToken);

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
