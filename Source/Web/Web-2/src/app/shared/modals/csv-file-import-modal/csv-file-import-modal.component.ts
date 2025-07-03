import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Status } from 'src/common/models/app-custom-model';

@Component({
    selector: 'app-csv-file-import-modal',
    templateUrl: './csv-file-import-modal.component.html',
    styleUrls: ['./csv-file-import-modal.component.scss'],
    standalone: false
})
export class CsvFileImportModalComponent implements OnInit {

  @ViewChild('form') public importForm!: NgForm;
  @Input() public title: string | undefined;
  @Input() public isArtistImport: boolean = false;
  @Input() public isTechnicianImport: boolean = false;
  @Input() public isEquipmentImport: boolean = false;
  @Input() public isPropertyImport: boolean = false;
  @Input() public isVendorImport: boolean = false;
  @Input() public isShootDayBudgetImport: boolean = false;
  @Input() public isShootDayExpenseImport: boolean = false;
  @Input() public onCloseCallback: (() => void) | null = null;
  @Input() public companyId: number = 0;
  public fileName: string | undefined;
  public files: File | undefined | null;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public errorFiles: any = [];
  public isErrorFiles: boolean = false;
  public showFileUpload: boolean = false;
  public isChooseAll: boolean = false;
  public currentStep: number = 1;
  public tableData: unknown[] = [];
  public completedPercentage: number = 0
  public sendedItem: number = 0
  public totalItem: number = 0
  public failedItem: number = 0
  public successItem: number = 0
  public headerData = [];
  public form: FormGroup;
  public selectedCheckBox: boolean[] = [];
  public message: string | undefined;
  public currentToken: string | undefined;
  public onLoadingProgress: number = 0;
  public isLoading: boolean = false;
  public headerList = [];
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  public listCheckArray: any[] = [];
  public selectedDelegateFileSourceProcessorType: Status | null = null;

  public constructor(private activeModal: NgbActiveModal,
    private httpService: HttpService,
    private fb: FormBuilder,
    private dialogService: CommonDialogService) {
    this.form = this.fb.group({
      checkArray: this.fb.array([], [Validators.required])
    })
  }

  public ngOnInit(): void {
    this.showFileUpload = true;
  }

  public uploadFile(event: Event): void {
    const inputElementFiles = (event.currentTarget as HTMLInputElement)?.files;
    if (inputElementFiles && inputElementFiles.length > 0) {
      this.files = inputElementFiles[0];
      this.fileName = inputElementFiles[0]?.name;
      this.files = inputElementFiles[0];

      if (this.files === null) {
        this.isErrorFiles = true;
        return;
      } else {
        this.isErrorFiles = false;
      }
      this.errorFiles = [];
      // eslint-disable-next-line @typescript-eslint/no-unsafe-call

    }

  }

  public chooseAllList(): void {
    this.isChooseAll = !this.isChooseAll;
    this.listCheckArray = [];
    this.tableData.map((item, i) => {
      if (this.isChooseAll) {
        this.selectedCheckBox[i] = true;
      } else {
        this.selectedCheckBox[i] = false;
      }

      if (this.selectedCheckBox[i]) {
        this.listCheckArray.push(item);
      } else {
        this.listCheckArray = [];
      }

    });
  }

  public downloadSampleFile(): void {
    let downloadUrl: string | undefined;
    //  downloadUrl = this.httpService.downloadSampleImportProviderNetworksUrl(this.currentToken);
    // this.customSelectService.export(downloadUrl);
  }

  public fileBrowseHandler(event: Event, files: unknown): void {
    this.fileName = "";
    (event.currentTarget as HTMLInputElement).value = ''
  }

  public onClearFiles(): void {
    this.files = null;
    this.fileName = "";
  }

  public onSubmit(f: NgForm): void {

    if (this.files == null || this.fileName == '') {
      this.isErrorFiles = true;
      return;
    }
    this.currentStep++;

    if (this.currentStep === 3) {

      this.executeImportProcess();
    }
    if (this.currentStep === 4) {
      // 
    }
    if (this.currentStep > 4) {
      if (this.onCloseCallback) this.onCloseCallback();
      this.activeModal.close();
    }
  }

