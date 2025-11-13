import { ActivatedRoute, Router } from '@angular/router';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup, NgForm } from '@angular/forms';
import { JobTemplateCategoryModel, JobTemplateCategorySearchCriteria, JobTemplateCreateModel, JobTemplateModel, JobTemplateParameterCreateModel, JobTemplateParameterUpdateModel, JobTemplateUpdateModel, JobTemplateWithDiffModel, LoggedInUserModel } from 'src/common/models/app-model';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ScreenActionTypes, Status } from 'src/common/models/app-custom-model';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { getHeaderWithBlockUI, getHeaderWithLoadingText } from 'src/common/interceptors/httpconfig.interceptor';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { JobTemplateCategoryCrudModalComponent } from '../job-template-category-crud-modal/job-template-category-crud-modal.component';
import { JobTemplateParameterListScreenComponent } from '../../components/screen/job-template-parameter-list-screen/job-template-parameter-list-screen.component';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { RouterDataService } from 'src/common/services/router/router-data.service';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { TitleService } from '../../services/title.service';

@UntilDestroy()
@Component({
    selector: 'app-job-template-crud-modal',
    templateUrl: './job-template-crud-modal.component.html',
    standalone: false
})
export class JobTemplateCrudModalComponent implements OnInit {

  @Input() public jobTemplateModel: JobTemplateWithDiffModel = new JobTemplateWithDiffModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isDeletable = false;
  @ViewChild(JobTemplateParameterListScreenComponent) public jobTemplateParameterListScreenComponent!: JobTemplateParameterListScreenComponent;
  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;
  public isShowFullScreen = false;
  public description__size: number = JobTemplateModel.description__size;
  public executableFolder__size: number = JobTemplateModel.executableFolder__size;
  public executableName__size: number = JobTemplateModel.executableName__size;
  public executableParameter__size: number = JobTemplateModel.executableParameter__size;
  public customExportName__size: number = JobTemplateModel.customExportName__size;
  public templateName__size: number = JobTemplateModel.templateName__size;
  public taskIdentifierName__size: number = JobTemplateModel.taskIdentifierName__size;
  public reportTemplateFile__size: number = JobTemplateModel.reportTemplateFile__size;
  public defaultOutputFileName__size: number = JobTemplateModel.defaultOutputFileName__size;

  public predefinedJobTypes: Status[] = [];
  public selectedPredefinedJobType: Status | undefined;
  public predefinedJobOrReportTypes: Status[] = [];
  public selectedPredefinedJobOrReportType: Status | undefined;
  public jobTemplateCategoryTypeahead$ = new Subject<string>();
  public jobTemplateCategoryTypeaheadLoading = false;
  public jobTemplateCategorys: JobTemplateCategoryModel[] = [];
  public selectedJobTemplateCategory: JobTemplateCategoryModel | null | undefined;
 
  public initialUrl: string | undefined;
  public jobTemplateCategorySearchCriteria: JobTemplateCategorySearchCriteria = new JobTemplateCategorySearchCriteria();

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private routerDataService: RouterDataService,
    private router: Router,
    private modalService: NgbModal,
    private titleService: TitleService,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.initialUrl = this.sessionService.getInitialUrl();
    const routerParam = this.routerDataService.getRouterParamData(this.router.routerState.snapshot.root);
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedJobTypes = this.selectService.GetPredefinedJobTypes();
    this.predefinedJobOrReportTypes = this.selectService.GetPredefinedJobOrReportTypes();
    if (!this.isCreateMode) {
      this.selectedPredefinedJobType = this.predefinedJobTypes.find(item => item.id === this.jobTemplateModel.predefinedJobTypeId);
      this.selectedPredefinedJobOrReportType = this.predefinedJobOrReportTypes.find(item => item.id === this.jobTemplateModel.predefinedJobOrReportTypeId);
    }
    if (!ObjectUtils.isObjNullOrUndefined(routerParam)) {
      // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
      const actionType: ScreenActionTypes = routerParam['actionType'];
      if (!ObjectUtils.isNullOrUndefined(actionType)) {
        if (actionType == 'create') {
          this.isCreateMode = true;
          this.isShowFullScreen = true;
          this.titleService.pageTitle = "Create Job Template";
          this.buttonText = "Create";
        } else if (actionType == 'edit') {
          this.isEditMode = true;
          this.titleService.pageTitle = "Edit Job Template";
          this.getJobTemplate(+routerParam['jobTemplateId']);
          this.isShowFullScreen = true;
          this.buttonText = "Update";
        } else if (actionType == 'view') {
          this.isViewMode = true;
          this.titleService.pageTitle = "View Job Template";
          this.getJobTemplate(+routerParam['jobTemplateId']);
          this.isShowFullScreen = true;
        }
      }
    }

