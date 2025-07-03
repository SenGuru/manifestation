import { ApexChart, ApexDataLabels, ApexGrid, ApexPlotOptions, ApexStroke, ApexTitleSubtitle, ApexXAxis, ChartComponent } from 'ng-apexcharts';
import { Component, Input, ViewChild } from '@angular/core';

import { ChartOptions } from 'src/common/models/app-custom-model';

@Component({
    selector: 'app-apex-pie-chart',
    templateUrl: './apex-pie-chart.component.html',
    styleUrls: ['./apex-pie-chart.component.css'],
    standalone: false
})
export class ApexPieChartComponent {

  @ViewChild("chart") public chart?: ChartComponent;
  @Input() public chartOptions?: Partial<ChartOptions>;

  public predefinedChart: ApexChart = { type: 'bar', };
  public predefinedXAxis: ApexXAxis = { type: 'category' };
  public predefinedLabels: ApexDataLabels = {}
  public predefinedGrid: ApexGrid = {};
  public predefinedStroke: ApexStroke = {};
  public predefinedApexTitle: ApexTitleSubtitle = {};
  public predefinedPlotOptions: ApexPlotOptions = {};
}
