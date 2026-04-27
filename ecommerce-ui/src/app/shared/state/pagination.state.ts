import { signal, computed } from '@angular/core';

export class PaginationState {
  readonly pageIndex = signal(1);
  readonly pageSize = signal(8);
  readonly totalCount = signal(0);

  readonly totalPages = computed(() => Math.ceil(this.totalCount() / this.pageSize()) || 1);

  update(pageIndex: number, totalCount: number, pageSize?: number): void {
    this.pageIndex.set(pageIndex);
    this.totalCount.set(totalCount);
    if (pageSize) {
      this.pageSize.set(pageSize);
    }
  }

  setPage(page: number): boolean {
    if (page >= 1 && page <= this.totalPages() && page !== this.pageIndex()) {
      this.pageIndex.set(page);
      return true;
    }
    return false;
  }
}
