import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm, UntypedFormBuilder } from '@angular/forms';

import { CommonDialogService } from '../../services/common-dialog.service';
import { HttpService } from 'src/common/services/app-http-client';
import { NgSelectComponent } from '@ng-select/ng-select';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SessionService } from 'src/common/framework/web/infrastructure/session/session.service';
import { StorageConnectionStringModel } from 'src/common/models/app-custom-model';

@Component({
    selector: 'app-configuration-file-storage-editor',
    templateUrl: './configuration-file-storage-editor.component.html',
    styleUrls: ['./configuration-file-storage-editor.component.css'],
    standalone: false
})
export class ConfigurationFileStorageEditorComponent implements OnInit, AfterViewInit {

  @ViewChild('vcConnectionStringField') public connectionStringField!: NgSelectComponent;
  @Input() public connectionString: string | undefined;
  @Input() public title: string | undefined;
  @Input() public type: string | undefined;
  public connectionStrings: StorageConnectionStringModel[] = [{ id: 1, type: 'Disk', initialPath: 'disk://path=' },
  { id: 2, type: 'GoogleStorage', initialPath: 'google.storage://bucket=;cred=' },
  { id: 3, type: 'AwsS3', initialPath: 'aws.s3://keyId=;key=;bucket=;region=' },
  { id: 4, type: 'AzureFiles', initialPath: 'azure.file://account=;key=' },
  { id: 5, type: 'AzureBlobStorage', initialPath: 'azure.blob://account=;key=' }
  ];

  public awsRegions: string[] = ['us-east-1', 'us-east-2', 'us-west-1', 'us-west-2', 'eu-north-1', 'eu-west-1', 'eu-west-2', 'eu-west-3',
    'eu-central-1', 'ap-northeast-1', 'ap-northeast-2', 'ap-northeast-3', 'ap-south-1', 'ap-southeast-1', 'ap-southeast-2',
    'sa-east-1', 'us-gov-east-1', 'us-gov-west-1', 'cn-north-1', 'cn-northwest-1', 'ca-central-1'];
  public selectedawsRegions: string | undefined;
  public selectDisk = false;
  public selectGoogleStorage = false;
  public selectAws = false;
  public selectAzureFiles = false;
  public selectAzureBlobStorage = false;
  public selectedConnectionStrings: StorageConnectionStringModel | undefined;
  public diskPath = '';
  public googleStorageBucket = '';
  public googleStorageCred = '';
  public awsKeyId: number | undefined;
  public awsKey = '';
  public awsBucket = '';
  public azureFilesAccount = '';
  public azureFilesKey = '';
  public azureBlobStorageAccount = '';
  public azureBlobStorageKey = '';
  public requiredPath = '';
  public splicedRequiredPath: unknown[] = [];

  public constructor(private activeModal: NgbActiveModal,
    private formBuilder: UntypedFormBuilder,
    private dialogService: CommonDialogService,
    private httpService: HttpService,
    private sessionService: SessionService) { }

  public ngOnInit(): void {
    if (this.connectionString !== undefined) {
      this.selectDisk = false;
      this.selectGoogleStorage = false;
      this.selectAws = false;
      this.selectAzureFiles = false;
      this.selectAzureBlobStorage = false;
      this.requiredPath = this.connectionString;
      if (this.connectionString.includes('disk')) {
        this.selectDisk = true;
        this.selectedConnectionStrings = this.connectionStrings.filter(list => list.type === 'Disk')[0];
        this.requiredPath = this.connectionString;
        const path1 = this.connectionString.split('=');
        this.diskPath = path1[1] ?? '';
      }
      else if (this.connectionString.includes('google')) {
        this.selectGoogleStorage = true;
        this.selectedConnectionStrings = this.connectionStrings.filter(list => list.type === 'GoogleStorage')[0];
        const path1 = this.connectionString.split('=');
        const path2 = path1[1]?.split(';') ?? '';
        this.googleStorageBucket = path2[0] ?? '';
        this.googleStorageCred = path1[2] ?? '';
      }
      else if (this.connectionString.includes('aws')) {
        this.selectAws = true;
        this.selectedConnectionStrings = this.connectionStrings.filter(list => list.type === 'AwsS3')[0];
        const path1 = this.connectionString.split('=');
        const path2 = path1[1]?.split(';') ?? '';
        const path3 = path1[3]?.split(';') ?? '';
        this.selectedawsRegions = path1[4];
        this.awsKey = path2[0] ?? '';
        this.awsKeyId = Number(path2[0]);
        this.awsBucket = path3[0] ?? '';

      }
      else if (this.connectionString.includes('azure.file')) {
        this.selectAzureFiles = true;
        this.selectedConnectionStrings = this.connectionStrings.filter(list => list.type === 'AzureFiles')[0];
        const path1 = this.connectionString.split('=');
        const path2 = path1[1]?.split(';') ?? '';
        this.azureFilesKey = path1[2] ?? '';
        this.azureFilesAccount = path2[0] ?? '';
      }
      else if (this.connectionString.includes('azure.blob')) {
        this.selectAzureBlobStorage = true;
        this.selectedConnectionStrings = this.connectionStrings.filter(list => list.type === 'AzureBlobStorage')[0];
        const path1 = this.connectionString.split('=');
        const path2 = path1[1]?.split(';') ?? '';
        this.azureBlobStorageAccount = path2[0] ?? '';
        this.azureBlobStorageKey = path1[2] ?? '';
      }
    }
  }