  public executeImportProcess(): void {

    if (this.listCheckArray?.length == 0) {
      this.dialogService.errorWithTitle("Error", 'Please select atleast one item.');
      this.currentStep--;
      return;
    }
    let payloadData: unknown[] = [];

    if (this.isChooseAll) {
      payloadData = this.tableData;
    }
    else {
      payloadData = this.listCheckArray;
    }

    // let requestList: Observable<unknown>[] = []; 
    //   payloadData.map((data) => {
    //     let movieSceneModel = new MovieSceneCreateModel();
    //     for(let headerItem of this.headerList) {
    //       movieSceneModel[headerItem] = data[headerItem];
    //     } 
    //     requestList.push(this.httpService.createMovieScene(movieSceneModel));
    //   }); 

    this.onLoadingProgress = 0;
    this.totalItem = payloadData.length;

    // forkJoin(requestList).subscribe(results => results.map((res: any, index: number) => {
    //   this.onLoadingProgress = Math.round((index + 1) / this.totalItem * 100);
    //   this.sendedItem = index + 1;
    //   let msg = '';
    //   if (res.isSuccess) {
    //     this.successItem++;
    //     msg = `# ${index + 1} - schedule item Successfully imported \n`;
    //   }
    //   else {
    //     this.failedItem++;
    //     msg = `# ${index + 1} - schedule item not imported - ${res.errorHolder.friendlyMessage + '\n'}`;
    //   }
    //   if (this.message) {
    //     this.message += msg;
    //   }
    //   else {
    //     this.message = msg;
    //   }

    //   this.onCloseCallback();
    // }
    // ));
    // const resourceImportWithFileFormModel = new ResourceImportWithFileFormModel();
    // resourceImportWithFileFormModel.uploadedFile = this.files;
    // resourceImportWithFileFormModel.companyId = this.companyId;
    // let fnc: Observable<ReturnModel<boolean>>;

    // fnc.subscribe({
    //   next: result => {
    //     this.onLoadingProgress = 100;
    //     if (!result.isSuccess) {
    //       this.dialogService.errorWithReturnModel("Unable to import Data", result);
    //       return;
    //     }
    //     this.dialogService.successWithLargeToastr("Successfully CSV Data imported");
    //   },
    //   error: (err: unknown) => this.dialogService.error("Unable to import Data"),
    //   complete: () => null,
    // });
  }

  public cancel(checkDirty: boolean): void {

    if (!checkDirty && this.onCloseCallback) this.onCloseCallback();

    if (this.importForm.dirty && checkDirty) {
      this.dialogService.cancelConfirmWithTitle('Discard the changes?', 'If you proceed, you cannot undo later.', 'Discard').then(res => {
        if (res)
          return this.activeModal.close();
      });
    }
    else {
      this.activeModal.close();
    }
  }

  public goToPrevious(): void {
    if (this.currentStep === 2) {
      this.listCheckArray = [];
      this.tableData = [];
      this.isChooseAll = false;
      this.onClearFiles();
      this.selectedCheckBox = [];
    }
    this.currentStep--;
  }

  public onCheckboxChange(e: Event, item: unknown, selectedIndex: number): void {
    const previousValue = this.selectedCheckBox[selectedIndex];
    this.selectedCheckBox[selectedIndex] = !this.selectedCheckBox[selectedIndex];
    if (previousValue) {
      const idx1 = this.listCheckArray.findIndex((i) => i === item);
      if (idx1 !== -1) {
        this.listCheckArray.splice(idx1, 1);
      }
      else {
        this.listCheckArray.push(item);
      }
    } else {
      this.listCheckArray.push(item);
    }
    if (this.tableData.length > this.listCheckArray.length && this.isChooseAll) {
      this.isChooseAll = false;
    }
    if (this.tableData.length == this.listCheckArray.length) {
      this.isChooseAll = true;
    }
  }

}
