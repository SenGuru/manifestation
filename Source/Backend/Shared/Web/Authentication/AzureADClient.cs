using Newtonsoft.Json.Linq;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;
using RestSharp.Authenticators;

namespace OAuth2.Client.Impl
{
    public class AzureADClient : OAuth2Client
    {
        public AzureADClient(IRequestFactory factory, IClientConfiguration configuration, string tenantId)
            : base(factory, configuration)
        {
            TenantId = tenantId;
        }

        public string TenantId { get; set; }

        public override string Name
        {
            get { return "AzureAD"; }
        }

        protected override Endpoint AccessTokenServiceEndpoint
        {
            get
            {
                //https://login.microsoftonline.com/common/oauth2/v2.0/token
                return new Endpoint
                {
                    BaseUri = $"https://login.microsoftonline.com/common/oauth2/v2.0",
                    Resource = "/token"
                };
            }
        }

        protected override Endpoint UserInfoServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://graph.microsoft.com",
                    Resource = "/oidc/userinfo"
                };
            }
        }

        protected override Endpoint AccessCodeServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = $"https://login.microsoftonline.com/common/oauth2/v2.0",
                    Resource = "/authorize"
                };
            }
        }

        protected override void BeforeGetAccessToken(BeforeAfterRequestArgs args)
        {
            string grantTypeToken = args.Parameters["refresh_token"] != null ? "refresh_token" : "authorization_code";

            args.Request.AddObject(new
            {
                client_secret = args.Configuration.ClientSecret,
                client_id = args.Configuration.ClientId,
                grant_type = grantTypeToken,
                code = args.Parameters["refresh_token"] ?? args.Parameters["code"],
                redirect_uri = args.Configuration.RedirectUri
            });
#pragma warning disable CS0618 // Type or member is obsolete
            args.Request.RequestFormat = RestSharp.DataFormat.Json;
#pragma warning restore CS0618 // Type or member is obsolete
        }

        protected override void BeforeGetUserInfo(BeforeAfterRequestArgs args)
        {
            args.Client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(AccessToken, "Bearer");
#pragma warning disable CS0618 // Type or member is obsolete
            args.Request.RequestFormat = RestSharp.DataFormat.Json;
#pragma warning restore CS0618 // Type or member is obsolete
        }

        protected override UserInfo ParseUserInfo(string content)
        {
            var response = JObject.Parse(content);

            if (response == null)
            {
                return new UserInfo();
            }

            var userinfo = new UserInfo();

            if (response.TryGetValue("givenname", out var firstNameToken) && firstNameToken.Type == JTokenType.String)
            {
                userinfo.FirstName = firstNameToken.Value<string>();
            }

            if (response.TryGetValue("familyname", out var lastNameToken) && lastNameToken.Type == JTokenType.String)
            {
                userinfo.LastName = lastNameToken.Value<string>();
            }

            if (response.TryGetValue("email", out var emailToken) && emailToken.Type == JTokenType.String)
            {
                userinfo.Email = emailToken.Value<string>();
            }

            return userinfo;
        }
    }
}