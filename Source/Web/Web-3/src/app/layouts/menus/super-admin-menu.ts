import { MenuItem } from "src/common/models/app-custom-model";

// super admin
export const SUPER_ADMIN_MENU: MenuItem[] = [
  {
    id: 1,
    label: 'MENUITEMS.SUPERADMIN.TEXT',
    isTitle: true,
    isCollapsed: false,
  },
  {
    id: 2,
    label: 'MENUITEMS.COMPANIES.TEXT',
    icon: 'bx bx-building-house',
    link: '/sa/companies',
    isCollapsed: false,
  },
  {
    id: 3,
    label: 'MENUITEMS.USERS.TEXT',
    icon: 'bx bxs-user-pin',
    link: '/sa/users',
    isCollapsed: false,
  },
  {
    id: 4,
    label: 'MENUITEMS.MAILSERVERCONFIG.TEXT',
    icon: 'bx bx-mail-send',
    link: '/sa/mail-server-config',
    isCollapsed: false,
  },
  {
    id: 5,
    label: 'MENUITEMS.SMSSERVERCONFIG.TEXT',
    icon: 'bx bxs-message',
    link: '/sa/sms-server-config',
    isCollapsed: false,
  },
  {
    id: 6,
    label: 'MENUITEMS.SINGLESIGNON.TEXT',
    icon: 'bx bxs-log-in',
    link: '/sa/single-sign-on',
    isCollapsed: false,
  },
  {
    id: 7,
    label: 'MENUITEMS.VOICENOTIFICATION.TEXT',
    icon: 'bx bxs-user-voice',
    link: '/sa/voice-notification',
    isCollapsed: false,
  },
  {
    id: 8,
    label: 'MENUITEMS.WHATSAPPNOTIFICATION.TEXT',
    icon: 'bx bxl-whatsapp',
    link: '/sa/whatsapp-notification',
    isCollapsed: false,
  },
  {
    id: 10,
    label: 'MENUITEMS.MESSAGETEMPLATE.TEXT',
    icon: 'bx bxs-message-dots',
    link: '/sa/message-template',
    isCollapsed: false,
  },
  {
    id: 11,
    label: 'MENUITEMS.MESSAGETOSEND.TEXT',
    icon: 'bx bx-chat',
    link: '/sa/message-to-send',
    isCollapsed: false,
  },
  {
    id: 12,
    label: 'MENUITEMS.JOBTEMPLATES.TEXT',
    icon: 'bx bx-task',
    link: '/sa/job-templates',
    isCollapsed: false,
  },
  {
    id: 13,
    label: 'MENUITEMS.JOBS.TEXT',
    icon: 'bx bx-list-check',
    link: '/sa/jobs',
    isCollapsed: false,
  },
  {
    id: 14,
    label: 'MENUITEMS.AUDIT.TEXT',
    icon: 'bx bx-layer',
    link: '/sa/audit',
    isCollapsed: false,
  },
  {
    id: 15,
    label: 'MENUITEMS.APPLOGS.TEXT',
    icon: 'bx bx-list-ol',
    link: '/sa/app-logs',
    isCollapsed: false,
  },
  {
    id: 16,
    label: 'MENUITEMS.PERFLOGS.TEXT',
    icon: 'bx bxs-report',
    link: '/sa/perf-logs',
    isCollapsed: false,
  },
];

export const ADMIN_COMPANY_MENU: MenuItem[] = [
  {
    id: 1,
    label: 'MENUITEMS.CONFIGURATION.TEXT',
    isTitle: true,
    isCollapsed: false,
  },
  {
    id: 2,
    label: 'MENUITEMS.COMPANYCONFIG.TEXT',
    icon: 'bx bx-cog',
    link: '/sa/company/layoutTypeId/companyId/company-config',
    isCollapsed: false
  },
  {
    id: 3,
    label: 'MENUITEMS.USERS.TEXT',
    icon: 'bx bxs-user-rectangle',
    link: '/sa/company/layoutTypeId/companyId/users',
    isCollapsed: false,
  }, {
    id: 4,
    label: 'MENUITEMS.BACKTOLISTVIEW.TEXT',
    icon: 'mdi mdi-keyboard-return',
    link: `/sa/companies`,
    isCollapsed: false,
  },
];