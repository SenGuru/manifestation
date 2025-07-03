import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApexPieChartComponent } from './apex-pie-chart.component';

describe('ApexPieChartComponent', (): void => {
  let component: ApexPieChartComponent;
  let fixture: ComponentFixture<ApexPieChartComponent>;

  beforeEach(async(): void => {
    await TestBed.configureTestingModule({
      declarations: [ApexPieChartComponent]
    })
      .compileComponents();
  });

  beforeEach((): void => {
    fixture = TestBed.createComponent(ApexPieChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
