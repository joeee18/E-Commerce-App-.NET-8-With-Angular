using ECom.Core.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Core.Services
{
    public class GenrationToken : IGenrationToken
    {
        private readonly IConfiguration configuration;

        public GenrationToken(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GetAndCreateToken(AppUser user)
        {
            //Private Claims
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.Email , user.Email)
            };

            var security = configuration["Token:Secret"];
            var key = Encoding.ASCII.GetBytes(security);

            SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(value: 1),
                Issuer = configuration["Token:Issuer"],
                SigningCredentials = credentials,
                NotBefore = DateTime.Now
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

    }
}
