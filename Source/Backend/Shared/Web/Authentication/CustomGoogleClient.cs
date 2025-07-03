using Newtonsoft.Json.Linq;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;

namespace OAuth2.Client.Impl
{
    public class CustomGoogleClient : OAuth2Client
    {
        public CustomGoogleClient(IRequestFactory factory, IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }

        public override string Name
        {
            get { return "Google"; }
        }

        protected override Endpoint AccessCodeServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://accounts.google.com",
                    Resource = "/o/oauth2/auth"
                };
            }
        }

        protected override Endpoint AccessTokenServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://accounts.google.com",
                    Resource = "/o/oauth2/token"
                };
            }
        }

        protected override Endpoint UserInfoServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://www.googleapis.com",
                    Resource = "/oauth2/v1/userinfo"
                };
            }
        }

        protected override UserInfo ParseUserInfo(string content)
        {
            var response = JObject.Parse(content);

            if (response == null)
            {
                return new UserInfo();
            }

            var userinfo = new UserInfo();
            string avatarUri = "";

            if (response.TryGetValue("id", out var id) && id.Type == JTokenType.String)
            {
                userinfo.Id = id.Value<string>();
            }

            if (response.TryGetValue("email", out var email) && email.Type == JTokenType.String)
            {
                userinfo.Email = email.Value<string>();
            }

            if (response.TryGetValue("name", out var name) && name.Type == JTokenType.String)
            {
                userinfo.LastName = name.Value<string>();
            }

            if (response.TryGetValue("picture", out var picture) && picture.Type == JTokenType.String)
            {
                avatarUri = picture.Value<string>();
            }

            const string avatarUriTemplate = "{0}?sz={1}";

            userinfo.AvatarUri.Small = !string.IsNullOrWhiteSpace(avatarUri) ? string.Format(avatarUriTemplate, avatarUri, 36) : string.Empty;
            userinfo.AvatarUri.Normal = avatarUri;
            userinfo.AvatarUri.Large = !string.IsNullOrWhiteSpace(avatarUri) ? string.Format(avatarUriTemplate, avatarUri, 300) : string.Empty;

            return userinfo;
        }
    }
}