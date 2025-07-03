import { AbstractControl, NgForm, UntypedFormArray, UntypedFormBuilder, UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { JobTemplateModel, JobTemplateSearchCriteria, JobWithDiffModel, LoggedInUserModel, ScheduledJobModel, ScheduledJobSearchCriteria } from 'src/common/models/app-model';

import { AuditDuoColumnModel } from 'src/common/models/app-custom-model';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from '../../services/common-dialog.service';
import { CrudService } from '../../services/crud.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';

@Component({
    selector: 'app-run-job-modal',
    templateUrl: './run-job-modal.component.html',
    styleUrl: './run-job-modal.component.scss',
    standalone: false
})
export class RunJobModalComponent implements OnInit {

  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public jobModel: JobWithDiffModel = new JobWithDiffModel();
  @Output() public modalResponse: EventEmitter<number[]> = new EventEmitter<number[]>();
  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;
  public aSubmittedDate: Date | null = null;

  public jobTemplates: JobTemplateModel[] = [];
  public selectedJobTemplate: JobTemplateModel | undefined;
  public scheduledJobs: ScheduledJobModel[] = [];
  public selectedScheduledJob: ScheduledJobModel | undefined;
  public jobTemplateSearchModel = new JobTemplateSearchCriteria();
  public jobTemplateCurrentSort = 'up';
  public jobTemplateTotalItems = 0;
  public scheduledJobSearchModel = new ScheduledJobSearchCriteria();
  public scheduledJobCurrentSort = 'up';
  public scheduledJobTotalItems = 0;
  public isRunScheduledJob = false;
  public form: UntypedFormGroup;
  public selectedCheckBox: boolean[] = [];
  public jobTemplateColumns = [
    {
      text: '',
      align: 'left',
      sortable: false,
      value: 'none',
      width: '115px',
    },
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
      text: 'Company Name',
      align: 'left',
      sortable: true,
      value: 'companyName',
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

  public scheduledJobColumns = [
    {
      text: '',
      align: 'left',
      sortable: false,
      value: 'none',
      width: '115px',
    },
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
      text: 'Company Name',
      align: 'left',
      sortable: true,
      value: 'companyName',
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
    private fb: UntypedFormBuilder,
    private crudService: CrudService,
    private activeModal: NgbActiveModal,) {
    this.form = this.fb.group({
      checkArray: this.fb.array([], [Validators.required])
    })
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.getJobTemplates(this.jobTemplateSearchModel);
    this.getScheduledJobs(this.scheduledJobSearchModel);
  }

  public getJobTemplates(searchModel: JobTemplateSearchCriteria): void {
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getJobTemplates(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.jobTemplates = data.model;
          this.jobTemplateTotalItems = data.totalRecords ?? 0;
        }
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Job Template');
      },
      complete: () => null,
    });
  }

  public getScheduledJobs(searchModel: ScheduledJobSearchCriteria): void {
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    this.httpService.getScheduledJobs(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) {
          this.scheduledJobs = data.model;
          this.scheduledJobTotalItems = data.totalRecords ?? 0;
        }
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Job Template');
      },
      complete: () => null,
    });
  }
  public cancel(checkDirty: boolean): void {
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    const { checkArray } = this.form.value;
    if (this.form.dirty && checkDirty && checkArray?.length > 0) {
      this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'Discard').then(res => {
        if (res)
          return this.activeModal.close();
      });
    } else {
      this.activeModal.close();
    }
  }

  public onJobTemplateSearch(f: NgForm): void {
    if (f.invalid) return;
    this.getJobTemplates(this.jobTemplateSearchModel);
  }

  public onJobTemplateCheckboxChange(e: Event): void {
    const checkArray: UntypedFormArray = this.form.get('checkArray') as UntypedFormArray;
    const currentValue = (e.currentTarget as HTMLInputElement).checked;
    const index = checkArray.controls.findIndex(list => list.value === currentValue);
    if (currentValue && index == -1) {
      checkArray.push(new UntypedFormControl(+currentValue));
    } else {
      let i = 0;
      checkArray.controls.forEach((item: AbstractControl) => {
        if (item.value == currentValue) {
          checkArray.removeAt(i);
          return;
        }
        i++;
      });
    }
  }

  public onScheduledJobCheckboxChange(e: Event): void {
    const checkArray: UntypedFormArray = this.form.get('checkArray') as UntypedFormArray;
    const currentValue = (e.currentTarget as HTMLInputElement).value;
    const index = checkArray.controls.findIndex(list => list.value === currentValue);
    if (currentValue && index == -1) {
      checkArray.push(new UntypedFormControl(+currentValue));
    } else {
      let i = 0;
      checkArray.controls.forEach((item: AbstractControl) => {
        if (item.value == currentValue) {
          checkArray.removeAt(i);
          return;
        }
        i++;
      });
    }
  }

  public changeRunJobOption(isRunScheduledJob: boolean): void {
    if (isRunScheduledJob) {
      this.getScheduledJobs(this.scheduledJobSearchModel);
    } else {
      this.getJobTemplates(this.jobTemplateSearchModel);
    }
  }

  public onScheduledJobSearch(f: NgForm): void {
    if (f.invalid) return;
    this.getScheduledJobs(this.scheduledJobSearchModel);
  }

  public onSubmit(f: NgForm): void {
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    const { checkArray } = this.form.value;
    if (checkArray?.length == 0) {
      this.dialogService.error('Please select atleast one Job template.');
    } else {
      this.modalResponse.emit(checkArray as number[]);
      this.activeModal.close();
    }
  }

  public onJobTemplateReset(f: NgForm): void {
    f.resetForm();
    this.getJobTemplates(this.jobTemplateSearchModel);
  }

  public onScheduledJobReset(f: NgForm): void {
    f.resetForm();
    this.getScheduledJobs(this.scheduledJobSearchModel);
  }

  public jobTemplatechangeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.jobTemplateSearchModel, { currentSort: this.jobTemplateCurrentSort });
    this.jobTemplateCurrentSort = this.jobTemplateSearchModel.sortAscending ? 'up' : 'down';
    this.getJobTemplates(this.jobTemplateSearchModel);
  }

  public jobTemplatePaginationChange(e: BaseSearchCriteria): void {
    this.jobTemplateSearchModel.page = e.page;
    this.jobTemplateSearchModel.pageSize = e.pageSize;
    this.getJobTemplates(this.jobTemplateSearchModel);
  }

  public scheduledJobchangeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.scheduledJobSearchModel, { currentSort: this.scheduledJobCurrentSort });
    this.scheduledJobCurrentSort = this.scheduledJobSearchModel.sortAscending ? 'up' : 'down';
    this.getScheduledJobs(this.scheduledJobSearchModel);
  }

  public scheduledJobPaginationChange(e: BaseSearchCriteria): void {
    this.scheduledJobSearchModel.page = e.page;
    this.scheduledJobSearchModel.pageSize = e.pageSize;
    this.getScheduledJobs(this.scheduledJobSearchModel);
  }
}
