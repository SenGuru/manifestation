import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomToasterComponent } from './custom-toaster.component';

describe('CustomToasterComponent', (): void => {
  let component: CustomToasterComponent;
  let fixture: ComponentFixture<CustomToasterComponent>;

  beforeEach(((): void => {
    TestBed.configureTestingModule({
      declarations: [ CustomToasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(CustomToasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
