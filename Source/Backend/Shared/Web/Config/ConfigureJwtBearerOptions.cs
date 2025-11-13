using System.Text;
using Manifest.Common.Interfaces.Service.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Manifest.Web.Config
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly ICommonConfiguration commonConfiguration;

        public ConfigureJwtBearerOptions(ICommonConfiguration commonConfiguration)
        {
            this.commonConfiguration = commonConfiguration;
        }

        public void PostConfigure(string name, JwtBearerOptions options)
        {
            var key = Encoding.ASCII.GetBytes(commonConfiguration.JwtSecretKey);
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            /*
            options.Events.OnTokenValidated = (context) =>
            {
                return Task.CompletedTask;
            };
            */
        }
    }
}
