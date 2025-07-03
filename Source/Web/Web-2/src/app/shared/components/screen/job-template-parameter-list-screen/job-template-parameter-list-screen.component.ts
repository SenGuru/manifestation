import { Component, Input, OnInit } from '@angular/core';
import { JobTemplateParameterModel, JobTemplateParameterSearchCriteria } from 'src/common/models/app-model';

import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { JobTemplateParameterCrudModalComponent } from '../../../modals/job-template-parameter-crud-modal/job-template-parameter-crud-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SessionService } from 'src/common/services/session/session.service';

@Component({
    selector: 'app-job-template-parameter-list-screen',
    templateUrl: './job-template-parameter-list-screen.component.html',
    standalone: false
})
export class JobTemplateParameterListScreenComponent extends BaseListComponent<JobTemplateParameterModel, JobTemplateParameterSearchCriteria> implements OnInit {

  @Input() public jobTemplateParameterAction: string | undefined;
  @Input() public isJobTemplateScreen = true;
  @Input() public jobTemplateId: number = 0;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  public override searchModel: JobTemplateParameterSearchCriteria = new JobTemplateParameterSearchCriteria();
  public override columns = [
    {
      text: 'Name',
      align: 'left',
      sortable: true,
      value: 'name',
      width: '115px',
    },
    {
      text: 'Control Type',
      align: 'left',
      sortable: true,
      value: 'predefinedFieldControlTypeName',
      width: '115px',
    },
    {
      text: 'Position',
      align: 'left',
      sortable: true,
      value: 'position',
      width: '115px',
    },
    {
      text: 'Value',
      align: 'left',
      sortable: true,
      value: 'value',
      width: '115px',
    },
    {
      text: 'Caption',
      align: 'left',
      sortable: true,
      value: 'caption',
      width: '115px',
    },
    {
      text: 'Size',
      align: 'left',
      sortable: true,
      value: 'size',
      width: '115px',
    },
  ];

  public constructor(
    private dialogService: CommonDialogService,
    public sessionService: SessionService,
    private httpService: HttpService,
    private modalService: NgbModal,
    public enumViewService: EnumViewService,

  ) {
    super();
  }

  public ngOnInit(): void {
    if (this.isJobTemplateScreen) this.searchModel.jobTemplateId = this.jobTemplateId;
    this.getJobTemplateParameters(this.searchModel);
  }

  public getJobTemplateParameters(searchModel: JobTemplateParameterSearchCriteria): void {
    this.httpService.getJobTemplateParameters(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Job Template Parameters", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Job Template Parameters"),
      complete: () => null,
    });
  }

  public showAddJobTemplateParameterModal(): void {
    const modalRef = this.modalService.open(JobTemplateParameterCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as JobTemplateParameterCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Job Template Parameter';
    componentInstance.modalResponse.subscribe({
      next: (value: JobTemplateParameterModel) => {
        if (!this.listData) this.listData = [];
        else {
          const eItemIdx = this.listData.findIndex(item => item.name == value.name);
          if (eItemIdx !== -1) return this.dialogService.error("Already same job param name exists");
        }
        this.listData.push(value);
        componentInstance.cancel();
        return;
      }
    });
  }

  public showUpdateJobTemplateParameterModal(item: JobTemplateParameterModel, index: number): void {
    const modalRef = this.modalService.open(JobTemplateParameterCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as JobTemplateParameterCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Job Template Parameter';
    componentInstance.jobTemplateParameterModel = JSON.parse(JSON.stringify(item)) as JobTemplateParameterModel;
    componentInstance.modalResponse.subscribe({
      next: (value: JobTemplateParameterModel) => { 
        const eItemIdx = this.listData.findIndex(item => item.name == value.name);
        if (eItemIdx !== -1 && eItemIdx !== index) return this.dialogService.error("Already same job param name exists");
        this.listData.splice(index, 1, value);
        componentInstance.cancel();
        return;
      }
    });
  }

  public deleteJobTemplateParameter(item: JobTemplateParameterModel, index: number): void {
    this.dialogService.confirmWithTitle(`Delete Job Template Parameter`, `Do you want to delete the Job Template Parameter - ${item.name}?`).then(res => {
      if (!res) return;
      this.listData.splice(index, 1);
    });
  }

  public paginationChange(e: BaseSearchCriteria): void {
    // let jobTemplateParameterSearchCriteria = new JobTemplateParameterSearchCriteria();
    // jobTemplateParameterSearchCriteria.page = e.page;
    // jobTemplateParameterSearchCriteria.pageSize = e.pageSize;
    // this.getJobTemplateParameters(jobTemplateParameterSearchCriteria);
  }

}
