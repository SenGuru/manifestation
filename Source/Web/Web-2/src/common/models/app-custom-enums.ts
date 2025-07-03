
export enum LoginTypes {
    SuperAdmin = 1,
    Configurator = 2,
    User = 3,
}

export enum LogoutTypes {
    InActivity = 1,
    Manual = 2,
    Authentication = 3,
}

export enum AppScreenLayoutTypes {
    SuperAdminWithCompanyList = 1,
    CompanyDetail = 2,
    ProjectDetail = 3,
    ProjectTemplateDetail = 4,
    GlobalSearchDetails = 5,
    UserDetail = 6,
}

export enum CompanyStatusTypes {
    Active = 1,
    InActive = 2,
}

export enum UserStatusTypes {
    Active = 1,
    InActive = 2,
}

export enum ProductStatusTypes {
    Active = 1,
    InActive = 2,
}

export enum ActiveStatusTypes {
    Active = 1,
    NotActive = 2,
}

export enum AppPerfLogTimeTypes {
    GreaterThanMS = 1,
    LessThanMS = 2,
    EqualMS = 3,
}

export enum SuperAdminConfgurationSidebarMenus {
    Companies = 1,
    Users = 2,
    MailServer = 3,
    SmsServer = 4,
    SingleSignOn = 5,
    PushNotification = 6,
    VoiceNotification = 7,
    WhatsappNotification = 8,
    MessageTemplates = 9,
    MessageToSend = 10,
    JobTemplates = 11,
    Jobs = 12,
    Audit = 13,
    AppLogs = 14,
    PerfLogs = 15,
}

export enum SuperAdminCompanySidebarMenus {
    dashboard = 1,
    CompanyEntity = 2,
    Projects = 3,
    MyTasks = 4,
    Templates = 5,
    Reports = 6,
    Users = 7,
}

export enum SuperAdminProjectSidebarMenus {
    Information = 1,
    Controls = 2,
    Approvals = 3,
    Reviews = 4,
}

export enum SuperAdminProjectTemplateSidebarMenus {
    Information = 1,
    Workflow = 2,
    CustomFields = 3,
}

export enum ReportChartTypes {
    BarChart = 1,
    PieChart = 2,
}