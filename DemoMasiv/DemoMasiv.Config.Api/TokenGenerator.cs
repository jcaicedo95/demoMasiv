using DemoMasiv.Config.Api.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;

using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoMasiv.Config.Api
{
    public class TokenGenerator
    {
        public static String Generate(String userName, int userId, bool AdminRole = true)
        {
            byte[] key = GetSecretKey();
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[] {
                                            new Claim(ClaimTypes.Name, userName),
                                            new Claim(ClaimTypes.PrimarySid, userId.ToString()),
                                            new Claim(ClaimTypes.Role, $"{AdminRole}")
                                        });
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor 
            { 
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken createdToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(createdToken);
        }

        public static byte[] GetSecretKey()
            => Encoding.ASCII.GetBytes(System.Environment.GetEnvironmentVariable("SecretKey"));
    }
}
