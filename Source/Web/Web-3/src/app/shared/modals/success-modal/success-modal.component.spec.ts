import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuccessModalComponent } from './success-modal.component';

describe('SuccessModalComponent', (): void => {
  let component: SuccessModalComponent;
  let fixture: ComponentFixture<SuccessModalComponent>;
   
  beforeEach((() => {
    TestBed.configureTestingModule({
      declarations: [ SuccessModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(SuccessModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
