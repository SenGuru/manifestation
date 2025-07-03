import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HostAdminLayoutComponent } from './host-admin-layout.component';

describe('HostAdminLayoutComponent', () => {
  let component: HostAdminLayoutComponent;
  let fixture: ComponentFixture<HostAdminLayoutComponent>;

  beforeEach((() => {
    TestBed.configureTestingModule({
      declarations: [ HostAdminLayoutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HostAdminLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
