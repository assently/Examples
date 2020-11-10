using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Assently.Samples.DotnetCore.Services
{
    public class IdentityTokenService
    {
        private readonly byte[] _keyBytes;
        private readonly string _customerName;
        private readonly string _identityIssuer;

        public IdentityTokenService(IConfiguration configuration)
        {
            var secret = configuration.GetSection("JwtConfig").GetSection("identitySecret").Value;
            _keyBytes = Encoding.UTF8.GetBytes(secret);
            _customerName = configuration.GetSection("JwtConfig").GetSection("customerName").Value;
            _identityIssuer = configuration.GetSection("JwtConfig").GetSection("identityIssuer").Value;
        }

        public JwtSecurityToken ValidateToken(string token, string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),
                ValidateIssuer = true,
                ValidIssuer = _identityIssuer, 
                ValidAudience = _customerName,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                LifetimeValidator = (before, expires, jwttoken, validationParameters) =>
                {
                    var now = DateTime.UtcNow.Add(validationParameters.ClockSkew);
                    if (before.HasValue && now < before.Value)
                    {
                        return false;
                    }

                    if (expires.HasValue && expires <= now)
                    {
                        return false;
                    }

                    return true;
                }
            };

            tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

            if (ValidateCorrelatedTokens(validatedToken as JwtSecurityToken, authToken))
                return validatedToken as JwtSecurityToken;
            
            return null;
        }

        private bool ValidateCorrelatedTokens(JwtSecurityToken identityToken, string authTokenString)
        {
            var authToken = new JwtSecurityToken(authTokenString);
            var authJwtId = authToken.Claims.First(c => c.Type == "jti").Value;
            var identityTokenAuthJwtId = identityToken.Claims.First(c => c.Type == "auth_jti").Value;
            if (authJwtId == identityTokenAuthJwtId)
            {
                return true;
            }

            return false;
        }
    }
}