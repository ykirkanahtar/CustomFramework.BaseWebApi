using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CustomFramework.BaseWebApi.Authorization
{
    public static class JwtManager
    {

        public static string GenerateToken(List<Claim> claims, string key, string issuer, string audience, out DateTime expireDateTime, out DateTime expireUtcDateTime, int expireInminutes = 20)
        {
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            expireUtcDateTime = DateTime.UtcNow.AddMinutes(expireInminutes);
            expireDateTime = DateTime.Now.AddMinutes(expireInminutes);
            
            var securityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expireDateTime,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}