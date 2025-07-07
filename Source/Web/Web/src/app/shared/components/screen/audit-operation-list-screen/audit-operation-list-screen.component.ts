import {
  AuditOperationModel,
  AuditOperationSearchCriteria,
  UserModel,
  UserSearchCriteria,
} from "src/common/models/app-model";
import { Component, Input, OnInit } from "@angular/core";
import {
  ManifestColumnModel,
  Status,
} from "src/common/models/app-custom-model";
import { Subject, debounceTime, distinctUntilChanged, switchMap } from "rxjs";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";

import { AuditOperationCrudModalComponent } from "../../../modals/audit-operation-crud-modal/audit-operation-crud-modal.component";
import { BaseListComponent } from "src/common/base-components/base.list.component";
import { BaseSearchCriteria } from "src/common/framework/common/models/base-search-criteria.model";
import { CommonDialogService } from "src/app/shared/services/common-dialog.service";
import { CrudService } from "../../../services/crud.service";
import { EnumViewService } from "src/common/models/app-enum-view";
import { HttpService } from "src/common/services/app-http-client";
import { NgForm } from "@angular/forms";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { SelectService } from "src/common/services/select-service";
import { SessionService } from "src/common/services/session/session.service";
import { TitleService } from "src/app/shared/services/title.service";
import { getHeaderWithBlockUI } from "src/common/interceptors/httpconfig.interceptor";

