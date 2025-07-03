import { Directive, EventEmitter, HostBinding, HostListener, Output } from '@angular/core';

@Directive({
  selector: '[appFileDragDrop]'
})
export class FileDragDropDirective {

  // tslint:disable-next-line: no-output-on-prefix
  @Output() public fileDropped = new EventEmitter<FileList>();
  // '#f5fcff'
  @HostBinding('style.background-color') private background = '#ffffff';
  @HostBinding('style.opacity') private opacity = '1';
  // Dragover listener
  @HostListener('dragover', ['$event']) public onDragOver(evt: Event): void {
    evt.preventDefault();
    evt.stopPropagation();
    this.background = '#f5fcff';
    this.opacity = '0.8'
  }

  // Dragleave listener
  @HostListener('dragleave', ['$event']) public onDragLeave(evt: Event): void {
    evt.preventDefault();
    evt.stopPropagation();
    this.background = '#f5fcff'
    this.opacity = '1'
  }

  // Drop listener
  @HostListener('drop', ['$event']) public ondrop(evt: DragEvent): void {
    evt.preventDefault();
    evt.stopPropagation();
    this.background = '#ffffff'
    this.opacity = '1'
    const files = evt?.dataTransfer?.files;
    if (files && files?.length > 0) {
      this.fileDropped.emit(files)
    }
  }

}
