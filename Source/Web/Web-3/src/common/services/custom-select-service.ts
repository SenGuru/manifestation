import { BudgetExpenseReportDivisionTypes, CompanyStatusTypes, ReportChartTypes, UserStatusTypes } from '../models/app-custom-enums';

import { CommonDialogService } from 'src/app/shared/services/common-dialog.service';
import { Injectable } from '@angular/core';
import { Status } from '../models/app-custom-model';

@Injectable({
  providedIn: 'root'
})
export class CustomSelectService {

  public constructor(private dialogService: CommonDialogService) {
  }

  public getCompaniestatus(): Status[] {
    return [
      { id: CompanyStatusTypes.Active, type: 'Active' },
      { id: CompanyStatusTypes.InActive, type: 'InActive' },
    ];
  }

  public getUserStatus(): Status[] {
    return [
      { id: UserStatusTypes.Active, type: 'Active' },
      { id: UserStatusTypes.InActive, type: 'InActive' },
    ];
  }

  public export(downloadUrl: string): void {
    const newWin = window.open(downloadUrl, '_blank');
    try {
      newWin?.focus();
    } catch (e) {
      this.dialogService.error('Unable to Download File. Browser popup/redirect is blocked.');
    }
  }

  // eslint-disable-next-line  
  public decimalNumberOnly(event: any, inputValue: string): boolean {
    // eslint-disable-next-line
    const charCode = (event.which) ? event.which : event.keyCode;

    // Allow numeric digits (0-9)
    if (charCode >= 48 && charCode <= 57) {
      return true;
    }

    // Allow a single decimal point
    if (charCode === 46 && !inputValue.includes('.')) {
      return true;
    }

    return false;
  }
 
  // eslint-disable-next-line
  numberOnly(event: any): boolean {
    // eslint-disable-next-line
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;
  }
  public getLogSeverityTypes(): Status[] {
    return [
      { id: 1, type: 'CRITICAL' },
      { id: 2, type: 'TRACE' },
      { id: 3, type: 'ERROR' },
      { id: 4, type: 'WARNING' },
      { id: 5, type: 'INFO' },
      { id: 6, type: 'FATAL' },
      { id: 7, type: 'DEBUG' },
      { id: 8, type: 'TRANS-BEGIN' },
      { id: 9, type: 'TRANS-COMMIT' },
      { id: 10, type: 'TRANS-ROLLBACK' },
      { id: 11, type: 'SQL' },
      { id: 12, type: 'SQL-ERROR' },
    ];
  }

  public getAppPerfLogTimeTypes(): Status[] {
    return [
      { id: 1, type: '>' },
      { id: 2, type: '<' },
      { id: 3, type: '=' },
    ];
  }

  public formatBytes(bytes: number, decimals: number): string {
    if (bytes === 0) {
      return '0 Bytes';
    }
    const k = 1024;
    const dm = decimals <= 0 ? 0 : decimals || 2;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
  }

  public GetBudgetExpenseReportDivisionTypes(): Status[] {
    return [
      { id: BudgetExpenseReportDivisionTypes.All, type: 'All' },
      { id: BudgetExpenseReportDivisionTypes.PreProduction, type: 'Pre Production' },
      { id: BudgetExpenseReportDivisionTypes.Production, type: 'Production' },
      { id: BudgetExpenseReportDivisionTypes.PostProduction, type: 'Post Production' },
      { id: BudgetExpenseReportDivisionTypes.OtherBudgetAndExpense, type: 'Other Budget And Expense' },
    ];
  }

  public GetReportChartTypes(): Status[] {
    return [
      { id: ReportChartTypes.BarChart, type: 'Bar Chart' },
      { id: ReportChartTypes.PieChart, type: 'Pie Chart' },
    ];
  }
}
