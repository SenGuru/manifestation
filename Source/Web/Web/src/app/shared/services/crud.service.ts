import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';
import { Injectable } from '@angular/core';
import { ManifestColumnModel } from 'src/common/models/app-custom-model';
import { NgForm } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class CrudService {

  public constructor() {
    // 
  }

public baseInit(search: BaseSearchCriteria, sortyBy = 'name'): void {
    search.sortBy = sortyBy;
    search.sortAscending = true;
    search.pageSize = 20;
  }

public basePaginationChange(e: BaseSearchCriteria, search: BaseSearchCriteria): void {
    search.page = e.page;
    search.pageSize = e.pageSize;
  }

public baseOnSearch(f: NgForm, search: BaseSearchCriteria, sortyBy = 'name'): void {
    search.sortBy = sortyBy;
    search.sortAscending = true;
    search.page = 1;
  }

public baseOnReset(f: NgForm, search: BaseSearchCriteria, sortyBy = 'name'): void {
    f.resetForm();
    search.keyword = '';
    search.sortBy = sortyBy;
    search.sortAscending = true;
    search.page = 1;
  }

public baseChangeSort(header: ManifestColumnModel, search: BaseSearchCriteria, sortData: { currentSort: string }): void {
    if (!header.sortable) return;

    search.sortBy = header.value ?? '';
    search.sortAscending = !search.sortAscending;
    let nextSort: string | undefined;

    if (sortData.currentSort === 'down') {
      nextSort = 'up';
    } else if (sortData.currentSort === 'up') {
      nextSort = 'down';
    } else if (sortData.currentSort === 'default') {
      nextSort = 'up';
    }
    sortData.currentSort = nextSort ?? '';
    search.page = 1;
  }
}
