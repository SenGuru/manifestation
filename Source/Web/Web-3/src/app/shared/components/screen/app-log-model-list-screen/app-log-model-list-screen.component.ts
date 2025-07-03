
import { AppLogExtendedModel, AuditDuoColumnModel, LogSeverityType, Status } from 'src/common/models/app-custom-model';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppLogSearchCriteria } from 'src/common/models/app-model';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from 'src/app/shared/services/crud.service';
import { CustomSelectService } from 'src/common/services/custom-select-service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { SessionService } from 'src/common/services/session/session.service';

@Component({
    selector: 'app-app-log-model-list-screen',
    templateUrl: './app-log-model-list-screen.component.html',
    standalone: false
})
export class AppLogModelListScreenComponent extends BaseListComponent<AppLogExtendedModel, AppLogSearchCriteria> implements OnInit {

  @ViewChild('appLogModelSearch') public appLogModelSearchForm!: NgForm;
  @Input() public title: string | undefined;
  @Input() public isCreateMode: boolean = false;
  @Input() public isViewMode: boolean = false;
  @Input() public isEditMode: boolean = false;
  @Input() public showLogCacheScreen = false;
  public severityTypes: Status[] = [];
  public selectedSeverityType: Status[] = [];
  public readonly WORD_WRAP = 'log-viewer'
  public selectedCheckBox: boolean[] = [];
  public startDate: Date | null = null;
  public endDate: Date | null = null;
  public override searchModel: AppLogSearchCriteria = new AppLogSearchCriteria();
  public isSelectedForCollapse = true;
  public selectedIndexForCollapse: number | undefined;
  public isWordWrap = false;
  public initColumns: AuditDuoColumnModel[] = [];
  public listCheckArray: AppLogExtendedModel[] = [];
  public appLogDataList: AppLogExtendedModel[] = [];
  public appActivityId: string | null = null;
  public logSeverityType: LogSeverityType | undefined;
  public override columns: AuditDuoColumnModel[] = [
    {
      text: '',
      align: 'left',
      sortable: false,
      value: 'none',
      width: '3',
    },
    {
      text: 'Log Date',
      align: 'left',
      sortable: true,
      value: 'logDate',
      width: '3',
    },
    {
      text: 'Severity',
      align: 'left',
      sortable: true,
      value: 'severity',
      width: '4',
    },
    {
      text: 'Machine / User Name',
      align: 'left',
      sortable: true,
      value: 'machineName',
      width: '6',
    },
    {
      text: 'App Name',
      align: 'left',
      sortable: true,
      value: 'appName',
      width: '7',
    },
    {
      text: 'File Name / Line # / FNC',
      align: 'left',
      sortable: true,
      value: 'currentSourceFileName',
      width: '7',
    },
    {
      text: 'Activity ID / Message',
      align: 'left',
      sortable: true,
      value: 'message',
      width: '15',
    },
  ];

  public constructor(
    private dialogService: CommonDialogService,
    public sessionService: SessionService,
    private httpService: HttpService,
    public enumViewService: EnumViewService,
    private crudService: CrudService,
    private customSelectService: CustomSelectService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
  ) {
    super();
  }

  public ngOnInit(): void {
    if (this.showLogCacheScreen) this.columns = this.columns.filter(item => item.value !== 'none');
    else this.getAppLogModels(this.searchModel);
    this.initColumns = [...this.columns];
    this.severityTypes = this.customSelectService.getLogSeverityTypes();
    this.setWordWrapField();
  }

  public setWordWrapField(): void {
    const wordWrapValue = localStorage.getItem(this.WORD_WRAP);
    this.isWordWrap = wordWrapValue === 'true';
    if (this.isWordWrap) {
      this.columns = this.initColumns.filter(column => column.value !== 'message');
    }
  }

  public AddToLogCache(): void {
    for (let index = 0; index < this.listCheckArray.length; index++) {
      (this.listCheckArray[index] as AppLogExtendedModel).isLogCache = true;
    }
    this.appLogDataList = this.listCheckArray;
  }

