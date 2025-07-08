import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-currency-symbol',
    templateUrl: './currency-symbol.component.html',
    styleUrl: './currency-symbol.component.scss',
    standalone: false
})
export class CurrencySymbolComponent {

  @Input() public predefinedCurrencyTypeId: number | undefined;
}
