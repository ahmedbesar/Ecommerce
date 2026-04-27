import { Component, OnInit, signal, computed } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/product.model';
import { ProductCardComponent } from '../../../shared/components/product-card/product-card';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [FormsModule, ProductCardComponent],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss'
})
export class ProductListComponent implements OnInit {
  products = signal<Product[]>([]);
  loading = signal<boolean>(true);
  error = signal<string | null>(null);
  searchQuery = signal<string>('');

  // Pagination
  pageIndex = signal<number>(1);
  pageSize = signal<number>(10);
  totalCount = signal<number>(0);

  totalPages = computed(() => Math.ceil(this.totalCount() / this.pageSize()) || 1);
  pages = computed(() => Array.from({ length: this.totalPages() }, (_, i) => i + 1));

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading.set(true);
    this.error.set(null);
    this.productService.getProducts({
      pageIndex: this.pageIndex(),
      pageSize: this.pageSize(),
      search: this.searchQuery() || undefined
    }).subscribe({
      next: (res) => {
        debugger;
        this.products.set(res.data || []);
        this.totalCount.set(res.totalCount || 0);
        this.pageIndex.set(res.pageIndex || 1);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load products. Please ensure the API is running.');
        this.loading.set(false);
        console.error(err);
      }
    });
  }

  onSearch(): void {
    this.pageIndex.set(1);
    this.loadProducts();
  }

  onPageChange(newPageIndex: number): void {
    if (newPageIndex >= 1 && newPageIndex <= this.totalPages()) {
      this.pageIndex.set(newPageIndex);
      this.loadProducts();
    }
  }
}
