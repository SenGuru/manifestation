import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';

import { BaseSearchCriteria } from 'src/common/framework/common/models/base-search-criteria.model';

@Component({
    selector: 'app-pagination',
    templateUrl: './pagination.component.html',
    styleUrls: ['./pagination.component.css'],
    standalone: false
})
export class PaginationComponent {

  @Input() public currentPage = 1;
  @Input() public perPage = 10;
  @Input() public showPager = true;
  @Input() public showPageDataOnly = false;
  @Input() public pageOf = true;
  @Input() public totalItems = 0;
  @Input() public searchScreenScroll!: ElementRef;
  @Output() public pageOptionChanged = new EventEmitter();
  public constructor() {
    // 
  }

  public get totalPages(): number {
    return Math.ceil(this.totalItems / this.perPage);
  }

  public get currentStartData(): number {
    return (this.currentPage * this.perPage - (this.perPage - 1));
  }

  public get currentEndData(): number {
    return (this.currentPage * this.perPage) > this.totalItems ? this.totalItems : (this.currentPage * this.perPage);
  }

  public onChangePage(e: unknown): void {
    const se = new BaseSearchCriteria();
    se.page = this.currentPage;
    se.pageSize = this.perPage;
    this.pageOptionChanged.emit(se);
    this.scrollToTop();
  }

  public scrollToTop(): void {
    const element = document.getElementsByClassName('table-responsive')[0];
    if (element) {
      element.scroll({
        top: 0,
        behavior: 'smooth',
      })
    }
  }

  public refresh(): void {
    this.currentPage = 1;
    this.perPage = 4;
  }

}
