import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-ui-field-error-display',
    templateUrl: './form-field-error-display.component.html',
    standalone: false
})
export class FormFieldErrorDisplayComponent {

  @Input() public errorMsg: string | undefined;
  @Input() public displayError: boolean | undefined;

}
