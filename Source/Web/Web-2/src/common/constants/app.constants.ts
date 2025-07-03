
export const STORAGE_KEYS = {
    CURRENT_USER_LOCAL_STORAGE_NAME: 'currentUser',
    PERMISSION_ENABLE_DISABLE_OPTION_STORAGE_NAME: 'permissionEnableDisableOption'
}

export const SESSION_KEYS = {
    COMPANY_INFORMATION_SESSION_STORAGE_NAME: 'companyInformationSession',
    COMPANY_SUMMARY_INFORMATION_SESSION_STORAGE_NAME: 'companySummaryInformationSession',
    SYSTEM_SUMMARY_INFORMATION_SESSION_STORAGE_NAME: 'systemSummaryInformationSession',
}

export const ADMIN_MENU_INITIAL_URL_REPLACEMENT_NAME = "admin";

export const COUNTRY_ID_USA = 1;
export const COUNTRY_ID_INDIA = 80;
export const COUNTRY_TEXT_INDIA = 'India';

export const ALLOWED_ALL_FILE_EXTENSIONS: string = '.doc,.docx,.pdf,.txt,.xlsx,.xls,.png,.jpeg,.jpg';
export const ALLOWED_PROFILE_IMAGE_EXTENSIONS: string = '.png,.jpeg,.jpg,.webp';
export const ALLOWED_TEMPLATE_FILE_EXTENSIONS: string = '.doc,.docx';
export const MINIMUM_PASSWORD_CHARACTER_LENGTH = 8;
export const MAXIMUM_PASSWORD_CHARACTER_LENGTH = 100;

export const COMMON_POCO_FIELDS: string[] = [
    "createdDate",
    "modifiedDate",
    "createdByUserId",
    "modifiedByUserId",
    "createdByUserFirstName",
    "createdByUserLastName",
    "createdByUserEmail",
    "modifiedByUserFirstName",
    "modifiedByUserLastName",
    "modifiedByUserEmail",
    "createdBy",
    "modifiedBy",
    "isDeleted",
    'existingArtistRates',
    'existingTechnicianRates',
    'existingEquipmentRates',    
    'existingVendorRates',  
];
