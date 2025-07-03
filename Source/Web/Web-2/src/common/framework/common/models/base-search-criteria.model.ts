import { IBaseSearchCriteria } from './i-base-search-criteria.model';

export class BaseSearchCriteria implements IBaseSearchCriteria {
  public page = 1;
  public pageSize = 10;
  public keyword = '';
  public SearchWord = '';
  public sortBy = '';
  public sortAscending = true;
  public currentRows = 0;
  // public fromDate = '';
  // public  toDate = '';
  public selectedId?: number | null;
  public totalRowCount = 10;
  public numericPageCount = 1;
  public resetAdvancedOption(): void {
    // 
  }
}
