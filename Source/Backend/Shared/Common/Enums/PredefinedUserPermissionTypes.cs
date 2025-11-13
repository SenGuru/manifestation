namespace Manifest.Common.Enums
{
    public enum PredefinedUserPermissionTypes
    {
        //Super Admin
        ConfigureSystem = 2600001,

        //Configurator
        ConfigureCompany = 2600011,
        ViewCompany = 2600012,

        //Users
        ManageUser = 2600221,

        //CallNotification
        ManageCallNotification = 2600231,
        ViewCallNotification = 2600232,

        //PaymentStatus
        ManagePaymentStatus = 2600241,
        ViewPaymentStatus = 2600242,

        //Analytics
        ViewAnalytics = 2600251,

        //Chat
        ViewChat = 2600261,
    }
}
