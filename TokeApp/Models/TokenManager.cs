using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace TokeApp.Models
{
    public class TokenManager
    {
        private static readonly string secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

        private static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                {
                    return null;
                }

                var key = Convert.FromBase64String(secret);
                var parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                return tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);
            }
            catch
            {
                return null;
            }
        }

        public static string GenerateToken(string username)
        {
            var key = Convert.FromBase64String(secret);
            var securityKey = new SymmetricSecurityKey(key);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public static string ValidateToken(string token)
        {            
            var principal = GetPrincipal(token);
            if (principal == null)
            {
                return null;
            }

            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity) principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);            
            return usernameClaim.Value;
        }
    }
}