import { Component, OnInit } from '@angular/core';

import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { SystemSummaryModel } from 'src/common/models/app-model';

@Component({
    selector: 'app-super-admin-header',
    templateUrl: './super-admin-header.component.html',
    styleUrl: './super-admin-header.component.scss',
    standalone: false
})
export class SuperAdminHeaderComponent implements OnInit {

  public systemSummaryModel: SystemSummaryModel | undefined;
  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
  ) {

  }

  public ngOnInit(): void {
    this.getSystemSummaries();
  }

  public getSystemSummaries(): void {
    this.httpService.getAppDetails().subscribe({
      next: result => {
        if (!result.isSuccess) return this.dialogService.errorWithReturnModel("Unable to get System Summaries", result);
        if (result.model) this.systemSummaryModel = result.model;
        return;
      },
      error: (err: unknown) => this.dialogService.error("Unable to get System Summaries"),
      complete: () => null,
    });
  }

}
