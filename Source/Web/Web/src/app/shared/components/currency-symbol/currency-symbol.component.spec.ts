import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrencySymbolComponent } from './currency-symbol.component';

describe('CurrencySymbolComponent', (): void => {
  let component: CurrencySymbolComponent;
  let fixture: ComponentFixture<CurrencySymbolComponent>;

  beforeEach(async(): Promise<void> => {
    await TestBed.configureTestingModule({
      imports: [CurrencySymbolComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CurrencySymbolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
