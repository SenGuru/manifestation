import { ApexAxisChartSeries, ApexNonAxisChartSeries } from 'ng-apexcharts';

import { ChartOptions } from 'src/common/models/app-custom-model';
import { ConvertUtils } from 'src/common/framework/common/utils/convert.utils';
import { Injectable } from '@angular/core';
import { ObjectUtils } from 'src/common/framework/common/utils/object.utils';
import { ReportChartTypes } from 'src/common/models/app-custom-enums';

@Injectable({
  providedIn: 'root'
})
export class ChartService {

  private budgetAmountColor: string = '#3ce6ac';
  private expenseAmountColor: string = '#f56767'
  private paidAmountColor: string = '#40b2f5'
  private chartHeight = "450px";
  public constructor() {
    // 
  }

  public getPieChartOptions(categories: string[], chartSeries: number[], colors: string[], predefinedCurrencyTypeId: number): ChartOptions {
    let chartOptions: ChartOptions = new ChartOptions();

    chartOptions = {
      series: chartSeries,
      chart: {
        type: 'pie',
        toolbar: {
          show: true,
        },
        zoom: {
          enabled: true,
          autoScaleYaxis: true,

        },
        selection: {
          enabled: true,
        },
        width: "90%",
        height: this.chartHeight,
      },
      labels: categories,
      responsive: [{
        breakpoint: 480,
        options: {
          chart: {
            width: '100%'
          },
          legend: {
            position: 'bottom'
          }
        }
      }],
      dataLabels: {
        enabled: true,
        formatter: function(value, { seriesIndex, w }): string {
          return w.globals.labels[seriesIndex] + ": " + Math.floor(value as number) + '%';
        },
        offsetY: -20,
        style: {
          fontSize: '12px',
          colors: ["#304758"]
        }
      },
      tooltip: {
         //@ts-expect-error  --- following line expected to have an error
        custom: function({ series, seriesIndex, dataPointIndex, w }): string {
          return (
            '<div class="arrow_box">' +
            "<span>" +
            w.globals.labels[seriesIndex] +
            ": " + ConvertUtils.getFormattedCurrecyText((series[seriesIndex] as number), predefinedCurrencyTypeId) +
            "</span>" +
            "</div>"
          );
        }
      }
    };
    return chartOptions;

  }

  private getMonthlyBudgetExpenseReportInternal(data: unknown[], reportType: ReportChartTypes, predefinedCurrencyTypeId: number): ChartOptions | null {

    if (ObjectUtils.isArrNullOrEmpty(data)) {
      return null;
    }
    const categories: string[] = [];
    const budgetTotalAmounts: number[] = [];
    const expenseTotalAmounts: number[] = [];
    const paidTotalAmounts: number[] = [];
    let isHideDataLabels: boolean = false;
    if (data.length > 4) isHideDataLabels = true;

    const chartSeries: ApexAxisChartSeries | ApexNonAxisChartSeries = [
      {
        name: "Budgets",
        data: budgetTotalAmounts
      },
      {
        name: "Expenses",
        data: expenseTotalAmounts
      },
      {
        name: "Paid",
        data: paidTotalAmounts
      }
    ];

    const colors = [this.budgetAmountColor, this.expenseAmountColor, this.paidAmountColor];

    return this.getBarChartOptionsInternal(categories, chartSeries, colors, isHideDataLabels, predefinedCurrencyTypeId);
  }

  private getBarChartOptionsInternal(categories: string[], chartSeries: ApexAxisChartSeries | ApexNonAxisChartSeries, colors: string[], hideDataLabels: boolean = false, predefinedCurrencyTypeId: number): ChartOptions {
    let chartOptions: ChartOptions = new ChartOptions();

    chartOptions = {
      series: chartSeries,

      chart: {
        type: 'bar',
        toolbar: {
          show: true,
        },
        zoom: {
          enabled: true,
          autoScaleYaxis: true,

        },
        selection: {
          enabled: true,
        },
        width: "90%",
        height: this.chartHeight,
      },
      yaxis: {
        show: true,
        logarithmic: true
      },

      colors: colors, // Customize your colors here
      plotOptions: {
        bar: {
          borderRadius: 5,
          dataLabels: {
            position: 'top', // top, center, bottom
          },
          horizontal: false,
        }
      }, // tooltip customize
      tooltip: {

        // @ts-expect-error -- ignore compilation error for next line
        custom: function({ series, seriesIndex, dataPointIndex, w }): string {
          return (
            '<div class="arrow_box">' +
            "<span>" +
            w.globals.seriesNames[seriesIndex] +
            ": " +
            ConvertUtils.getFormattedCurrecyText((series[seriesIndex][dataPointIndex] ?? 0) as number, predefinedCurrencyTypeId) +
            "</span>" +
            "</div>"
          );
        }
      },
      dataLabels: {
        enabled: !hideDataLabels,
        formatter: function(val): string {
          return ConvertUtils.getFormattedCurrecyText((val ?? 0) as number, predefinedCurrencyTypeId);
        },
        offsetY: -20,
        style: {
          fontSize: '12px',
          colors: ["#304758"]
        }
      },
      xaxis: {
        categories: categories,
      },
    };
    return chartOptions;

  }

}
