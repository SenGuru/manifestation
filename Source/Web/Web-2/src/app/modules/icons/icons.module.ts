import { IconArrowBack, IconArrowBackUp, IconBell, IconBellBolt, IconBellRinging, IconBrandWhatsapp, IconBriefcase, IconBuilding, IconBuildingSkyscraper, IconCalendarEvent, IconCategory, IconCategory2, IconChartDots, IconChecklist, IconChecks, IconChevronLeft, IconChevronRight, IconCloudComputing, IconCopyCheck, IconDeviceMobileMessage, IconEdit, IconEye, IconEyeClosed, IconFileAnalytics, IconFileCode, IconFileDescription, IconFileInfo, IconFileStack, IconHome, IconInfoSquare, IconList, IconListDetails, IconLogin, IconMail, IconMailCog, IconMapPin, IconMessage, IconMessage2Code, IconMessages, IconMicrophone, IconMicroscope, IconPencil, IconPhone, IconPlus, IconReportAnalytics, IconSearch, IconSettingsCode, IconSettingsUp, IconStack, IconStack2, IconTrash, IconUserCircle, IconUserSquare, IconUsers, IconWritingSign } from 'angular-tabler-icons/icons';

import { NgModule } from '@angular/core';
import { TablerIconsModule } from 'angular-tabler-icons';

// Select some icons (use an object, not an array)
const icons = {
  IconEye,
  IconEyeClosed,
  IconPencil,
  IconTrash,
  IconEdit,
  IconPlus,
  IconSearch,
  IconBell,
  IconMail,
  IconBuilding,
  IconUserSquare,
  IconHome,
  IconChevronRight,
  IconChevronLeft,
  IconCloudComputing,
  IconMailCog,
  IconDeviceMobileMessage,
  IconWritingSign,
  IconLogin,
  IconBellRinging,
  IconMicrophone,
  IconBriefcase,
  IconMessages,
  IconMessage2Code,
  IconBrandWhatsapp,
  IconListDetails,
  IconStack2,
  IconList,
  IconUserCircle,
  IconFileAnalytics,
  IconUsers,
  IconReportAnalytics,
  IconFileCode,
  IconFileStack,
  IconBuildingSkyscraper,
  IconArrowBackUp,
  IconInfoSquare,
  IconChecks,
  IconSettingsCode,
  IconChecklist,
  IconMapPin,
  IconPhone,
  IconCategory,
  IconFileDescription,
  IconCalendarEvent,
  IconChartDots,
  IconCategory2,
};

@NgModule({
  declarations: [],
  imports: [
    TablerIconsModule.pick(icons)
  ],
  exports: [
    TablerIconsModule,
  ]
})
export class IconsModule { }
