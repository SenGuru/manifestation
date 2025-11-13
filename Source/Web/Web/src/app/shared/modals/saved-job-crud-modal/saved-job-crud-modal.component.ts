import { Component, Input, OnInit } from "@angular/core";
import { FormGroup, NgForm } from "@angular/forms";
import {
  JobTemplateModel,
  JobTemplateSearchCriteria,
  LoggedInUserModel,
  SavedJobCreateModel,
  SavedJobModel,
  SavedJobUpdateModel,
  UserModel,
  UserSearchCriteria,
} from "src/common/models/app-model";
import { Subject, debounceTime, distinctUntilChanged, switchMap } from "rxjs";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import {
  getHeaderWithBlockUI,
  getHeaderWithLoadingText,
} from "src/common/interceptors/httpconfig.interceptor";

import { CommonDialogService } from "../../services/common-dialog.service";
import { HttpService } from "src/common/services/app-http-client";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { SelectService } from "src/common/services/select-service";
import { SessionService } from "src/common/framework/web/infrastructure/session/session.service";
import { Status } from "src/common/models/app-custom-model";

@UntilDestroy()
@Component({
  selector: "app-saved-job-crud-modal",
  templateUrl: "./saved-job-crud-modal.component.html",
  standalone: false,
})
export class SavedJobCrudModalComponent implements OnInit {
  @Input() public savedJobModel: SavedJobModel = new SavedJobModel();
  @Input() public title: string | undefined;
  @Input() public buttonText: string | undefined;
  @Input() public isCreateMode = false;
  @Input() public isViewMode = false;
  @Input() public isEditMode = false;
  @Input() public isDeletable = false;

  public currentUser: LoggedInUserModel | null = null;
  public onCloseCallback: (() => void) | null = null;
  public aSubmittedDate: Date | null = null;
  public executableFolder__size: number = SavedJobModel.executableFolder__size;
  public executableName__size: number = SavedJobModel.executableName__size;
  public executableParameter__size: number =
    SavedJobModel.executableParameter__size;
  public serializedJobParameter__size: number =
    SavedJobModel.serializedJobParameter__size;
  public reportTemplateFile__size: number =
    SavedJobModel.reportTemplateFile__size;
  public customExportName__size: number = SavedJobModel.customExportName__size;
  public jobName__size: number = SavedJobModel.jobName__size;

  public predefinedJobTypes: Status[] = [];
  public selectedPredefinedJobType: Status | undefined;
  public predefinedJobOrReportTypes: Status[] = [];
  public selectedPredefinedJobOrReportType: Status | undefined;
  public jobTemplateTypeahead$ = new Subject<string>();
  public jobTemplateTypeaheadLoading = false;
  public jobTemplates: JobTemplateModel[] = [];
  public selectedJobTemplate: JobTemplateModel | undefined;
  public userTypeahead$ = new Subject<string>();
  public userTypeaheadLoading = false;
  public users: UserModel[] = [];
  public selectedUser: UserModel | undefined;

  public constructor(
    private httpService: HttpService,
    private dialogService: CommonDialogService,
    private sessionService: SessionService,
    public activeModal: NgbActiveModal,
    private selectService: SelectService
  ) { }

  public ngOnInit(): void {
    this.currentUser = this.sessionService.getCurrentUserInfo();
    this.predefinedJobTypes = this.selectService.GetPredefinedJobTypes();
    if (!this.isCreateMode)
      this.selectedPredefinedJobType = this.predefinedJobTypes.find(
        (item) => item.id === this.savedJobModel.predefinedJobTypeId
      );
    this.predefinedJobOrReportTypes =
      this.selectService.GetPredefinedJobOrReportTypes();
    if (!this.isCreateMode)
      this.selectedPredefinedJobOrReportType =
        this.predefinedJobOrReportTypes.find(
          (item) => item.id === this.savedJobModel.predefinedJobOrReportTypeId
        );
    this.getJobTemplates();
    this.getUsers();
    this.initTypeAheads();
  }

  public initTypeAheads(): void {
    this.jobTemplateTypeahead$
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap((searchValue) => {
          this.jobTemplateTypeaheadLoading = true;
          const jobTemplateSearchCriteria = new JobTemplateSearchCriteria();
          jobTemplateSearchCriteria.keyword = searchValue;
          if (!this.isCreateMode)
            jobTemplateSearchCriteria.selectedId =
              this.savedJobModel.jobTemplateId;
          return this.httpService.getJobTemplates(
            jobTemplateSearchCriteria,
            getHeaderWithBlockUI()
          );
        }),
        untilDestroyed(this)
      )
      .subscribe({
        next: (data) => {
          this.jobTemplateTypeaheadLoading = false;
          if (data.model) this.jobTemplates = data.model;
        },
        error: (err: unknown) => {
          this.jobTemplateTypeaheadLoading = false;
        },
        complete: () => null,
      });

