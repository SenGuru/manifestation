import { ApexChart, ApexDataLabels, ApexGrid, ApexPlotOptions, ApexStroke, ApexTitleSubtitle, ApexXAxis } from 'ng-apexcharts';
import { Component, Input } from '@angular/core';

import { ChartOptions } from 'src/common/models/app-custom-model';

@Component({
    selector: 'app-apex-donut-chart',
    templateUrl: './apex-donut-chart.component.html',
    styleUrls: ['./apex-donut-chart.component.css'],
    standalone: false
})
export class ApexDonutChartComponent {

  @Input() public chartOptions: Partial<ChartOptions> | undefined;

  public predefinedChart: ApexChart = { type: 'bar', };
  public predefinedXAxis: ApexXAxis = { type: 'category' };
  public predefinedLabels: ApexDataLabels = {}
  public predefinedGrid: ApexGrid = {};
  public predefinedStroke: ApexStroke = {};
  public predefinedApexTitle: ApexTitleSubtitle = {};
  public predefinedPlotOptions: ApexPlotOptions = {};
  }
