import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuperAdminSummaryHeaderComponent } from './super-admin-summary-header.component';

describe('CompanyHeaderComponent', () => {
  let component: SuperAdminSummaryHeaderComponent;
  let fixture: ComponentFixture<SuperAdminSummaryHeaderComponent>;

  beforeEach(async() => {
    await TestBed.configureTestingModule({
      imports: [SuperAdminSummaryHeaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuperAdminSummaryHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
