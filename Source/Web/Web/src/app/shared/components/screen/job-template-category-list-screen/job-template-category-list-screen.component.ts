import {
  JobTemplateCategoryModel,
  JobTemplateCategorySearchCriteria,
} from "src/common/models/app-model";
import { Component, Input, OnInit } from "@angular/core";
import { Subject, debounceTime, distinctUntilChanged, switchMap } from "rxjs";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";

import { AuditDuoColumnModel } from "src/common/models/app-custom-model";
import { BaseListComponent } from "src/common/base-components/base.list.component";
import { BaseSearchCriteria } from "src/common/framework/common/models/base-search-criteria.model";
import { CommonDialogService } from "src/app/shared/services/common-dialog.service";
import { CrudService } from "../../../services/crud.service";
import { HttpService } from "src/common/services/app-http-client";
import { JobTemplateCategoryCrudModalComponent } from "../../../modals/job-template-category-crud-modal/job-template-category-crud-modal.component";
import { NgForm } from "@angular/forms";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { SessionService } from "src/common/services/session/session.service";
import { TitleService } from "src/app/shared/services/title.service";
import { getHeaderWithBlockUI } from "src/common/interceptors/httpconfig.interceptor";

@UntilDestroy()
@Component({
  selector: "app-job-template-category-list-screen",
  templateUrl: "./job-template-category-list-screen.component.html",
  standalone: false,
})
export class JobTemplateCategoryListScreenComponent
  extends BaseListComponent<
    JobTemplateCategoryModel,
    JobTemplateCategorySearchCriteria
  >
  implements OnInit
{
  @Input() public jobTemplateCategoryAction: string | undefined;
  @Input() public globalSearchText: string | undefined;
  public override searchModel: JobTemplateCategorySearchCriteria =
    new JobTemplateCategorySearchCriteria();
  public override columns = [
    {
      text: "Name",
      align: "left",
      sortable: true,
      value: "name",
      width: "115px",
    },
    {
      text: "Is Deleted",
      align: "left",
      sortable: true,
      value: "isDeleted",
      width: "115px",
    },
    {
      text: "Created Date",
      align: "left",
      sortable: true,
      value: "createdDate",
      width: "115px",
    },
    {
      text: "Created By",
      align: "left",
      sortable: true,
      value: "createdByUserFirstName",
      width: "115px",
    },
    {
      text: "Modified Date",
      align: "left",
      sortable: true,
      value: "modifiedDate",
      width: "115px",
    },
    {
      text: "Modified By",
      align: "left",
      sortable: true,
      value: "modifiedByUserFirstName",
      width: "115px",
    },
  ];

  public constructor(
    private dialogService: CommonDialogService,
    public sessionService: SessionService,
    private httpService: HttpService,
    private modalService: NgbModal,
    private titleService: TitleService,
    private crudService: CrudService
  ) {
    super();
    this.titleService.pageTitle = "Job Categories";
  }

  public ngOnInit(): void {
    this.getJobTemplateCategorys(this.searchModel);
  }

  public getJobTemplateCategorys(
    searchModel: JobTemplateCategorySearchCriteria
  ): void {
    this.httpService.getJobTemplateCategorys(searchModel).subscribe({
      next: (result) => {
        if (!result.isSuccess) {
          this.dialogService.errorWithReturnModel(
            "Unable to get Job Template Categorys",
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
        this.dialogService.error("Unable to get Job Template Categorys"),
      complete: () => null,
    });
  }

  public showAddJobTemplateCategoryModal(): void {
    const modalRef = this.modalService.open(
      JobTemplateCategoryCrudModalComponent,
      { windowClass: "my-class", size: "xl", beforeDismiss: () => false }
    );
    const componentInstance =
      modalRef.componentInstance as JobTemplateCategoryCrudModalComponent;
    componentInstance.isCreateMode = true;
    componentInstance.buttonText = "Create";
    componentInstance.title = "Create Job Template Category";
    componentInstance.onCloseCallback = (): void => {
      this.getJobTemplateCategorys(this.searchModel);
    };
  }

  public showViewJobTemplateCategoryModal(
    item: JobTemplateCategoryModel
  ): void {
    const modalRef = this.modalService.open(
      JobTemplateCategoryCrudModalComponent,
      { windowClass: "my-class", size: "xl", beforeDismiss: () => false }
    );
    const componentInstance =
      modalRef.componentInstance as JobTemplateCategoryCrudModalComponent;
    componentInstance.isViewMode = true;
    componentInstance.buttonText = "View";
    componentInstance.title = "View Job Template Category";
    componentInstance.jobTemplateCategoryModel = JSON.parse(
      JSON.stringify(item)
    ) as JobTemplateCategoryModel;
  }

  public showUpdateJobTemplateCategoryModal(
    item: JobTemplateCategoryModel
  ): void {
    const modalRef = this.modalService.open(
      JobTemplateCategoryCrudModalComponent,
      { windowClass: "my-class", size: "xl", beforeDismiss: () => false }
    );
    const componentInstance =
      modalRef.componentInstance as JobTemplateCategoryCrudModalComponent;
    componentInstance.isEditMode = true;
    componentInstance.buttonText = "Update";
    componentInstance.title = "Edit Job Template Category";
    componentInstance.jobTemplateCategoryModel = JSON.parse(
      JSON.stringify(item)
    ) as JobTemplateCategoryModel;
    componentInstance.onCloseCallback = (): void => {
      this.getJobTemplateCategorys(this.searchModel);
    };
  }

  public deleteJobTemplateCategory(item: JobTemplateCategoryModel): void {
    this.dialogService
      .confirmWithTitle(
        `Delete JobTemplateCategory`,
        `Do you want to delete the jobTemplateCategory - ${item.name}?`
      )
      .then((res) => {
        if (!res) return;
        this.httpService
          .markJobTemplateCategoryAsDeleted(item.jobTemplateCategoryId)
          .subscribe((result) => {
            if (!result.isSuccess)
              return this.dialogService.errorWithDefaultTitleForModel(
                result,
                "Unable to delete the Job Template Category"
              );
            this.getJobTemplateCategorys(this.searchModel);
            this.dialogService.success("Successfully deleted the Job Template");
            return;
          });
      });
  }

  public onSearch(f: NgForm): void {
    if (f.invalid) return;
    this.getJobTemplateCategorys(this.searchModel);
  }

  public onReset(f: NgForm): void {
    f.resetForm();
    this.searchModel.page = 1;
    this.getJobTemplateCategorys(this.searchModel);
  }

  public changeSort(header: AuditDuoColumnModel): void {
    this.crudService.baseChangeSort(header, this.searchModel, {
      currentSort: this.currentSort,
    });
    this.currentSort = this.searchModel.sortAscending ? "up" : "down";
    this.getJobTemplateCategorys(this.searchModel);
  }

  public paginationChange(e: BaseSearchCriteria): void {
    const jobTemplateCategorySearchCriteria =
      new JobTemplateCategorySearchCriteria();
    jobTemplateCategorySearchCriteria.page = e.page;
    jobTemplateCategorySearchCriteria.pageSize = e.pageSize;
    this.getJobTemplateCategorys(jobTemplateCategorySearchCriteria);
  }
}
