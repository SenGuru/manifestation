import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HostAdminNavbarComponent } from './host-admin-navbar.component';

describe('HostAdminNavbarComponent', () => {
  let component: HostAdminNavbarComponent;
  let fixture: ComponentFixture<HostAdminNavbarComponent>;

  beforeEach(async() => {
    await TestBed.configureTestingModule({
      imports: [HostAdminNavbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HostAdminNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
