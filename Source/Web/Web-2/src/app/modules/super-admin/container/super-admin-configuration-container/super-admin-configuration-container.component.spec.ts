import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuperAdminConfigurationContainerComponent } from './super-admin-configuration-container.component';

describe('SuperAdminConfigurationContainerComponent', () => {
  let component: SuperAdminConfigurationContainerComponent;
  let fixture: ComponentFixture<SuperAdminConfigurationContainerComponent>;

  beforeEach(async() => {
    await TestBed.configureTestingModule({
      imports: [SuperAdminConfigurationContainerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuperAdminConfigurationContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
