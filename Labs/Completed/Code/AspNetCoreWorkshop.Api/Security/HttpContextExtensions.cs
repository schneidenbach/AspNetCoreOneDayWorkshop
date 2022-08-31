using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreWorkshop.Api.Security
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Note: you should NEVER use code like this in a production scenario.
        /// </summary>
        public static string GenerateJwt(this HttpContext context, string role)
        {
            var config = context.RequestServices.GetRequiredService<IConfiguration>();
            var id = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(JwtRegisteredClaimNames.Jti, id),
                new Claim(ClaimTypes.Role, role)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var issuer = config["Tokens:Issuer"];
            
            var token = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                // WARNING: You should not use expiration this long
                expires: DateTime.Now.AddDays(365),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}