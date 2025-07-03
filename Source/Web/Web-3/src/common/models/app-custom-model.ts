import { ApexAxisChartSeries, ApexChart, ApexDataLabels, ApexFill, ApexGrid, ApexLegend, ApexMarkers, ApexNonAxisChartSeries, ApexPlotOptions, ApexResponsive, ApexStroke, ApexTitleSubtitle, ApexXAxis, ApexYAxis } from "ng-apexcharts";
import { AppLogModel, UserModel } from "./app-model";

export interface Status {
  id: number | null;
  type: string | undefined;
}

export interface KeyValueString {
  key: string | undefined;
  value: string | undefined;
}

export interface TextValueString {
  text: string | undefined;
  value: string | undefined;
}

export interface AuditDuoColumnModel {
  text: string | undefined;
  align: string | undefined;
  sortable: boolean;
  value: string | undefined;
  width: string | undefined;
}

export interface MenuItem {
  id?: number;
  label?: string | undefined;
  icon?: string | undefined;
  isCollapsed?: boolean;
  isUiElement?: boolean;
  link?: string | undefined;
  subItems?: MenuItem[];
  isTitle?: boolean;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  badge?: any;
  parentId?: number;
  isLayout?: boolean;
  collapseid?: string | undefined;
}

export class MessageOption {
  public title!: string | undefined;
  public message!: string | undefined;
}

export class AppLayoutModel {
  LAYOUT_MODE: string = "";
  DATA_LAYOUT: string = "";
  LAYOUT_WIDTH: string = "";
  SIDEBAR_MODE: string = "";
  TOPBAR_TYPE: string = "";
}

export class ChartOptions {
  public series?: ApexAxisChartSeries | ApexNonAxisChartSeries;
  public chart?: ApexChart;
  public xaxis?: ApexXAxis;
  public dataLabels?: ApexDataLabels;
  public grid?: ApexGrid;
  public fill?: ApexFill;
  public legend?: ApexLegend;
  public stroke?: ApexStroke;
  public colors?: string[];
  public title?: ApexTitleSubtitle;
  public responsive?: ApexResponsive[];
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public labels?: any;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public tooltip?: any;
  public plotOptions?: ApexPlotOptions;
  public yaxis?: ApexYAxis;
  public markers?: ApexMarkers;
};

export interface UploadFile {
  name: string | undefined;
  type: string | undefined;
  size: string | undefined;
}

export type CompanyInfoHeaderTabTypes = 'info' | 'storage-config' | 'appointment-list' | 'appointment-calendar' | 'company-licenses';

export type CustomerInfoHeaderTabTypes = 'info';

export type RuleInfoHeaderTabTypes = 'rule-editor';

export type UtcToBrowserTimeDateFormatType = 'split' | 'formatted';

export type GlobalSearchFeature = 'company' | 'user' | 'customer' | 'rule' | 'appointment';

export type CompanyFileStorageConfigurationStringTypes = 'report-server' | 'profile-picture' | 'file-attachments';

export type ScreenActionTypes = 'create' | 'edit' | 'view';

export type LogSeverityType = "CRITICAL" | "TRACE" | "ERROR" | "WARNING" | "INFO" | "FATAL" | "DEBUG" | "TRANS-BEGIN" | "TRANS-COMMIT" | "TRANS-ROLLBACK" | "SQL" | "SQL-ERROR";

export type MessageTemplateTabs = 'email' | 'sms' | 'notification';

export type PhoneNumberValidationType = 'work-phone' | 'cell-phone' | 'assistant-work-phone';

export type LightAndDarkModeTypes = 'light' | 'dark';

export class GlobalSearchRecordsCount {
  public companyCount: number | null = 0;
  public userCount: number | null = 0;
  public ruleCount: number | null = 0;
  public customerCount: number | null = 0;
  public appointmentCount: number | null = 0;
}

export class AppLogExtendedModel extends AppLogModel {
  public isLogCache: boolean = false;

  public static override getCopy(self?: AppLogModel): AppLogExtendedModel {
    const result = new AppLogExtendedModel();
    Object.assign(result, self);
    result.isLogCache = false;
    return result;
  }

  public static getCopyArray(self: AppLogModel[]): AppLogExtendedModel[] {
    const result: AppLogExtendedModel[] = [];
    for (let i = 0; i < self.length; i++) {
      const log = this.getCopy(self[i]);
      result.push(log);
    }
    return result;
  }
}

export class ChatUserCustomModel {
  public chatGroupName: string | null = '';
  public userIds: number[] = [];
}

export class ChatUserGroupCustomModel {
  public group: string | null = '';
  public users: UserModel[] = [];
}

export class StorageConnectionStringModel {
  public id: number = 0;
  public type: string | undefined;
  public initialPath: string | undefined;
}
