export enum LoginTypes {
    SuperAdmin = 1,
    Configurator = 3,
    User = 2,
}

export enum LogoutTypes {
    InActivity = 1,
    Manual = 2,
    Authentication = 3,
}

export enum SidebarLayoutTypes {
    CompanyList = 1,
    CompanyDetail = 2,
    CustomerDetail = 3,
    RuleDetail = 4,
    GlobalSearchDetails = 5,
}

export enum CompanyStatusTypes {
    Active = 1,
    InActive = 2,
}

export enum UserStatusTypes {
    Active = 1,
    InActive = 2,
}


export enum AppPerfLogTimeTypes {
    GreaterThanMS = 1,
    LessThanMS = 2,
    EqualMS = 3,
}

export enum ReportChartTypes {
    BarChart = 1,
    PieChart = 2,
}

export enum BudgetExpenseReportDivisionTypes {
    All = 1,
    PreProduction = 2,
    Production = 3,
    PostProduction = 4,
    OtherBudgetAndExpense = 5,
}

export enum CompanyMenuTypes {
    CompanyInformation = 1,
    Customers = 2,
    Dashboard = 3,
    GeoName = 4,
    AppointmentWithToken = 5,
    Appointment = 6,
    AnalysisRules = 7,
    CompanyConfig = 8,
    BackToList = 9,
}

export enum SuperAdminConfigurationMenuTypes {
    SuperAdminConfiguration = 1,
    Companies = 2,
    Users = 3,
    MailServer = 4,
    SmsServer = 5,
    SingleSignOnServer = 6,
    VoiceNotificationServer = 7,
    WhatsAppNotificationServer = 8,
    MessageTemplate = 9,
    MessageToSend = 10,
    JobTemplate = 11,
    Jobs = 12,
    AuditOperation = 13,
    AppLogs = 14,
    PerfLogs = 15,
}


export enum CustomerMenuTypes {
    CustomerInformation = 1,
    Chart = 2,
    Dasa = 3,
    HouseAndPlanets = 4,
    RuleResult = 5,
    BackToList = 6,
}


export enum RuleDetailMenuTypes {
    RuleInformationHeader = 1,
    RuleInformation = 2,
    BackToList = 3,
}
