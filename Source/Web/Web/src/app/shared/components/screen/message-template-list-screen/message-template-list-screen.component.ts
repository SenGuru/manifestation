
import { ManifestColumnModel, Status } from 'src/common/models/app-custom-model';
import { MessageTemplateModel, MessageTemplateSearchCriteria } from 'src/common/models/app-model';
import { Component, OnInit } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { EnumViewService } from 'src/common/models/app-enum-view';
import { HttpService } from 'src/common/services/app-http-client';
import { MessageTemplateCrudModalComponent } from '../../../modals/message-template-crud-modal/message-template-crud-modal.component';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/services/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
  selector: 'app-message-template-list-screen',
  templateUrl: './message-template-list-screen.component.html',
  standalone: false
})
export class MessageTemplateListScreenComponent extends BaseListComponent<MessageTemplateModel, MessageTemplateSearchCriteria> implements OnInit {

  public override searchModel: MessageTemplateSearchCriteria = new MessageTemplateSearchCriteria();
  public predefinedMessageTemplateTypes: Status[] = [];
  public selectedPredefinedMessageTemplateTypes: Status[] = [];

  public override columns = [
    {
      text: 'Message Event',
      align: 'left',
      sortable: true,
      value: 'predefinedMessageTemplateTypeName',
      width: '115px',
    },
    {
      text: 'Email Subject',
      align: 'left',
      sortable: true,
      value: 'emailSubject',
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
    private crudService: CrudService,
    private titleService: TitleService,
    public enumViewService: EnumViewService,
  ) {
    super();
    this.titleService.pageTitle = "Message Template";
  }

  public ngOnInit(): void {
    this.getMessageTemplates(this.searchModel);
    this.predefinedMessageTemplateTypes = this.selectService.GetPredefinedMessageTemplateTypes();
  }
 
 
  public getMessageTemplates(searchModel: MessageTemplateSearchCriteria): void {
    this.httpService.getMessageTemplates(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Message Templates", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) => this.dialogService.error("Unable to get Message Templates"),
      complete: () => null,
    });
  }

  public showAddMessageTemplateModal(): void {
    const modalRef = this.modalService.open(MessageTemplateCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false, scrollable: true });
    const componentInstance = modalRef.componentInstance as MessageTemplateCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Message Template';
    componentInstance.onCloseCallback = (): void => {
      this.getMessageTemplates(this.searchModel);
    }
  }

  public showViewMessageTemplateModal(item: MessageTemplateModel): void {
    const modalRef = this.modalService.open(MessageTemplateCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false, scrollable: true });
    const componentInstance = modalRef.componentInstance as MessageTemplateCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Message Template';
    componentInstance.messageTemplateModel = JSON.parse(JSON.stringify(item)) as MessageTemplateModel;
  }

  public showUpdateMessageTemplateModal(item: MessageTemplateModel): void {
    const modalRef = this.modalService.open(MessageTemplateCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false, scrollable: true });
    const componentInstance = modalRef.componentInstance as MessageTemplateCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = 'Update';
    componentInstance.title = 'Edit Message Template';
    componentInstance.messageTemplateModel = JSON.parse(JSON.stringify(item)) as MessageTemplateModel;
    componentInstance.onCloseCallback = (): void => {
      this.getMessageTemplates(this.searchModel);
    }
  }

  public deleteMessageTemplate(item: MessageTemplateModel): void {
    this.dialogService.confirmWithTitle(`Delete Message Template`, `Do you want to delete the Message Template - ${item.predefinedMessageTemplateTypeName}?`).then(res => {
      if (!res) return;
      this.httpService.markMessageTemplateAsDeleted(item.messageTemplateId).subscribe(result => {
        if (!result.isSuccess) return this.dialogService.errorWithDefaultTitleForModel(result, "Unable to delete the Message Template");
        this.getMessageTemplates(this.searchModel);
        this.dialogService.success("Successfully deleted the Message Template");
        return;
      });
    });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    const predefinedMessageTemplateTypesArray = this.selectedPredefinedMessageTemplateTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedMessageTemplateTypeIds = predefinedMessageTemplateTypesArray;

    this.getMessageTemplates(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.predefinedMessageTemplateTypeIds = null;
    this.getMessageTemplates(this.searchModel);
  }

  public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getMessageTemplates(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    this.searchModel.page = e.page;
    this.searchModel.pageSize = e.pageSize;
    this.getMessageTemplates(this.searchModel);
  }

  public completeActionSuccess(message: string): void {
    this.dialogService.success(message);
  }

  public completeActionFailure(message: string): void {
    this.dialogService.error(message);
  }
}
