using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Manifest.Common.Enums;
using Manifest.Common.Interfaces.Service.App;
using Manifest.Common.Interfaces.Service.Config;
using Manifest.Common.Models.ViewModels;
using Manifest.Common.Models.ViewModels.Account;
using Framework.Infrastructure.Utils;
using Microsoft.IdentityModel.Tokens;

namespace Manifest.Service.App
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly ISystemConfiguration systemConfiguration;

        public TokenGeneratorService(
            ISystemConfiguration systemConfiguration)
        {
            this.systemConfiguration = systemConfiguration;
        }

        public string GetUserTokenAsString(UserModel user, long predefinedCurrencyTypeId)
        {
            var model = GetUserToken(user, predefinedCurrencyTypeId);
            if (model == null)
                return null;
            return model.Token;
        }

        public (string authValue, DateTime validUntil, Exception ex) GetUserInfoFromToken(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(systemConfiguration.CommonConfiguration.JwtSecretKey);
                var handler = new JwtSecurityTokenHandler();
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var claims = handler.ValidateToken(token, validations, out var tokenSecure);

                var authValue = claims.FindFirstValue(ClaimTypes.Name);
                if (authValue.IsNullOrEmpty())
                {
                    return (null, DateTime.MinValue, new Exception("Incorrect Token"));
                }

                return (claims.FindFirstValue(ClaimTypes.Name), tokenSecure.ValidTo, null);
            }
            catch (Exception ex)
            {
                return (null, DateTime.MinValue, ex);
            }
        }

        public LoggedInUserModel CreateExpiringUserToken(UserModel user, long expirationMinutes, List<string> permissions = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(systemConfiguration.CommonConfiguration.JwtSecretKey);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Authentication, (user.PredefinedUserTypeId == (int)PredefinedUserTypes.SuperAdmin).ToString())
        };

            if (permissions != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, string.Join(",", permissions)));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, ""));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new LoggedInUserModel
            {
                Token = tokenString,
            };
        }

        public LoggedInUserModel GetUserToken(UserModel user, long predefinedCurrencyTypeId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(systemConfiguration.CommonConfiguration.JwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Authentication, (user.PredefinedUserTypeId == (int)PredefinedUserTypes.SuperAdmin).ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var result = new LoggedInUserModel
            {
                UserProfileGuid = user.ProfileImageGuid,
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserTypeId = user.PredefinedUserTypeId,
                PredefinedCurrencyTypeId = predefinedCurrencyTypeId,
                Token = tokenHandler.WriteToken(token),
                UserTypeName = ((PredefinedUserTypes)user.PredefinedUserTypeId).ToString().SplitCamelCase(),
            };

            return result;
        }
    }
}