import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbCollapseModule, NgbDropdownModule, NgbModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';

import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { FooterComponent } from './footer/footer.component';
import { HorizontalComponent } from './horizontal/horizontal.component';
import { HorizontaltopbarComponent } from './horizontaltopbar/horizontaltopbar.component';
import { LanguageService } from '../../common/services/language/language.service';
import { LayoutComponent } from './layout.component';
import { LayoutService } from './service/layout.service';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from './sidebar/sidebar.component';
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
    HorizontaltopbarComponent,
    HorizontalComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    NgbNavModule,
    BsDropdownModule,
    NgbModule,
    NgbDropdownModule,
    NgbCollapseModule,
    TranslateModule,
    FormsModule, ReactiveFormsModule,
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
    HorizontaltopbarComponent,
    HorizontalComponent,
  ]
})
export class LayoutsModule { }
