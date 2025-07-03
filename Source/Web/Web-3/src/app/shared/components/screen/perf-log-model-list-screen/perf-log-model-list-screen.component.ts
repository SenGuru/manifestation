import { AuditDuoColumnModel, Status } from 'src/common/models/app-custom-model';
import { Component, Input, OnInit } from '@angular/core';
import { PerfLogModel, PerfLogSearchCriteria } from 'src/common/models/app-model';

import { AppPerfLogTimeTypes } from 'src/common/models/app-custom-enums';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from 'src/app/shared/services/crud.service';
import { CustomSelectService } from 'src/common/services/custom-select-service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { SessionService } from 'src/common/services/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';

@Component({
    selector: 'app-perf-log-model-list-screen',
    templateUrl: './perf-log-model-list-screen.component.html',
    standalone: false
})
export class PerfLogModelListScreenComponent extends BaseListComponent<PerfLogModel, PerfLogSearchCriteria> implements OnInit {
  @Input() public perfLogModelAction: string | undefined;
  @Input() public globalSearchText: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  public startDate: Date | null = null;
  public endDate: Date | null = null;
  public override searchModel: PerfLogSearchCriteria = new PerfLogSearchCriteria();
  public isSelectedForCollapse = true;
  public selectedIndexForCollapse: number | undefined;
  public appPerfLogTimeTypes: Status[] = [];
  public selectedAppPerfLogTimeType: Status | undefined;
   
  public perfLogTimeType: typeof AppPerfLogTimeTypes = AppPerfLogTimeTypes;
  public appFunctionName: string | null = null;
  public override columns = [
    {
      text: 'Log Date Time',
      align: 'left',
      sortable: true,
      value: 'logDate',
      width: '115px',
    },
    {
      text: 'Machine Name / User Name',
      align: 'left',
      sortable: true,
      value: 'machineName',
      width: '115px',
    },
    {
      text: 'App Name',
      align: 'left',
      sortable: true,
      value: 'appName',
      width: '115px',
    },
    {
      text: 'Activity Id',
      align: 'left',
      sortable: true,
      value: 'activityId',
      width: '115px',
    },
    {
      text: 'App Module',
      align: 'left',
      sortable: true,
      value: 'appModule',
      width: '115px',
    },
    {
      text: 'App Function',
      align: 'left',
      sortable: true,
      value: 'appFunction',
      width: '115px',
    }, {
      text: 'Time',
      align: 'left',
      sortable: true,
      value: 'elapsedTimeMs',
      width: '115px',
    },
  ];

  public constructor(
    private dialogService: CommonDialogService,
    public sessionService: SessionService,
    private httpService: HttpService,
    private titleService: TitleService,
    private crudService: CrudService,
    private customSelectService: CustomSelectService,
  ) {
    super();
    this.titleService.pageTitle = 'Perf Log Viewers';
  }

  public ngOnInit(): void {
    this.appPerfLogTimeTypes = this.customSelectService.getAppPerfLogTimeTypes();
    this.getPerfLogModels(this.searchModel);
  }

  public getPerfLogModels(searchModel: PerfLogSearchCriteria): void {
    this.httpService.getPerfLogs(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          return this.dialogService.errorWithReturnModel("unable to get Perf Log Model ", result);
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
        return;
      },
      error: (err: unknown) => {
        this.dialogService.error('Unable to get Perf Log Models');
      },
      complete: () => null
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    let perfLogSearchCriteria = new PerfLogSearchCriteria();
    perfLogSearchCriteria = PerfLogSearchCriteria.getCopy(this.searchModel);
    perfLogSearchCriteria.startDate = this.startDate;
    perfLogSearchCriteria.endDate = this.endDate;
    if (this.appFunctionName) perfLogSearchCriteria.appFunctions = [this.appFunctionName];
    this.getPerfLogModels(perfLogSearchCriteria);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.appFunctions = null;
    this.searchModel.startDate = null;
    this.searchModel.endDate = null;
    this.getPerfLogModels(this.searchModel);
  }

  public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getPerfLogModels(this.searchModel);
  }

  public changePerfLogTimeType(): void {
    this.searchModel.greaterThanMS = null;
    this.searchModel.lesserThanMS = null;
    this.searchModel.equalToMs = null;
  }

  public changeCollapseIdentifier(isShow: boolean, index: number): void {
    this.isSelectedForCollapse = isShow;
    this.selectedIndexForCollapse = index;
  }

  public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.searchModel.startDate = this.startDate;
    this.searchModel.endDate = this.endDate;
    this.getPerfLogModels(this.searchModel);
  }

}
