import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CsvReaderService {

  public constructor() {
    // 
  }

  public getDataRecordsArrayFromCSVFile(csvRecordsArray: unknown[], headerArray: unknown[]): unknown[] {
    const csvArr: unknown[] = [];

    for (let i = 1; i < csvRecordsArray.length; i++) {
      const curruntRecord = (csvRecordsArray[i] as string).split(',');
      let currentRow = "";
      if (curruntRecord.length == headerArray.length) {
        for (let j = 0; j < curruntRecord.length; j++) {
          currentRow = currentRow + headerArray[j] + ":'" + curruntRecord[j] + "',";
        }
        currentRow = "{" + currentRow.slice(0, -1) + "}";
        csvArr.push(currentRow);
      }
    }
    return csvArr;
  }

  public isValidCSVFile(file: File): boolean {
    if (file.name.endsWith(".csv")) {
      return true;
    } else {
      return false;
    }
  }

  public getHeaderArray(csvRecordsArr: unknown[]): unknown[] {
    const headers = (csvRecordsArr[0] as string).split(',');
    const headerArray = [];
    for (let j = 0; j < headers.length; j++) {
      headerArray.push(headers[j]);
    }
    return headerArray;
  }

  public fileReset(csvReader: { nativeElement: { value: string; }; }): void {
    csvReader.nativeElement.value = "";
  }
}
