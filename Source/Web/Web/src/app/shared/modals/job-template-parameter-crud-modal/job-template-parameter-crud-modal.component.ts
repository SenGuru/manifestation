import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { JobTemplateParameterModel, LoggedInUserModel } from 'src/common/models/app-model';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgForm } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { Status } from 'src/common/models/app-custom-model';

@Component({
    selector: 'app-job-template-parameter-crud-modal',
    templateUrl: './job-template-parameter-crud-modal.component.html',
    standalone: false
})
export class JobTemplateParameterCrudModalComponent implements OnInit {

  @Input() public jobTemplateParameterModel: JobTemplateParameterModel = new JobTemplateParameterModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isJobTemplateScreen = false;
  @Output() public modalResponse: EventEmitter<JobTemplateParameterModel> = new EventEmitter<JobTemplateParameterModel>();
  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;

  public name__size: number = JobTemplateParameterModel.name__size;
  public value__size: number = JobTemplateParameterModel.value__size;
  public defaultValue__size: number = JobTemplateParameterModel.defaultValue__size;
  public caption__size: number = JobTemplateParameterModel.caption__size;
  public populatorSource__size: number = JobTemplateParameterModel.populatorSource__size;

  public predefinedFieldControlTypes: Status[] = [];
  public selectedPredefinedFieldControlType: Status | undefined;
  public predefinedFieldTypes: Status[] = [];
  public selectedPredefinedFieldType: Status | undefined;
  public predefinedFieldPopulatorTypes: Status[] = [];
  public selectedPredefinedFieldPopulatorType: Status | undefined;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService,) {
  }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedFieldControlTypes = this.selectService.GetPredefinedFieldControlTypes();
    if (!this.isCreateMode) this.selectedPredefinedFieldControlType = this.predefinedFieldControlTypes.find(item => item.id === this.jobTemplateParameterModel.predefinedFieldControlTypeId);
    this.predefinedFieldTypes = this.selectService.GetPredefinedFieldTypes();
    if (!this.isCreateMode) this.selectedPredefinedFieldType = this.predefinedFieldTypes.find(item => item.id === this.jobTemplateParameterModel.predefinedFieldTypeId);
    this.predefinedFieldPopulatorTypes = this.selectService.GetPredefinedFieldPopulatorTypes();
    if (!this.isCreateMode) this.selectedPredefinedFieldPopulatorType = this.predefinedFieldPopulatorTypes.find(item => item.id === this.jobTemplateParameterModel.predefinedFieldPopulatorTypeId);
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const jobTemplateParameterModel = this.generateModel(f.value);
    this.modalResponse.emit(jobTemplateParameterModel);
  }

  public generateModel(obj: JobTemplateParameterModel): JobTemplateParameterModel | undefined {
    if (this.selectedPredefinedFieldControlType == null)
      return;
    if (this.selectedPredefinedFieldType == null)
      return;
    if (this.selectedPredefinedFieldPopulatorType == null)
      return;
    Object.assign(this.jobTemplateParameterModel, obj);
    Object.keys(this.jobTemplateParameterModel).forEach(key => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.jobTemplateParameterModel[key] && typeof this.jobTemplateParameterModel[key] === 'object' && !(this.jobTemplateParameterModel[key] instanceof Date)) {
        //@ts-expect-error  --- following line expected to have an error
        this.jobTemplateParameterModel[key] = null;
      }
    });

    this.jobTemplateParameterModel.predefinedFieldControlTypeId = this.selectedPredefinedFieldControlType?.id ?? 0;
    this.jobTemplateParameterModel.predefinedFieldControlTypeName = this.selectedPredefinedFieldControlType?.type ?? '';
    this.jobTemplateParameterModel.predefinedFieldTypeId = this.selectedPredefinedFieldType?.id ?? 0;
    this.jobTemplateParameterModel.predefinedFieldTypeName = this.selectedPredefinedFieldType?.type ?? '';
    this.jobTemplateParameterModel.predefinedFieldPopulatorTypeId = this.selectedPredefinedFieldPopulatorType?.id ?? 0;
    this.jobTemplateParameterModel.predefinedFieldPopulatorTypeName = this.selectedPredefinedFieldPopulatorType?.type ?? '';
    return this.jobTemplateParameterModel;
  }

  public cancel(): void {
    this.activeModal.close();
  }
}