    // this.submittedByUserTypeahead$.pipe(debounceTime(500),distinctUntilChanged(),switchMap((searchValue) => {
    //   this.submittedByUserTypeaheadLoading = true;
    //   let submittedByUserSearchCriteria = new SubmittedByUserSearchCriteria();
    //   submittedByUserSearchCriteria.keyword = searchValue;
    //   return this.httpService.getSubmittedByUsers(submittedByUserSearchCriteria, getHeaderWithBlockUI());
    // }),untilDestroyed(this)).subscribe({
    //   next: (data) => {
    //     this.submittedByUserTypeaheadLoading = false;
    //     this.submittedByUsers = data.model;
    // },
    // error: (err: unknown) => {
    // this.submittedByUserTypeaheadLoading = false;
    // console.error(err);
    // },
    // complete: () => null,});
    this.userTypeahead$
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap((searchValue) => {
          this.userTypeaheadLoading = true;
          const userSearchCriteria = new UserSearchCriteria();
          userSearchCriteria.keyword = searchValue;
          if (!this.isCreateMode)
            userSearchCriteria.selectedId = this.savedJobModel.userId;
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
  }

  public getJobTemplates(): void {
    const searchModel: JobTemplateSearchCriteria =
      new JobTemplateSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = "createdDate";
    if (!this.isCreateMode)
      searchModel.selectedId = this.savedJobModel.jobTemplateId;
    this.httpService.getJobTemplates(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess) return;
        if (data.model) {
          this.jobTemplates = data.model;
          if (!this.isCreateMode)
            this.selectedJobTemplate = data.model.find(
              (item) => item.jobTemplateId === this.savedJobModel.jobTemplateId
            );
        }
      },
      error: (error: unknown) => {
        this.dialogService.error("Not Able to get Job Template");
      },
      complete: () => null,
    });
  }

  public getUsers(): void {
    const searchModel: UserSearchCriteria = new UserSearchCriteria();
    searchModel.sortAscending = false;
    searchModel.sortBy = "createdDate";
    if (!this.isCreateMode) searchModel.selectedId = this.savedJobModel.userId;
    this.httpService.getUsers(searchModel).subscribe({
      next: (data) => {
        if (!data.isSuccess) return;
        if (data.model) {
          this.users = data.model;
          if (!this.isCreateMode)
            this.selectedUser = data.model.find(
              (item) => item.userId === this.savedJobModel.userId
            );
        }
      },
      error: (error: unknown) => {
        this.dialogService.error("Not Able to get User");
      },
      complete: () => null,
    });
  }

  public onSubmit(f: NgForm): void {
    if (!f.valid) {
      return;
    }

    // eslint-disable-next-line @typescript-eslint/no-unsafe-argument
    const savedJobModel = this.generateModel(f.value);
    if (savedJobModel) {
      if (this.isCreateMode) {
        this.createSavedJob(savedJobModel, f);
      } else {
        this.updateSavedJob(savedJobModel, f);
      }
    }
  }

  public generateModel(obj: SavedJobModel): SavedJobModel | undefined {
    if (this.selectedPredefinedJobType == null) return;
    if (this.selectedPredefinedJobOrReportType == null) return;
    if (this.selectedJobTemplate == null) return;
    if (this.selectedUser == null) return;
    Object.assign(this.savedJobModel, obj);
    Object.keys(this.savedJobModel).forEach((key) => {
      //@ts-expect-error  --- following line expected to have an error
      if (this.savedJobModel[key] &&
        //@ts-expect-error  --- following line expected to have an error
        typeof this.savedJobModel[key] === "object" &&
        //@ts-expect-error  --- following line expected to have an error
        !(this.savedJobModel[key] instanceof Date)
      ) {
        //@ts-expect-error  --- following line expected to have an error
        this.savedJobModel[key] = null;
      }
    });

    this.savedJobModel.predefinedJobTypeId =
      this.selectedPredefinedJobType?.id ?? 0;
    this.savedJobModel.predefinedJobOrReportTypeId = this
      .selectedPredefinedJobOrReportType
      ? this.selectedPredefinedJobOrReportType.id
      : null;
    this.savedJobModel.jobTemplateId =
      this.selectedJobTemplate?.jobTemplateId ?? 0;
    this.savedJobModel.userId = this.selectedUser
      ? this.selectedUser.userId
      : null;
    return this.savedJobModel;
  }

  public createSavedJob(savedJobModel: SavedJobCreateModel, f: NgForm): void {
    this.httpService
      .createSavedJob(savedJobModel, getHeaderWithLoadingText("Saving ..."))
      .subscribe({
        next: (result) => {
          if (!result.isSuccess) {
            this.dialogService.errorWithReturnModel(
              "Not Able to Create SavedJob",
              result
            );
            return;
          }
          this.dialogService.success("Successfully Created the SavedJob.");
          f.resetForm();
          this.activeModal.close();
          if (this.onCloseCallback) this.onCloseCallback();
        },
        error: (err: unknown) =>
          this.dialogService.error("Not Able to create SavedJob"),
        complete: () => null,
      });
  }

  public updateSavedJob(savedJobModel: SavedJobUpdateModel, f: NgForm): void {
    this.httpService
      .updateSavedJob(savedJobModel, getHeaderWithLoadingText("Updating ..."))
      .subscribe({
        next: (result) => {
          if (!result.isSuccess) {
            this.dialogService.errorWithReturnModel(
              "Not Able to update SavedJob",
              result
            );
            return;
          }
          this.dialogService.success("Successfully updated the SavedJob.");
          f.resetForm();
          this.activeModal.close();
          if (this.onCloseCallback) this.onCloseCallback();
        },
        error: (err: unknown) =>
          this.dialogService.error("Not Able to update SavedJob"),
        complete: () => null,
      });
  }

  public cancel(): void {
    this.activeModal.close();
  }
}
