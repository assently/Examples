using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Assently.Samples.DotnetCore.Models.CoreId;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Assently.Samples.DotnetCore.Services
{
    public class AuthorizationRequestTokenService
    {
        private readonly string _authSecret;
        private readonly string _host;
        private readonly string _displayName;
        private readonly string _customerName;
        private readonly int _expirationInMinutes;

        public AuthorizationRequestTokenService(IConfiguration config)
        {
            _host = config.GetSection("JwtConfig").GetSection("host").Value;
            _displayName = config.GetSection("JwtConfig").GetSection("displayName").Value;
            _customerName = config.GetSection("JwtConfig").GetSection("customerName").Value;
            _authSecret = config.GetSection("JwtConfig").GetSection("authSecret").Value;
            _expirationInMinutes = int.Parse(config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value);
        }

        public string CreateToken()
        {
            // The time at which the token was issued.
            var issuedAt = (long) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var claims = new List<Claim>
            {
                new Claim("dnm", _displayName),
                new Claim("jti", Guid.NewGuid().ToString()), // "jti" (Jwt Id) is a mandatory token identifier. Should be a GUID4.
                new Claim("hst", _host), //Has to match host of where the CoreID Client is being loaded.
                new Claim("iat", issuedAt.ToString()),
                
                new Claim("aud", Aud.SeBankId), // Add every provider you accept
                new Claim("response_mode", "form_post"), //Applies to providers using redirect
                new Claim("redirect_uri", "https://localhost:5001/Authentication/OidcReturn") //Applies to providers using redirect
            };

            // Expiration time for the token, this is how long the token can be used to start an authentication session.
            var expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_expirationInMinutes));

            var jwt = new JwtSecurityToken(
                issuer: _customerName,
                claims: claims,
                expires: expires,
                signingCredentials:
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSecret)),
                    SecurityAlgorithms.HmacSha256
                )
            );

            var jwtHandler = new JwtSecurityTokenHandler();
            return jwtHandler.WriteToken(jwt);
        }
    }
}