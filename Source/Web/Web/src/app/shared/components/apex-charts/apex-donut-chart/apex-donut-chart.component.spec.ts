import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApexDonutChartComponent } from './apex-donut-chart.component';

describe('ApexDonutChartComponent', (): void => {
  let component: ApexDonutChartComponent;
  let fixture: ComponentFixture<ApexDonutChartComponent>;

  beforeEach(async(): void => {
    await TestBed.configureTestingModule({
      declarations: [ ApexDonutChartComponent ]
    })
    .compileComponents();
  });

  beforeEach((): void => {
    fixture = TestBed.createComponent(ApexDonutChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
