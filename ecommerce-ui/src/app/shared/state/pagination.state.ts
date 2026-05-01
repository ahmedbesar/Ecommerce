import { signal, computed } from '@angular/core';

export class PaginationState {
  readonly pageIndex = signal(1);
  readonly pageSize = signal(10);
  readonly totalCount = signal(0);

  readonly totalPages = computed(() => Math.ceil(this.totalCount() / this.pageSize()) || 1);

  setPageIndex(pageIndex: number): void {
    this.pageIndex.set(pageIndex);
  }

  setPageSize(pageSize: number): void {
    this.pageSize.set(pageSize);
  }

  setTotalCount(totalCount: number): void {
    this.totalCount.set(totalCount);
  }


}
