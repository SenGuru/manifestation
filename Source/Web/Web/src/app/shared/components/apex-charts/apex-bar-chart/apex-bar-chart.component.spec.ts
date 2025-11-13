import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApexBarChartComponent } from './apex-bar-chart.component';

describe('ApexBarChartComponent', (): void => {
  let component: ApexBarChartComponent;
  let fixture: ComponentFixture<ApexBarChartComponent>;

  beforeEach(async(): void => {
    await TestBed.configureTestingModule({
      declarations: [ ApexBarChartComponent ]
    })
    .compileComponents();
  });

  beforeEach((): void => {
    fixture = TestBed.createComponent(ApexBarChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
