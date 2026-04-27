import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/product.model';
import { ProductCardComponent } from '../../../shared/components/product-card/product-card';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { PaginationState } from '../../../shared/state/pagination.state';
import { createViewState } from '../../../shared/state/view.state';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [FormsModule, ProductCardComponent, PaginationComponent],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss'
})
export class ProductListComponent implements OnInit {
  viewState = createViewState<Product[]>([]);
  pagination = new PaginationState();
  searchQuery = signal<string>('');

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.viewState.setLoading();

    this.productService.getProducts({
      pageIndex: this.pagination.pageIndex(),
      pageSize: this.pagination.pageSize(),
      search: this.searchQuery() || undefined
    }).subscribe({
      next: (res) => {
        this.viewState.setSuccess(res.data || []);
        this.pagination.update(res.pageIndex || 1, res.totalCount || 0);
      },
      error: (err) => {
        this.viewState.setError('Failed to load products. Please ensure the API is running.');
        console.error(err);
      }
    });
  }

  onSearch(): void {
    this.pagination.pageIndex.set(1);
    this.loadProducts();
  }

  onPageChange(newPageIndex: number): void {
    if (this.pagination.setPage(newPageIndex)) {
      this.loadProducts();
    }
  }
}
