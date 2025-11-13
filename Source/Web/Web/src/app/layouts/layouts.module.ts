import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbCollapseModule, NgbDropdownModule, NgbModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';

import { ChatComponent } from './chat/chat.component';
import { FooterComponent } from './footer/footer.component';
import { LanguageService } from '../../common/services/language/language.service';
import { LayoutComponent } from './layout.component';
import { LayoutService } from './service/layout.service';
import { LightboxModule } from 'ngx-lightbox';
import { NotificationCardComponent } from '../shared/components/notification-card/notification-card.component';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from './sidebar/sidebar.component';
import { SimplebarAngularModule } from 'simplebar-angular';
import { TopbarComponent } from './topbar/topbar.component';
import { TranslateModule } from '@ngx-translate/core';
import { VerticalComponent } from './vertical/vertical.component';

// Component pages

@NgModule({
  declarations: [
    LayoutComponent,
    VerticalComponent,
    TopbarComponent,
    SidebarComponent,
    FooterComponent,
    ChatComponent,
    NotificationCardComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    NgbNavModule,
    NgbModule,
    NgbDropdownModule,
    NgbCollapseModule,
    SimplebarAngularModule,
    TranslateModule,
    FormsModule, ReactiveFormsModule,
    LightboxModule,
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
  providers: [LanguageService, DatePipe, LayoutService],
  exports: [
    LayoutComponent,
    VerticalComponent,
    TopbarComponent,
    SidebarComponent,
    FooterComponent,
    NotificationCardComponent,
    ChatComponent,
  ]
})
export class LayoutsModule { }
