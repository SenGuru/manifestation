import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyConfigScreenComponent } from './company-config-screen.component';

describe('CompanyConfigScreenComponent', (): void => {
  let component: CompanyConfigScreenComponent;
  let fixture: ComponentFixture<CompanyConfigScreenComponent>;

  beforeEach(async(): Promise<void> => {
    await TestBed.configureTestingModule({
      imports: [CompanyConfigScreenComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompanyConfigScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
