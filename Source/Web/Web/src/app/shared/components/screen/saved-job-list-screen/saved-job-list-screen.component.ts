import { AuditDuoColumnModel, Status } from 'src/common/models/app-custom-model';
import { JobTemplateModel, JobTemplateSearchCriteria, SavedJobModel, SavedJobSearchCriteria, UserModel, UserSearchCriteria } from 'src/common/models/app-model';
import { Component, Input, OnInit } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SavedJobCrudModalComponent } from '../../../modals/saved-job-crud-modal/saved-job-crud-modal.component';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/services/session/session.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
    selector: 'app-saved-job-list-screen',
    templateUrl: './saved-job-list-screen.component.html',
    standalone: false
})
export class SavedJobListScreenComponent extends BaseListComponent<SavedJobModel, SavedJobSearchCriteria> implements OnInit {

  @Input() public savedJobAction: string | undefined;
  @Input() public globalSearchText: string | undefined;
  public override searchModel: SavedJobSearchCriteria = new SavedJobSearchCriteria();

 
  public predefinedJobTypes: Status[] = [];
  public selectedPredefinedJobTypes: Status[] = [];
  public predefinedJobOrReportTypes: Status[] = [];
  public selectedPredefinedJobOrReportTypes: Status[] = [];
  public jobTemplateTypeahead$ = new Subject<string>();
  public jobTemplateTypeaheadLoading = false;
  public jobTemplates: JobTemplateModel[] = [];
  public selectedJobTemplate: JobTemplateModel | undefined;
  public submittedByUserTypeahead$ = new Subject<string>();
  public submittedByUserTypeaheadLoading = false;
  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading = false;
  public users: UserModel[] = [];
  public selectedUser: UserModel | undefined;

  public override columns = [
    {
      text: 'Job Type',
      align: 'left',
      sortable: true,
      value: 'predefinedJobTypeName',
      width: '115px',
    },
    {
      text: 'Job Or Report Type',
      align: 'left',
      sortable: true,
      value: 'predefinedJobOrReportTypeName',
      width: '115px',
    },
    {
      text: 'Job Template Name',
      align: 'left',
      sortable: true,
      value: 'jobTemplateName',
      width: '115px',
    },
    {
      text: 'Completed Date',
      align: 'left',
      sortable: true,
      value: 'completedDate',
      width: '115px',
    },
    {
      text: 'Submitted Date',
      align: 'left',
      sortable: true,
      value: 'submittedDate',
      width: '115px',
    },
    {
      text: 'Submitted By Name',
      align: 'left',
      sortable: true,
      value: 'submittedByUserName',
      width: '115px',
    },
    {
      text: 'Executable Folder',
      align: 'left',
      sortable: true,
      value: 'executableFolder',
      width: '115px',
    },
    {
      text: 'Executable Name',
      align: 'left',
      sortable: true,
      value: 'executableName',
      width: '115px',
    },
    {
      text: 'Executable Parameter',
      align: 'left',
      sortable: true,
      value: 'executableParameter',
      width: '115px',
    },
    {
      text: 'Serialized Job Parameter',
      align: 'left',
      sortable: true,
      value: 'serializedJobParameter',
      width: '115px',
    },
    {
      text: 'Report Template File',
      align: 'left',
      sortable: true,
      value: 'reportTemplateFile',
      width: '115px',
    },
    {
      text: 'Export Doc',
      align: 'left',
      sortable: true,
      value: 'exportDoc',
      width: '115px',
    },
    {
      text: 'Export Xls',
      align: 'left',
      sortable: true,
      value: 'exportXls',
      width: '115px',
    },
    {
      text: 'Export Ppt',
      align: 'left',
      sortable: true,
      value: 'exportPpt',
      width: '115px',
    },
    {
      text: 'Export Csv',
      align: 'left',
      sortable: true,
      value: 'exportCsv',
      width: '115px',
    },
    {
      text: 'Export Xml',
      align: 'left',
      sortable: true,
      value: 'exportXml',
      width: '115px',
    },
    {
      text: 'Export Pdf',
      align: 'left',
      sortable: true,
      value: 'exportPdf',
      width: '115px',
    },
    {
      text: 'Export Custom',
      align: 'left',
      sortable: true,
      value: 'exportCustom',
      width: '115px',
    },
    {
      text: 'Custom Export Name',
      align: 'left',
      sortable: true,
      value: 'customExportName',
      width: '115px',
    },
    {
      text: 'Pickup Date',
      align: 'left',
      sortable: true,
      value: 'pickupDate',
      width: '115px',
    },
    {
      text: 'Job Name',
      align: 'left',
      sortable: true,
      value: 'jobName',
      width: '115px',
    },
    {
      text: 'User Name',
      align: 'left',
      sortable: true,
      value: 'userName',
      width: '115px',
    },
    {
      text: 'Include Data Source File',
      align: 'left',
      sortable: true,
      value: 'includeDataSourceFile',
      width: '115px',
    },
    {
      text: 'Is Deleted',
      align: 'left',
      sortable: true,
      value: 'isDeleted',
      width: '115px',
    },
    {
      text: 'Created Date',
      align: 'left',
      sortable: true,
      value: 'createdDate',
      width: '115px',
    },
    {
      text: 'Created By',
      align: 'left',
      sortable: true,
      value: 'createdByUserFirstName',
      width: '115px',
    },
    {
      text: 'Modified Date',
      align: 'left',
      sortable: true,
      value: 'modifiedDate',
      width: '115px',
    },
    {
      text: 'Modified By',
      align: 'left',
      sortable: true,
      value: 'modifiedByUserFirstName',
      width: '115px',
    },
  ];