    this.getJobTemplateCategorys(this.jobTemplateCategorySearchCriteria);
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    this.jobTemplateCategoryTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.jobTemplateCategoryTypeaheadLoading = true;
      const jobTemplateCategorySearchCriteria = new JobTemplateCategorySearchCriteria();
      jobTemplateCategorySearchCriteria.keyword = searchValue;
      if (!this.isCreateMode) jobTemplateCategorySearchCriteria.selectedId = this.jobTemplateModel.jobTemplateCategoryId;
      return this.httpService.getJobTemplateCategorys(jobTemplateCategorySearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        if (data.model) {
          this.jobTemplateCategoryTypeaheadLoading = false;
          this.jobTemplateCategorys = data.model;
        }
      },
      error: (err: unknown) => {
        this.jobTemplateCategoryTypeaheadLoading = false;
      },
      complete: () => null,
    }); 
  }

  public showAddJobTemplateCategoryModal(): void {
    const modalRef = this.modalService.open(JobTemplateCategoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as JobTemplateCategoryCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Job Template Category';
    componentInstance.onCloseCallback = (res: JobTemplateCategoryModel | null): void => {
      if (ObjectUtils.isObjNullOrUndefined(res)) return;
      this.getJobTemplateCategorys(this.jobTemplateCategorySearchCriteria, res?.jobTemplateCategoryId);
    }
  }

  public showEditJobTemplateCategoryModal(item: JobTemplateCategoryModel): void {
    const modalRef = this.modalService.open(JobTemplateCategoryCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as JobTemplateCategoryCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Job Template Category';
    componentInstance.jobTemplateCategoryModel = JSON.parse(JSON.stringify(item)) as JobTemplateCategoryModel;
    componentInstance.onCloseCallback = (res: JobTemplateCategoryModel | null): void => {
      if (ObjectUtils.isObjNullOrUndefined(res)) return;
      this.getJobTemplateCategorys(this.jobTemplateCategorySearchCriteria, res?.jobTemplateCategoryId);
    }
  }

  public deleteJobTemplateCategory(item: JobTemplateCategoryModel): void {
    this.dialogService.confirmWithTitle(`Delete Job Template Category`, `Do you want to delete the jobTemplateCategory - ${item.name}?`).then(res => {
      if (!res) return;
      this.httpService.markJobTemplateCategoryAsDeleted(item.jobTemplateCategoryId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Job Template Category");
        this.getJobTemplateCategorys(this.jobTemplateCategorySearchCriteria);
        this.selectedJobTemplateCategory = null;
        this.dialogService.success("Successfully deleted the Job Template Category");
        return;
      });
    });
  }
 
  public getJobTemplate(jobTemplateId: number): void {
    this.httpService.getJobTemplate(jobTemplateId).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Job Template", result);
          return;
        }
        if (result.model) {
          this.jobTemplateModel = result.model as JobTemplateWithDiffModel;
          this.selectedPredefinedJobType = this.predefinedJobTypes.find(item => item.id === this.jobTemplateModel.predefinedJobTypeId);
          this.selectedPredefinedJobOrReportType = this.predefinedJobOrReportTypes.find(item => item.id === this.jobTemplateModel.predefinedJobOrReportTypeId);
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Job Template"),
      complete: () => null,
    });
  }

  public getJobTemplateCategorys(searchModel: JobTemplateCategorySearchCriteria, selectedJobTemplateCategoryId?: number): void {
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    if (!this.isCreateMode) searchModel.selectedId = this.jobTemplateModel.jobTemplateCategoryId;
    this.httpService.getJobTemplateCategorys(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.jobTemplateCategorys = data.model;
          if (!this.isCreateMode) this.selectedJobTemplateCategory = data.model.find(item => item.jobTemplateCategoryId === this.jobTemplateModel.jobTemplateCategoryId);
          if (selectedJobTemplateCategoryId) this.selectedJobTemplateCategory = data.model.find(item => item.jobTemplateCategoryId === selectedJobTemplateCategoryId);
        }
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Job Template Category');
      },
      complete: () => null,
    });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const jobTemplateModel = this.generateModel(f.value);
    if (jobTemplateModel) {
      if (this.isCreateMode) {
        this.createJobTemplate(jobTemplateModel, f);
      } else {
        this.updateJobTemplate(jobTemplateModel, f);
      }
    }
  }

  public generateModel(obj: JobTemplateModel): JobTemplateWithDiffModel | undefined {
    if (this.selectedPredefinedJobType == null)
      return;
    if (this.selectedPredefinedJobOrReportType == null)
      return;
    if (this.selectedJobTemplateCategory == null)
      return;
    Object.assign(this.jobTemplateModel, obj);
    Object.keys(this.jobTemplateModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.jobTemplateModel[key] && typeof this.jobTemplateModel[key] === 'object' && !(this.jobTemplateModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.jobTemplateModel[key] = null;
      }
    });

    this.jobTemplateModel.predefinedJobTypeId = this.selectedPredefinedJobType?.id ?? 0;
    this.jobTemplateModel.predefinedJobOrReportTypeId = this.selectedPredefinedJobOrReportType ? this.selectedPredefinedJobOrReportType.id : null;
    this.jobTemplateModel.jobTemplateCategoryId = this.selectedJobTemplateCategory?.jobTemplateCategoryId ?? 0;
    if (this.jobTemplateParameterListScreenComponent && this.jobTemplateParameterListScreenComponent.listData) {
      this.jobTemplateModel.existingParameters = [];
      this.jobTemplateModel.newParameters = [];
      for (const item of this.jobTemplateParameterListScreenComponent.listData) {
        if (!ObjectUtils.isNullOrUndefined(item.jobTemplateParameterId) && item.jobTemplateParameterId !== 0) {
          const jobTemplateParameterUpdateModel = new JobTemplateParameterUpdateModel();
          Object.assign(jobTemplateParameterUpdateModel, item);
          if (!this.isCreateMode) jobTemplateParameterUpdateModel.jobTemplateId = this.jobTemplateModel.jobTemplateId;
          this.jobTemplateModel.existingParameters.push(jobTemplateParameterUpdateModel);
        } else {
          const jobTemplateParameterCreateModel = new JobTemplateParameterCreateModel();
          Object.assign(jobTemplateParameterCreateModel, item);
          if (!this.isCreateMode) jobTemplateParameterCreateModel.jobTemplateId = this.jobTemplateModel.jobTemplateId;
          this.jobTemplateModel.newParameters.push(jobTemplateParameterCreateModel);
        }
      }
    }
    return this.jobTemplateModel;
  }

  public createJobTemplate(jobTemplateModel: JobTemplateCreateModel, f: NgForm): void {
    this.httpService.createJobTemplate(jobTemplateModel, getHeaderWithLoadingText("Saving ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to Create JobTemplate', result);
          return;
        }
        this.dialogService.success('Successfully Created the JobTemplate.');
        this.cancel();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to create JobTemplate'),
      complete: () => null,
    });
  }

  public updateJobTemplate(jobTemplateModel: JobTemplateUpdateModel, f: NgForm): void {
    this.httpService.updateJobTemplate(jobTemplateModel, getHeaderWithLoadingText("Updating ...")).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel('Not Able to update JobTemplate', result);
          return;
        }
        this.dialogService.success('Successfully updated the JobTemplate.');
        this.cancel();
      },
      error: (err: unknown) => this.dialogService.error('Not Able to update JobTemplate'),
      complete: () => null,
    });
  }

  public cancel(): void {
    this.router.navigateByUrl(`${this.initialUrl}/job-templates`);
  }
}
