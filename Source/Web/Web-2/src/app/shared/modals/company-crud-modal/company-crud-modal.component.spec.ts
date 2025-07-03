import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyCrudModalComponent } from './company-crud-modal.component';

describe('CompanyCrudModalComponent', (): void => {
  let component: CompanyCrudModalComponent;
  let fixture: ComponentFixture<CompanyCrudModalComponent>;

  beforeEach(async(): Promise<void> => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyCrudModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompanyCrudModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
