import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuperAdminCompanyContanierComponent } from './super-admin-company-contanier.component';

describe('SuperAdminCompanyContanierComponent', () => {
  let component: SuperAdminCompanyContanierComponent;
  let fixture: ComponentFixture<SuperAdminCompanyContanierComponent>;

  beforeEach(async() => {
    await TestBed.configureTestingModule({
      imports: [SuperAdminCompanyContanierComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuperAdminCompanyContanierComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
