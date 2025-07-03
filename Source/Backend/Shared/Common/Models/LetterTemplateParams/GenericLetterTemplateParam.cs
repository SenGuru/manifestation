using System.ComponentModel;

namespace Manifest.Common.Models.LetterTemplateParams
{
    public class GenericLetterTemplateParam : BaseLetterTemplateParam
    {
        [Description("Today's date ex. 1/1/2023")]
        public string TODAY { get; set; }

        [Description("Today + 30 days date ex. 1/31/2023")]
        public string TODAY_PLUS30 { get; set; }

        [Description("Today + 60 days date ex. 3/2/2023")]
        public string TODAY_PLUS60 { get; set; }

        [Description("Today + 90 days date ex. 4/1/2023")]
        public string TODAY_PLUS90 { get; set; }

        [Description("Overall status of the Case. For Credentialing and Contracting status, pls use respective tags")]
        public string STATUS { get; set; }

        [Description("Credentialing Case Status + SubStatus")]
        public string CRED_STATUS { get; set; }

        [Description("Credentialing Case Status")]
        public string CONTRACTING_STATUS { get; set; }

        [Description("Credentilaing Committee date of decision")]
        public string CC_DECISION_DATE { get; set; }

        [Description("Credentilaing Committee Reason for denial of case")]
        public string DENIAL_REASON { get; set; }

        [Description("Expected due date for case completion as per configured SLA")]
        public string DUE_DATE { get; set; }

        [Description("Recredentialing date set to the profile")]
        public string NEXT_RECREDENTIAL_DATE { get; set; }

        [Description("Contact name from the payer side")]
        public string PAYER_CONTACT_NAME { get; set; }

        [Description("Contact number from the payer side")]
        public string PAYER_CONTACT_NUMBER { get; set; }

        [Description("Contact email address from the payer side")]
        public string PAYER_EMAIL { get; set; }

        [Description("Contact address from the payer side in single line format")]
        public string PAYER_ADDRESS_SINGLE_LINE { get; set; }

        [Description("Contact address from the payer side in multi line format")]
        public string PAYER_ADDRESS_MULTI_LINE { get; set; }

        [Description("Contact FAX number from the payer side")]
        public string PAYER_FAX_NUMBER { get; set; }

        [Description("NPI Type of the provider, viz., Individual or Organization")]
        public string NPI_TYPE { get; set; }

        [Description("NPI number of the provider")]
        public string NPI { get; set; }

        [Description("Provider number assigned internally in the payer system of reference")]
        public string PROVIDER_ID { get; set; }

        [Description("Name of the Provider")]
        public string PROVIDER_NAME { get; set; }

        [Description("Provider mailing address in single line format")]
        public string PROVIDER_MAILING_ADDRESS_SINGLE_LINE { get; set; }

        [Description("Provider mailing address in multi line format")]
        public string PROVIDER_MAILING_ADDRESS_MULTI_LINE { get; set; }

        [Description("Provider primary service location address in single line format")]
        public string PROVIDER_SVC_ADDRESS_SINGLE_LINE { get; set; }

        [Description("Provider primary service location address in multi line format")]
        public string PROVIDER_SVC_ADDRESS_MULTI_LINE { get; set; }

        [Description("Provider Participation start date")]
        public string PROVIDER_PAR_START { get; set; }

        [Description("Provider Participation end date")]
        public string PROVIDER_PAR_END { get; set; }

        [Description("Provider Network approval date")]
        public string NETWORK_APPROVAL_DATE { get; set; }

        [Description("List of all Provider's active license type, license number, State (if available) in table format")]
        public LetterTemplateTable ALL_LICENSES_TBL { get; set; }

        [Description("Provider's DEA license number if available in the records")]
        public string DEA_LIC_NUM { get; set; }

        [Description("Provider's Medicaid license number if available in the records")]
        public string MEDICAID_LIC_NUM { get; set; }

        [Description("Provider's Medicare license number if available in the records")]
        public string MEDICARE_UPN_NUM { get; set; }

        [Description("List of all Provider's active Education , license number, Graduation Date (if available) in table format")]
        public LetterTemplateTable ALL_EDUCATIONS_TBL { get; set; }

        [Description("Provider's Primary specialty")]
        public string SPECIALTY_PRIMARY { get; set; }

        [Description("Provider's Secondary and Alternate specialty")]
        public string SPECIALTY_NON_PRIMARY { get; set; }

        [Description("List of all Provider's active Education , license number, Graduation Date (if available) in table format")]
        public LetterTemplateTable ALL_SPECIALTY_TBL { get; set; }

        [Description("Provider primary service location address in single line format")]
        public string PROVIDER_PRIMARY_SVC_ADDRESS_SINGLE_LINE { get; set; }

        [Description("Provider primary service location address in multi line format")]
        public string PROVIDER_PRIMARY_SVC_ADDRESS_MULTI_LINE { get; set; }

        [Description("List of all Provider's active service location address in table format")]
        public LetterTemplateTable PROVIDER_ALL_SVC_ADDRESS_TBL { get; set; }

        [Description("List of all Provider's active insurance policy details in table format (Policy number, Insurance Carrier)")]
        public LetterTemplateTable PROVIDER_INSURANCE_POLICIES_TBL { get; set; }

        [Description("List of all Provider's active hospital affliation details in table format (Affliation Type, Hospital Name, Start date (if available) )")]
        public LetterTemplateTable HOSPITAL_AFFILATION_TYPE_TBL { get; set; }

        [Description("List of all Provider's Accreditation details in table format (Agency, Audit/Survey date, Cerificate number (if available)")]
        public LetterTemplateTable PROVIDER_ACCREDITATION_TBL { get; set; }

        [Description("Last site visit details in table format (Site Visit Date, Deficiency Notes, Follow-up date)")]
        public LetterTemplateTable PROVIDER_LAST_SITE_VISIT_TBL { get; set; }
    }
}
