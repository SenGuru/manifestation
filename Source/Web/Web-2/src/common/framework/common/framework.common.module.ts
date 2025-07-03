import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BaseHttpClientService } from './infrastructure/http/base-http-client.service';
import { CommonModule } from '@angular/common';
import { ConfigService } from './infrastructure/config/config.service';
import { ConsoleLogWriter } from './infrastructure/log/writer/console-log.writer';
import { ElectronShellLogWriter } from './infrastructure/log/writer/electron-shell-log.writer';
import { FrameworkHttpClientService } from './infrastructure/http/framework-http-client.service';
import { FrameworkServiceApiService } from './infrastructure/http/framework-service-api.service';
import { GlobalExceptionHandlerService } from './infrastructure/error/global.exception.handler.service';
import { InMemoryLogWriter } from './infrastructure/log/writer/in-memory-log.writer';
import { LogPublishersService } from './infrastructure/log/log-publishers.service';
import { LogService } from './infrastructure/log/log.service';
import { NgModule } from '@angular/core';
import { WebApiLogWriter } from './infrastructure/log/writer/webapi-log.writer';

@NgModule({
  declarations: [
  ],
  imports: [
      CommonModule,
      FormsModule,
      ReactiveFormsModule],
  exports: [
      FormsModule,
      ReactiveFormsModule],
  providers: [
      ConfigService,
      GlobalExceptionHandlerService,
      BaseHttpClientService,
      FrameworkHttpClientService,
      FrameworkServiceApiService,
      LogService,
      LogPublishersService,
      ConsoleLogWriter,
      ElectronShellLogWriter,
      InMemoryLogWriter,
      WebApiLogWriter
  ]
})
export class FrameworkCommonModule {}