@UntilDestroy()
@Component({
  selector: "app-audit-operation-list-screen",
  templateUrl: "./audit-operation-list-screen.component.html",
  standalone: false,
})
export class AuditOperationListScreenComponent
  extends BaseListComponent<AuditOperationModel, AuditOperationSearchCriteria>
  implements OnInit
{
  @Input() public auditOperationAction: string | undefined;
  @Input() public globalSearchText: string | undefined;
  public override searchModel: AuditOperationSearchCriteria =
    new AuditOperationSearchCriteria();

  public predefinedAuditOperationTypes: Status[] = [];
  public selectedPredefinedAuditOperationTypes: Status[] = [];
  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading = false;
  public users: UserModel[] = [];
  public selectedUser: UserModel | undefined;

  public override columns = [
    {
      text: "Date Time",
      align: "left",
      sortable: true,
      value: "auditDateTime",
      width: "115px",
    },
    {
      text: "Type",
      align: "left",
      sortable: true,
      value: "predefinedAuditOperationTypeName",
      width: "115px",
    },
    {
      text: "User",
      align: "left",
      sortable: true,
      value: "userName",
      width: "115px",
    },
    {
      text: "Resource Type",
      align: "left",
      sortable: true,
      value: "predefinedResourceTypeName",
      width: "115px",
    },
    {
      text: "Broswer",
      align: "left",
      sortable: true,
      value: "browserName",
      width: "115px",
    },
    {
      text: "OS",
      align: "left",
      sortable: true,
      value: "operatingSystemName",
      width: "115px",
    },
    {
      text: "Device",
      align: "left",
      sortable: true,
      value: "deviceName",
      width: "115px",
    },
    {
      text: "Ip Address",
      align: "left",
      sortable: true,
      value: "ipAddress",
      width: "115px",
    },
    {
      text: "Description",
      align: "left",
      sortable: true,
      value: "description",
      width: "115px",
    },
  ];

  public constructor(
    private dialogService: CommonDialogService,
    public sessionService: SessionService,
    private httpService: HttpService,
    private modalService: NgbModal,
    private selectService: SelectService,
    private crudService: CrudService,
    public enumViewService: EnumViewService,
    private titleService: TitleService
  ) {
    super();
    this.titleService.pageTitle = "Audit";
  }

  public ngOnInit(): void {
    this.getAuditOperations(this.searchModel);
    this.predefinedAuditOperationTypes =
      this.selectService.GetPredefinedAuditOperationTypes();
    this.getUsers();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    this.userTypeahead$
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap((searchValue) => {
          this.userTypeaheadLoading = true;
          const userSearchCriteria = new UserSearchCriteria();
          userSearchCriteria.keyword = searchValue;
          return this.httpService.getUsers(
            userSearchCriteria,
            getHeaderWithBlockUI()
          );
        }),
        untilDestroyed(this)
      )
      .subscribe({
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
    this.userTypeahead$
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap((searchValue) => {
          this.userTypeaheadLoading = true;
          const usersSearchCriteria = new UserSearchCriteria();
          usersSearchCriteria.keyword = searchValue;
          return this.httpService.getUsers(
            usersSearchCriteria,
            getHeaderWithBlockUI()
          );
        }),
        untilDestroyed(this)
      )
      .subscribe({
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

  public getUsers(): void {
    const searchModel = new UserSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.active = true;
    searchModel.sortBy = "createdDate";
    this.httpService.getUsers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess) return;
        if (data.model) this.users = data.model;
      },
      error: (error: unknown) => {
        this.dialogService.error("Not Able to get User");
      },
      complete: () => null,
    });
  }

  public getAuditOperations(searchModel: AuditOperationSearchCriteria): void {
    this.httpService.getAuditOperations(searchModel).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel(
            "Unable to get Audit Operations",
            result
          );
          return;
        }

        if (result.model) {
          this.listData = result.model;
          this.totalItems = result.totalRecords ?? 0;
        }
      },
      error: (err: unknown) =>
        this.dialogService.error("Unable to get Audit Operations"),
      complete: () => null,
    });
  }

  public showViewAuditOperationModal(item: AuditOperationModel): void {
    const modalRef = this.modalService.open(AuditOperationCrudModalComponent, {
      windowClass: "my-class",
      size: "xl",
      beforeDismiss: () => false,
    });
    const componentInstance =
      modalRef.componentInstance as AuditOperationCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = "View";
    componentInstance.title = "View Audit Operation";
    componentInstance.auditOperationModel = JSON.parse(
      JSON.stringify(item)
    ) as AuditOperationModel;
  }

  public deleteAuditOperation(item: AuditOperationModel): void {
    this.dialogService
      .confirmWithTitle(
        `Delete Audit Operation`,
        `Do you want to delete the Audit Operation - ${item.firstName}?`
      )
      .then((res) => {
        if (!res) return;
        this.httpService
          .markAuditOperationAsDeleted(item.auditOperationId)
          .subscribe((result) => {
            if (!result.isSuccess)
              return this.dialogService.errorWithDefaultTitleForModel(
                result,
                "Unable to delete the Audit Operation"
              );
            this.getAuditOperations(this.searchModel);
            this.dialogService.success(
              "Successfully deleted the Audit Operation"
            );
            return;
          });
      });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    const predefinedAuditOperationTypesArray =
      this.selectedPredefinedAuditOperationTypes?.map((item) => item.id ?? 0);
    this.searchModel.predefinedAuditOperationTypeIds =
      predefinedAuditOperationTypesArray;
    this.searchModel.userId = this.selectedUser
      ? this.selectedUser.userId
      : null;
    this.getAuditOperations(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.searchModel.predefinedAuditOperationTypeIds = null;
    this.searchModel.userId = null;
    this.searchModel.movieId = null;
    this.searchModel.predefinedResourceTypeIds = null;
    this.getAuditOperations(this.searchModel);
  }

  public changeSort(header: ManifestColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, {
      currentSort: this.currentSort,
    });
    this.currentSort = this.searchModel.sortAscending ? "up" : "down";
    this.getAuditOperations(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    const auditOperationSearchCriteria = new AuditOperationSearchCriteria();
    auditOperationSearchCriteria.page = e.page;
    auditOperationSearchCriteria.pageSize = e.pageSize;
    this.getAuditOperations(auditOperationSearchCriteria);
  }
}
