import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/product.model';
import { createViewState } from '../../../shared/state/view.state';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [RouterLink, DecimalPipe],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.scss'
})
export class ProductDetailComponent implements OnInit {
  viewState = createViewState<Product | null>(null);

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) this.loadProduct(id);
  }

  loadProduct(id: string): void {
    this.viewState.setLoading();
    this.productService.getProductById(id).subscribe({
      next: (data) => {
        this.viewState.setSuccess(data);
      },
      error: (err) => {
        this.viewState.setError('Failed to load product details.');
        console.error(err);
      }
    });
  }

  get imageUrl(): string {
    const img = this.viewState.data()?.imageFile;
    if (img && img.startsWith('http')) return img;
    return 'https://placehold.co/600x400/e2e8f0/94a3b8?text=No+Image';
  }
}
