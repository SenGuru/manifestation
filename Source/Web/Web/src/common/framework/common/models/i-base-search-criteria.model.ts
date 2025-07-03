export interface IBaseSearchCriteria {
  page: number;
  pageSize: number;
  SearchWord: string| undefined;
  keyword: string| undefined;
  sortBy: string| undefined;
  sortAscending: boolean;
  currentRows: number;
  fromDate?: string| undefined;
  toDate?: string| undefined;
  totalRowCount: number;
  numericPageCount: number;
  resetAdvancedOption: () => void;
}
