using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PlainClasses.Services.Identity.Application.Commands;
using PlainClasses.Services.Identity.Application.Configurations.Options;

namespace PlainClasses.Services.Identity.Application
{
    public class JwtHandler : IJwtHandler
    {
        private readonly IOptions<JwtOption> _jwtOption;

        public JwtHandler(IOptions<JwtOption> jwtOption)
        {
            _jwtOption = jwtOption;
        }

        public string CreateToken(Guid userId, string fullName, IEnumerable<AuthDto> auths)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, fullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            claims.AddRange(auths.Select(
                    auth => new Claim(ClaimTypes.Role, auth.AuthName)
                )
            );

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var expires = DateTime.Now.AddMinutes(_jwtOption.Value.ExpiryMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Issuer = _jwtOption.Value.Issuer,
                Expires = expires,
                NotBefore = DateTime.Now
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenToReturn = tokenHandler.WriteToken(token);
            return tokenToReturn;
        }
    }
}