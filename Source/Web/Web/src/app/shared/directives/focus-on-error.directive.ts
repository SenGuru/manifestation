import { ContentChild, Directive, ElementRef, HostListener } from '@angular/core';

import { NgSelectComponent } from '@ng-select/ng-select';

@Directive({
    selector: '[appFocusOnError]',
    standalone: false
})
export class FocusOnErrorDirective {

  @ContentChild(NgSelectComponent) public select!: NgSelectComponent;

 public constructor(private el: ElementRef) { }

  @HostListener("submit")
public onFormSubmit(): void {
    // eslint-disable-next-line @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-call
    const invalidControl = this.el.nativeElement.querySelector(".ng-invalid") as HTMLElement;
    if (invalidControl) {
      if (invalidControl.tagName === "NG-SELECT") {
        const container = invalidControl.querySelector('div.ng-value-container');
        if (container) {
          const inputInSelect = container.querySelector('div.ng-input > input') as HTMLInputElement;
          if (inputInSelect) {
            inputInSelect.focus();
          }
        }
        return;
      }
      invalidControl.focus();
    }

  }

}
