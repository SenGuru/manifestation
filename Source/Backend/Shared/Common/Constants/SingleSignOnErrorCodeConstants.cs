namespace Manifest.Common.Constants
{
    public static class SingleSignOnErrorCodeConstants
    {
        public static class UnableToAuthenticateExternalLogin
        {
            public const string ErrorCode = "E1";
            public const string ErrorDescription = "Unable to authenticate the External Login";
        }

        public static class ProviderNotAvailable
        {
            public const string ErrorCode = "E2";
            public const string ErrorDescription = "Provider not available";
        }

        public static class EmailNotAvailable
        {
            public const string ErrorCode = "E3";
            public const string ErrorDescription = "Email not available";
        }

        public static class ServiceNotAvailable
        {
            public const string ErrorCode = "E4";
            public const string ErrorDescription = "Service not available";
        }

        public static class AccountNotAvailable
        {
            public const string ErrorCode = "E5";
            public const string ErrorDescription = "Account not available";
        }

        public static class UnableToAuthenticationToken
        {
            public const string ErrorCode = "E6";
            public const string ErrorDescription = "Unable to validate the login information";
        }

        public static class DisabledUserNotAllowed
        {
            public const string ErrorCode = "E7";
            public const string ErrorDescription = "User access is disabled. Please contact support for help.";
        }

        public static class SuperAdminUserNotAllowed
        {
            public const string ErrorCode = "E8";
            public const string ErrorDescription = "Only non admins allowed with this interface.";
        }

        public static class TokenSignatureDoesNotMatch
        {
            public const string ErrorCode = "E9";
            public const string ErrorDescription = "Authentication Token does not match.";
        }

        public static class UserAuthenticationNotAllowForTheUser
        {
            public const string ErrorCode = "E10";
            public const string ErrorDescription = "You are not subscribed to be validated using this authentication Provider. Please contact support to have your account subscribed to this provider.";
        }
    }
}