  public constructor(
    private dialogService: CommonDialogService,
    public sessionService: SessionService,
    private httpService: HttpService,
    private modalService: NgbModal,
    private selectService: SelectService,
    private crudService: CrudService

  ) {
    super();
  }

  public ngOnInit(): void {
    this.getSavedJobs(this.searchModel);
   
    this.predefinedJobTypes = this.selectService.GetPredefinedJobTypes();
    this.predefinedJobOrReportTypes = this.selectService.GetPredefinedJobOrReportTypes();
    this.getJobTemplates();
    this.getUsers();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    
    this.jobTemplateTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.jobTemplateTypeaheadLoading = true;
      const jobTemplateSearchCriteria = new JobTemplateSearchCriteria();
      jobTemplateSearchCriteria.keyword = searchValue;
      return this.httpService.getJobTemplates(jobTemplateSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.jobTemplateTypeaheadLoading = false;
        if (data.model) this.jobTemplates = data.model;
      },
      error: (err: unknown) => {
        this.jobTemplateTypeaheadLoading = false;
      },
      complete: () => null,
    });
    this.userTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.userTypeaheadLoading = true;
      const userSearchCriteria = new UserSearchCriteria();
      userSearchCriteria.keyword = searchValue;
      return this.httpService.getUsers(userSearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) {
          this.userTypeaheadLoading = false;
          this.users = data.model;
        }
      },
      error: (err: unknown) => {
        this.userTypeaheadLoading = false;
      },
      complete: () => null,
    });
  }

 
  public getJobTemplates(): void {
    const searchModel = new JobTemplateSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getJobTemplates(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.jobTemplates = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Job Template');
      },
      complete: () => null,
    });
  }

  public getUsers(): void {
    const searchModel = new UserSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getUsers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.users = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get User');
      },
      complete: () => null,
    });
  }

  public getSavedJobs(searchModel: SavedJobSearchCriteria): void {
    this.httpService.getSavedJobs(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Saved Jobs", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Saved Jobs"),
      complete: () => null,
    });
  }

  public showAddSavedJobModal(): void {
    const modalRef = this.modalService.open(SavedJobCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as SavedJobCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Saved Job';
    componentInstance.onCloseCallback = (): void => {
      this.getSavedJobs(this.searchModel);
    }
  }

  public showViewSavedJobModal(item: SavedJobModel): void {
    const modalRef = this.modalService.open(SavedJobCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as SavedJobCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Saved Job';
    componentInstance.savedJobModel = JSON.parse(JSON.stringify(item)) as SavedJobModel;
  }

  public showUpdateSavedJobModal(item: SavedJobModel): void {
    const modalRef = this.modalService.open(SavedJobCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as SavedJobCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Saved Job';
    componentInstance.savedJobModel = JSON.parse(JSON.stringify(item)) as SavedJobModel;
    componentInstance.onCloseCallback = (): void => {
      this.getSavedJobs(this.searchModel);
    }
  }

  public deleteSavedJob(item: SavedJobModel): void {
    this.dialogService.confirmWithTitle(`Delete Saved Job`, `Do you want to delete the saved Job - ${item.jobName}?`).then(res => {
      if (!res) return;
      this.httpService.markSavedJobAsDeleted(item.savedJobId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Saved Job");
        this.getSavedJobs(this.searchModel);
        this.dialogService.success("Successfully deleted the Saved Job");
        return;
      });
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    
    const predefinedJobTypesArray = this.selectedPredefinedJobTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedJobTypeIds = predefinedJobTypesArray;
    const predefinedJobOrReportTypesArray = this.selectedPredefinedJobOrReportTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedJobOrReportTypeIds = predefinedJobOrReportTypesArray;
    this.searchModel.jobTemplateId = this.selectedJobTemplate ? this.selectedJobTemplate.jobTemplateId : null;
    this.searchModel.userId = this.selectedUser ? this.selectedUser.userId : null;
    this.getSavedJobs(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    
    this.searchModel.predefinedJobTypeIds = null;
    this.searchModel.predefinedJobOrReportTypeIds = null;
    this.searchModel.jobTemplateId = null;
    this.searchModel.submittedByUserId = null;
    this.searchModel.userId = null;
    this.getSavedJobs(this.searchModel);
  }

  public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getSavedJobs(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    const savedJobSearchCriteria = new SavedJobSearchCriteria();
    savedJobSearchCriteria.page = e.page;
    savedJobSearchCriteria.pageSize = e.pageSize;
    this.getSavedJobs(savedJobSearchCriteria);
  }

}
