namespace Manifest.Common.Constants
{
    public static class StringConstants
    {
        public static readonly string[] CommonPocoFields = { "CreatedDate", "ModifiedDate", "CreatedByUserId", "ModifiedByUserId", "CreatedByUserFirstName", "CreatedByUserLastName", "CreatedByUserEmail", "ModifiedByUserFirstName", "ModifiedByUserLastName", "ModifiedByUserEmail", "CreatedBy", "ModifiedBy", "IsDeleted" };
        public static readonly string[] PositiveTelecomStatusCallBackNames = { "completed", "sent", "delivered" };
        public static readonly string[] NegativeTelecomStatusCallBackNames = { "canceled", "undelivered", "failed", "no-answer" };

        public static class Config
        {
            public const string DbLocation = "dbLocation";
            public const string Server = "server";
            public const string UserName = "userName";
            public const string Password = "password";
        }

        public static class PrefixForCode
        {
            public const string Artist = "ARC";
            public const string Technician = "TEC";
            public const string Equipment = "EQC";
            public const string Vendor = "VQC";
            public const string Property = "PRC";

            public const string Movie = "MOVC";
            public const string MovieArtist = "MARC";
            public const string MovieTechnician = "MTEC";
            public const string MovieEquipment = "MEQC";
            public const string MovieVendor = "MVQC";
            public const string MovieUnit = "MU";
            public const string MovieProperty = "MPC";
            public const string MovieSceneProperty = "MSPC";
            public const string MovieShootDayProperty = "MSDPC";

            public const string MovieShootDay = "MSDC";
            public const string MovieLocation = "MLC";
            public const string MovieScene = "MSCC";

            public const string MovieShootDayScene = "MSDSC";
            public const string MovieShootDayRequirement = "MSDRC";
            public const string MovieShootDayArtist = "MSDARC";
            public const string MovieShootDayTechnician = "MSDTEC";
            public const string MovieShootDayEquipment = "MSDEQC";
            public const string MovieShootDayVendor = "MSDVQC";
            public const string MovieShootDayExpense = "MSDEXC";

            public const string MovieBudgetDivision = "MBDC";
            public const string MovieBudgetCategory = "MBCC";

            public const string Company = "COC";

            public const string MailServer = "MSC";
            public const string SmsServer = "SMSC";
            public const string SingleSignOnServer = "SSC";
            public const string PushNotificationServer = "PNSC";
            public const string VoiceNotificationServer = "VNSC";
            public const string WhatsAppNotificationServer = "WNSC";
        }

        public static class TallyLedgerColumnNames
        {
            public const string Name = "Name";
            public const string GroupName = "Group Name";
        }

        public static class TallyVoucherColumnNames
        {
            public const string VoucherDate = "Voucher Date";
            public const string VoucherTypeName = "Voucher Type Name";
            public const string VoucherNumber = "Voucher Number";
            public const string LedgerName = "Ledger Name";
            public const string LedgerAmount = "Ledger Amount";
            public const string LedgerAmountDrOrCr = "Ledger Amount Dr/Cr";
        }
    }
}
