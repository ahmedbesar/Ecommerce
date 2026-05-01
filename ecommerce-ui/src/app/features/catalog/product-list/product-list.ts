import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../../core/services/catalog/product.service';
import { BrandService } from '../../../core/services/catalog/brand.service';
import { TypeService } from '../../../core/services/catalog/type.service';
import { Product, ProductBrand, ProductType } from '../../../core/models/product.model';
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

  // Local state for filters
  searchQuery = signal<string>('');
  selectedBrandId = signal<string | null>(null);
  selectedTypeId = signal<string | null>(null);
  selectedSort = signal<string | null>(null);

  brands = signal<ProductBrand[]>([]);
  types = signal<ProductType[]>([]);

  sortOptions = [
    { value: 'name', label: 'Name: A-Z' },
    { value: 'priceAsc', label: 'Price: Low to High' },
    { value: 'priceDesc', label: 'Price: High to Low' },
  ];

  constructor(
    private productService: ProductService,
    private brandService: BrandService,
    private typeService: TypeService,
  ) { }

  ngOnInit(): void {
    this.loadFilterData();
    this.loadProducts();
  }

  loadFilterData(): void {
    this.brandService.getBrands().subscribe(res => this.brands.set(res));
    this.typeService.getTypes().subscribe(res => this.types.set(res));
  }

  loadProducts(): void {
    this.viewState.setLoading();

    this.productService.getProducts({
      pageIndex: this.pagination.pageIndex(),
      pageSize: this.pagination.pageSize(),
      search: this.searchQuery() || undefined,
      brandId: this.selectedBrandId() || undefined,
      typeId: this.selectedTypeId() || undefined,
      sort: this.selectedSort() || undefined,
    }).subscribe({
      next: (res) => {
        this.viewState.setSuccess(res.data || []);
        this.pagination.setPageIndex(res.pageIndex || 1);
        this.pagination.setTotalCount(res.totalCount || 0);
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
    if (newPageIndex !== this.pagination.pageIndex()) {
      this.pagination.setPageIndex(newPageIndex);
      this.loadProducts();
    }
  }

  onBrandFilterApply(brandId: string | null): void {
    this.selectedBrandId.set(brandId);
    this.pagination.pageIndex.set(1);
    this.loadProducts();
  }

  onTypeFilterApply(typeId: string | null): void {
    this.selectedTypeId.set(typeId);
    this.pagination.pageIndex.set(1);
    this.loadProducts();
  }

  onSortFilterApply(event: any): void {
    this.selectedSort.set(event.target.value);
    this.pagination.pageIndex.set(1);
    this.loadProducts();
  }

  resetFilters(): void {
    this.selectedBrandId.set(null);
    this.selectedTypeId.set(null);
    this.selectedSort.set(null);
    this.searchQuery.set('');
    this.pagination.pageIndex.set(1);
    this.loadProducts();
  }
}
