import { ComponentFixture, TestBed, } from '@angular/core/testing';

import { ChangePasswordComponent } from './change-password.component';

describe('ChangePasswordComponent', (): void => {
  let component: ChangePasswordComponent;
  let fixture: ComponentFixture<ChangePasswordComponent>;

  beforeEach(((): void => {
    TestBed.configureTestingModule({
      declarations: [ ChangePasswordComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(ChangePasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
