import { ConfigurationFileStorageEditorComponent } from '../modals/configuration-file-storage-editor/configuration-file-storage-editor.component';
import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Injectable({
  providedIn: 'root'
})
export class AppDialogService {

  public constructor(private modalService: NgbModal) { }

  public showAddConfiguration(
    title: string,
    type: string,
    connectionString: string): Promise<unknown> {
    const modalRef = this.modalService.open(ConfigurationFileStorageEditorComponent, {
      centered: true, beforeDismiss: () => false
    });
    const componentInstance = modalRef.componentInstance as ConfigurationFileStorageEditorComponent;
    componentInstance.title = title;
    componentInstance.type = type;
    componentInstance.connectionString = connectionString;
    return modalRef.result;
  }

}