  public showLogCache(): void {
    const modalRef = this.modalService.open(AppLogModelListScreenComponent, { windowClass: 'my-class-lg', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as AppLogModelListScreenComponent;
    componentInstance.showLogCacheScreen = true;
    componentInstance.title = 'View App Log';
    componentInstance.listData = this.appLogDataList;
  }

  public close(checkDirty: boolean): void {
    if (this.appLogModelSearchForm?.dirty && checkDirty) {
      this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'Discard').then(res => {
        if (res)
          return this.activeModal.close();
      });
    }
    else {
      this.activeModal.close();
    }
  }

  public getAppLogModels(searchModel: AppLogSearchCriteria): void {
    this.httpService.getAppLogs(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          return this.dialogService.errorWithReturnModel("unable to get App Log Model ", result);
        }
        if (result.model) {
          this.listData = AppLogExtendedModel.getCopyArray(result.model);
          this.totalItems = result.totalRecords ?? 0;
        }
        return;
      },
      error: (err: unknown) => {
        this.dialogService.error('Unable to get App Logs');
      },
      complete: () => null
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    let appLogSearchCriteria = new AppLogSearchCriteria();
    appLogSearchCriteria = AppLogSearchCriteria.getCopy(this.searchModel);
    appLogSearchCriteria.startDate = this.startDate;
    appLogSearchCriteria.endDate = this.endDate;
    if (this.selectedSeverityType?.length > 0) {
      appLogSearchCriteria.severityLevels = this.selectedSeverityType.map(item => item.type ?? '');
    }
    if (this.appActivityId) appLogSearchCriteria.activityIds = [this.appActivityId];
    this.getAppLogModels(appLogSearchCriteria);
    this.resetSelectedLogCache();
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.selectedSeverityType = [];
    this.searchModel.activityIds = null;
    this.getAppLogModels(this.searchModel);
    this.resetSelectedLogCache();
  }

  public changeSort(header: AuditDuoColumnModel): void {
    if (this.showLogCacheScreen) return;
    this.resetSelectedLogCache();
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getAppLogModels(this.searchModel);
  }

  public changeCollapseIdentifier(isShow: boolean, index: number): void {
    this.isSelectedForCollapse = isShow;
    this.selectedIndexForCollapse = index;
  }

  public resetSelectedLogCache(): void {
    this.selectedCheckBox = [];
    this.listCheckArray = [];
    this.appLogDataList = [];
  }

  public paginationChange(e: BaseSearchCriteria): void {
    this.resetSelectedLogCache();
    const appLogSearchCriteria = new AppLogSearchCriteria();
    appLogSearchCriteria.page = e.page;
    appLogSearchCriteria.pageSize = e.pageSize;
    this.getAppLogModels(appLogSearchCriteria);
  }

  public completeActionSuccess(message: string): void {
    this.dialogService.success(message);
  }

  public completeActionFailure(message: string): void {
    this.dialogService.error(message);
  }

  public changeWordWrapMessage(): void {
    if (this.isWordWrap) {
      this.columns = this.initColumns.filter(column => column.value !== 'message');
      localStorage.setItem(this.WORD_WRAP, 'true');
    }
    else {
      this.columns = this.initColumns;
      localStorage.setItem(this.WORD_WRAP, 'false');
    }
  }

  public onCheckboxChange(item: AppLogExtendedModel, selectedIndex: number): void {
    const previousValue = this.selectedCheckBox[selectedIndex];
    this.selectedCheckBox[selectedIndex] = !this.selectedCheckBox[selectedIndex];
    if (previousValue) {
      const idx1 = this.listCheckArray.findIndex((i) => i === item);
      if (idx1 !== -1) {
        this.listCheckArray.splice(idx1, 1);
      }
      else {
        this.listCheckArray.push(item);
      }
    } else {
      this.listCheckArray.push(item);
    }

  }

}
