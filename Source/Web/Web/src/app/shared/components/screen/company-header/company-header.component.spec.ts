import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyHeaderComponent } from './company-header.component';

describe('CompanyHeaderComponent', (): void => {
  let component: CompanyHeaderComponent;
  let fixture: ComponentFixture<CompanyHeaderComponent>;

  beforeEach(async(): Promise<void> => {
    await TestBed.configureTestingModule({
      imports: [CompanyHeaderComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompanyHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