  public ngAfterViewInit(): void {
    if (this.connectionStringField) this.connectionStringField.focus();
  }

  public onSubmit(f: NgForm): void {
    if (!f.form.valid) {
      return;
    }
    else {
      this.activeModal.close(this.requiredPath);
    }

  }

  public close(f: NgForm): void {
    if (f.dirty) {
      this.dialogService
        .cancelConfirmWithTitle(
          'Discard the changes?',
          'If you proceed you cannot undo later',
          'cancel'
        )
        .then((res) => {
          if (!res) { return; }
          this.activeModal.close();
        });
    } else {
      this.activeModal.close();
    }
  }

  public changeConnectionString(): void {
    this.requiredPath = '';
    this.diskPath = '';
    switch (this.selectedConnectionStrings?.id) {
      case 1:
        this.requiredPath = this.selectedConnectionStrings.initialPath ?? '';
        this.selectDisk = true;
        this.selectGoogleStorage = false;
        this.selectAws = false;
        this.selectAzureFiles = false;
        this.selectAzureBlobStorage = false;
        break;
      case 2:
        this.requiredPath = this.selectedConnectionStrings.initialPath ?? '';
        this.selectDisk = false;
        this.selectGoogleStorage = true;
        this.selectAws = false;
        this.selectAzureFiles = false;
        this.selectAzureBlobStorage = false;
        break;
      case 3:
        this.requiredPath = this.selectedConnectionStrings.initialPath ?? '';
        this.selectDisk = false;
        this.selectGoogleStorage = false;
        this.selectAws = true;
        this.selectAzureFiles = false;
        this.selectAzureBlobStorage = false;
        break;
      case 4:
        this.requiredPath = this.selectedConnectionStrings.initialPath ?? '';
        this.selectDisk = false;
        this.selectGoogleStorage = false;
        this.selectAws = false;
        this.selectAzureFiles = true;
        this.selectAzureBlobStorage = false;
        break;
      case 5:
        this.requiredPath = this.selectedConnectionStrings.initialPath ?? '';
        this.selectDisk = false;
        this.selectGoogleStorage = false;
        this.selectAws = false;
        this.selectAzureFiles = false;
        this.selectAzureBlobStorage = true;
        break;
    }
  }

  public changePath(tag: string, event: Event): void {
    this.requiredPath = '';
    this.splicedRequiredPath = [];
    this.requiredPath = this.selectedConnectionStrings?.initialPath ?? '';
    this.splicedRequiredPath = this.getRegex(this.requiredPath);
    if (tag === 'disk') {
      this.splicedRequiredPath[1] = this.diskPath;
      this.requiredPath = `${this.splicedRequiredPath[0]}=${this.splicedRequiredPath[1]}`;
    }
    else if (tag == 'googleStorage') {
      this.splicedRequiredPath[1] = this.googleStorageBucket;
      this.splicedRequiredPath[2] = 'cred=';
      this.splicedRequiredPath[3] = this.googleStorageCred;
      this.requiredPath = `${this.splicedRequiredPath[0]}=${this.splicedRequiredPath[1]};${this.splicedRequiredPath[2]}${this.splicedRequiredPath[3]}`;
    }
    else if (tag == 'aws') {
      // aws.s3://keyId=;key=;bucket=;region=
      this.splicedRequiredPath[1] = Number(this.awsKeyId);
      this.splicedRequiredPath[2] = 'keyId=';
      this.splicedRequiredPath[3] = this.awsKey;
      this.splicedRequiredPath[4] = 'bucket=';
      this.splicedRequiredPath[5] = this.awsBucket;
      this.splicedRequiredPath[6] = 'region=';
      this.splicedRequiredPath[7] = this.selectedawsRegions;
      this.requiredPath = `${this.splicedRequiredPath[0]}=${this.splicedRequiredPath[1]};${this.splicedRequiredPath[2]}${this.splicedRequiredPath[3]};${this.splicedRequiredPath[4]}${this.splicedRequiredPath[5]};${this.splicedRequiredPath[6]}${this.splicedRequiredPath[7]}`;
    }
    else if (tag === 'azurefiles') {
      // azure.file://account=account_name;key=key_value
      this.splicedRequiredPath[1] = this.azureFilesAccount;
      this.splicedRequiredPath[2] = 'key=';
      this.splicedRequiredPath[3] = this.azureFilesKey;
      this.requiredPath = `${this.splicedRequiredPath[0]}=${this.splicedRequiredPath[1]};${this.splicedRequiredPath[2]}${this.splicedRequiredPath[3]}`;
    }
    else if (tag === 'azureblobstorage') {
      // azure.blob://account=account_value;key=key_value
      this.splicedRequiredPath[1] = this.azureBlobStorageAccount;
      this.splicedRequiredPath[2] = 'key=';
      this.splicedRequiredPath[3] = this.azureBlobStorageKey;
      this.requiredPath = `${this.splicedRequiredPath[0]}=${this.splicedRequiredPath[1]};${this.splicedRequiredPath[2]}${this.splicedRequiredPath[3]}`;
    }
  }

  public getRegex(x: string): string[] {
    const separators = ['\\=', ';'];
    const tokens = x.split(new RegExp(separators.join('|'), 'g'));
    return tokens;
  }
}