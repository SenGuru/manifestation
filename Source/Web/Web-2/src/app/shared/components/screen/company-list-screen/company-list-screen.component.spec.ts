import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyListScreenComponent } from './company-list-screen.component';

describe('CompanyListScreenComponent', (): void => {
  let component: CompanyListScreenComponent;
  let fixture: ComponentFixture<CompanyListScreenComponent>;

  beforeEach(async(): Promise<void> => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyListScreenComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompanyListScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
