import { MenuItem } from "src/common/models/app-custom-model";

// super admin
export const ADMIN_MENU: MenuItem[] = [
  {
    id: 1,
    label: 'MENUITEMS.SUPERADMIN.TEXT',
    isTitle: true,
    isCollapsed: false,
  },
  {
    id: 2,
    label: 'MENUITEMS.COMPANYCONFIG.TEXT',
    icon: 'bx bxs-buildings',
    link: '/admin/company-config',
    isCollapsed: false,
  },
  {
    id: 3,
    label: 'MENUITEMS.USERS.TEXT',
    icon: 'bx bxs-user-rectangle',
    link: '/admin/users',
    isCollapsed: false,
  },
];