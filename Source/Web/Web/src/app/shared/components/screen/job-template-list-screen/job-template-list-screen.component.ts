import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import { JobTemplateCategoryModel, JobTemplateCategorySearchCriteria, JobTemplateModel, JobTemplateSearchCriteria } from 'src/common/models/app-model';
import { Component, Input, OnInit } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
  selector: 'app-job-template-list-screen',
  templateUrl: './job-template-list-screen.component.html',
  standalone: false
})
export class JobTemplateListScreenComponent extends BaseListComponent<JobTemplateModel, JobTemplateSearchCriteria> implements OnInit {

  @Input() public jobTemplateAction: string | undefined;
  @Input() public globalSearchText: string | undefined;
  public override searchModel: JobTemplateSearchCriteria = new JobTemplateSearchCriteria();

  public predefinedJobTypes: Status[] = [];
  public selectedPredefinedJobTypes: Status[] = [];
  public predefinedJobOrReportTypes: Status[] = [];
  public selectedPredefinedJobOrReportTypes: Status[] = [];
  public jobTemplateCategoryTypeahead$ = new Subject<string>();
  public jobTemplateCategoryTypeaheadLoading = false;
  public jobTemplateCategorys: JobTemplateCategoryModel[] = [];
  public selectedJobTemplateCategory: JobTemplateCategoryModel | undefined;
  public initialUrl: string | undefined;

  public override columns = [
    {
      text: 'Job Type',
      align: 'left',
      sortable: true,
      value: 'predefinedJobTypeName',
      width: '115px',
    },
    {
      text: 'Description',
      align: 'left',
      sortable: true,
      value: 'description',
      width: '115px',
    },
    {
      text: 'Template Name',
      align: 'left',
      sortable: true,
      value: 'templateName',
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
    private router: Router,
    private titleService: TitleService,
    private selectService: SelectService,
    private crudService: CrudService

  ) {
    super();
    this.titleService.pageTitle = "Job Templates";
  }

  public ngOnInit(): void {
    this.initialUrl = this.sessionService.getInitialUrl();
    this.getJobTemplates(this.searchModel);
    this.predefinedJobTypes = this.selectService.GetPredefinedJobTypes();
    this.predefinedJobOrReportTypes = this.selectService.GetPredefinedJobOrReportTypes();
    this.getJobTemplateCategorys();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    this.jobTemplateCategoryTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.jobTemplateCategoryTypeaheadLoading = true;
      const jobTemplateCategorySearchCriteria = new JobTemplateCategorySearchCriteria();
      jobTemplateCategorySearchCriteria.keyword = searchValue;
      return this.httpService.getJobTemplateCategorys(jobTemplateCategorySearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
        this.jobTemplateCategoryTypeaheadLoading = false;
        if (data.model) this.jobTemplateCategorys = data.model;
      },
      error: (err: unknown) => {
        this.jobTemplateCategoryTypeaheadLoading = false;
      },
      complete: () => null,
    });
  }

  public getJobTemplateCategorys(): void {
    const searchModel = new JobTemplateCategorySearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getJobTemplateCategorys(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.jobTemplateCategorys = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Job Template Category');
      },
      complete: () => null,
    });
  }

  public getJobTemplates(searchModel: JobTemplateSearchCriteria): void {
    this.httpService.getJobTemplates(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Job Templates", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Job Templates"),
      complete: () => null,
    });
  }

  public showAddJobTemplateModal(): void {
    this.router.navigateByUrl(`${this.initialUrl}/job-templates/job-template-item/create`);
  }

  public showViewJobTemplateModal(item: JobTemplateModel): void {
    this.router.navigateByUrl(`${this.initialUrl}/job-templates/job-template-item/view/${item.jobTemplateId}`);
  }

  public showUpdateJobTemplateModal(item: JobTemplateModel): void {
    this.router.navigateByUrl(`${this.initialUrl}/job-templates/job-template-item/edit/${item.jobTemplateId}`);
  }

  public deleteJobTemplate(item: JobTemplateModel): void {
    this.dialogService.confirmWithTitle(`Delete Job Template`, `Do you want to delete the jobTemplate - ${item.templateName}?`).then(res => {
      if (!res) return;
      this.httpService.markJobTemplateAsDeleted(item.jobTemplateId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Job Template");
        this.getJobTemplates(this.searchModel);
        this.dialogService.success("Successfully deleted the Job Template");
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
    this.searchModel.jobTemplateCategoryId = this.selectedJobTemplateCategory ? this.selectedJobTemplateCategory.jobTemplateCategoryId : null;

    this.getJobTemplates(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.predefinedJobTypeIds = null;
    this.searchModel.predefinedJobOrReportTypeIds = null;
    this.searchModel.jobTemplateCategoryId = null;
    this.getJobTemplates(this.searchModel);
  }

  public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getJobTemplates(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    const jobTemplateSearchCriteria = new JobTemplateSearchCriteria();
    jobTemplateSearchCriteria.page = e.page;
    jobTemplateSearchCriteria.pageSize = e.pageSize;
    this.getJobTemplates(jobTemplateSearchCriteria);
  }

}
