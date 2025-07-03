
import { AuditDuoColumnModel, Status } from 'src/common/models/app-custom-model';
import { CompanyModel, CompanySearchCriteria, MessageToSendModel, MessageToSendSearchCriteria } from 'src/common/models/app-model';
import { Component, OnInit } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BaseListComponent } from 'src/common/base-components/base.list.component';
import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { CrudService } from '../../../services/crud.service';
import { HttpService } from 'src/common/services/app-http-client';
import { MessageToSendCrudModalComponent } from '../../../modals/message-to-send-crud-modal/message-to-send-crud-modal.component';
import { NgForm } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SelectService } from 'src/common/services/select-service';
import { SessionService } from 'src/common/services/session/session.service';
import { TitleService } from 'src/app/shared/services/title.service';
import { getHeaderWithBlockUI } from 'src/common/interceptors/httpconfig.interceptor';

@UntilDestroy()
@Component({
    selector: 'app-message-to-send-list-screen',
    templateUrl: './message-to-send-list-screen.component.html',
    standalone: false
})
export class MessageToSendListScreenComponent extends BaseListComponent<MessageToSendModel, MessageToSendSearchCriteria> implements OnInit {

  public override searchModel: MessageToSendSearchCriteria = new MessageToSendSearchCriteria();
  public companyTypeahead$ = new Subject<string>();
  public companyTypeaheadLoading = false;
  public companys: CompanyModel[] = [];
  public selectedCompany: CompanyModel | undefined;
  public predefinedMessageToSendTypes: Status[] = [];
  public selectedPredefinedMessageToSendTypes: Status[] = [];

  public override columns = [
    {
      text: 'Message To Send Type',
      align: 'left',
      sortable: true,
      value: 'predefinedMessageToSendTypeName',
      width: '115px',
    },
    {
      text: 'Realtime Message Name',
      align: 'left',
      sortable: true,
      value: 'realtimeMessageName',
      width: '115px',
    },
    {
      text: 'Message From',
      align: 'left',
      sortable: true,
      value: 'messageFrom',
      width: '115px',
    },
    {
      text: 'Message To',
      align: 'left',
      sortable: true,
      value: 'messageTo',
      width: '115px',
    },
    {
      text: 'Message Subject',
      align: 'left',
      sortable: true,
      value: 'messageSubject',
      width: '115px',
    },
    {
      text: 'Message Body',
      align: 'left',
      sortable: true,
      value: 'messageBody',
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
  ) {
    super();
    this.titleService.pageTitle = "Messages To Send";
  }

public ngOnInit(): void {
    this.getMessageToSends(this.searchModel);
    this.getCompanies();
    this.predefinedMessageToSendTypes = this.selectService.GetPredefinedMessageToSendTypes();
    this.initTypeAheads();
  }

public initTypeAheads(): void {
    this.companyTypeahead$.pipe(debounceTime(500), distinctUntilChanged(), switchMap((searchValue) => {
      this.companyTypeaheadLoading = true;
      const companySearchCriteria = new CompanySearchCriteria();
      companySearchCriteria.keyword = searchValue;
      return this.httpService.getCompanies(companySearchCriteria, getHeaderWithBlockUI());
    }), untilDestroyed(this)).subscribe({
      next: (data) => {
         if (data.model) {
          this.companyTypeaheadLoading = false;
          if (data.model) this.companys = data.model;
        }
      },
      error: (err: unknown) => {
        this.companyTypeaheadLoading = false;
      },
      complete: () => null,
    });
  }

public getCompanies(): void {
    const searchModel = new CompanySearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = 'createdDate';
    searchModel.isActive = true;
    this.httpService.getCompanies(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess)
          return;
        if (data.model) this.companys = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error('Not Able to get Company');
      },
      complete: () => null,
    });
  } 

public getMessageToSends(searchModel: MessageToSendSearchCriteria): void {
    this.httpService.getMessageToSends(searchModel).subscribe({
      next: result => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel("Unable to get Message To Send", result);
          return;
        }
        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown)=> this.dialogService.error("Unable to get Message To Send"),
      complete: () => null,
    });
  }

public showAddMessageToSendModal(): void {
    const modalRef = this.modalService.open(MessageToSendCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as MessageToSendCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = 'Create';
    componentInstance.title = 'Create Message To Send';
    componentInstance.onCloseCallback = (): void => {
      this.getMessageToSends(this.searchModel);
    }
  }

public showViewMessageToSendModal(item: MessageToSendModel): void {
    const modalRef = this.modalService.open(MessageToSendCrudModalComponent, { windowClass: 'my-class', size: 'xl', beforeDismiss: () => false });
    const componentInstance = modalRef.componentInstance as MessageToSendCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = 'View';
    componentInstance.title = 'View Message To Send';
    componentInstance.messageToSendModel = JSON.parse(JSON.stringify(item)) as MessageToSendModel;
  }

public onSearch(f: NgForm): void {
    if (f.invalid) return;
    this.searchModel.companyId = this.selectedCompany ? this.selectedCompany.companyId : null;
    const predefinedMessageToSendTypesArray = this.selectedPredefinedMessageToSendTypes?.map(item => item.id ?? 0);
    this.searchModel.predefinedMessageToSendTypeIds = predefinedMessageToSendTypesArray;
    this.getMessageToSends(this.searchModel);
  }

public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.companyId = null;
    this.searchModel.predefinedMessageToSendTypeIds = null;
    this.searchModel.realtimeMessageId = null;
    this.getMessageToSends(this.searchModel);
  }

public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, { currentSort: this.currentSort });
    this.currentSort = this.searchModel.sortAscending ? 'up' : 'down';
    this.getMessageToSends(this.searchModel);
  }

public paginationChange(e: BaseSearchCriteria): void {
    const messageToSendSearchCriteria = new MessageToSendSearchCriteria();
    messageToSendSearchCriteria.page = e.page;
    messageToSendSearchCriteria.pageSize = e.pageSize;
    this.getMessageToSends(messageToSendSearchCriteria);
  }

}
