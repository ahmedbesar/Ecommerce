import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models/product.model';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [RouterLink, DecimalPipe],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.scss'
})
export class ProductDetailComponent implements OnInit {
  product = signal<Product | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) this.loadProduct(id);
  }

  loadProduct(id: string): void {
    this.loading.set(true);
    this.error.set(null);
    this.productService.getProductById(id).subscribe({
      next: (data) => {
        this.product.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load product details.');
        this.loading.set(false);
        console.error(err);
      }
    });
  }

  get imageUrl(): string {
    const img = this.product()?.imageFile;
    if (img && img.startsWith('http')) return img;
    return 'https://placehold.co/600x400/e2e8f0/94a3b8?text=No+Image';
  }
}
