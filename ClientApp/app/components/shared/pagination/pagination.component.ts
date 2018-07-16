import { Component, OnChanges, Input, Output, EventEmitter } from '@angular/core';


@Component({
  selector: 'pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnChanges {
  @Input('total-items') totalItems: any;
  @Input('page-size') pageSize = 10;
  @Output('page-changed') pageChanged = new EventEmitter();

  pages: any[] = [];
  currentPage = 1;

  ngOnChanges() {
    this.currentPage = 1;
    // count how many pages should be display depends on the total items and pageSize
    var pagesCount = Math.ceil(this.totalItems / this.pageSize);
    this.pages = [];
    // push the page number to pages array to display on the view
    for (var i = 1; i <= pagesCount; i++) {
      this.pages.push(i);
    }

  }

  changePage(page: any) {
    this.currentPage = page;
    this.pageChanged.emit(page);
  }

  previous() {
    if (this.currentPage == 1)
      return;

    this.currentPage--;
    this.pageChanged.emit(this.currentPage);
  }

  next() {
    if (this.currentPage == this.pages.length)
      return;

    this.currentPage++;
    this.pageChanged.emit(this.currentPage);
  }

}
