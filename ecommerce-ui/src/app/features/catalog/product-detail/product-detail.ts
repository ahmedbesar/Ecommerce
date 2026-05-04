import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { DecimalPipe, CurrencyPipe } from '@angular/common';
import { ProductService } from '../../../core/services/catalog/product.service';
import { Product } from '../../../core/models/catalog/product.model';
import { createViewState } from '../../../shared/state/view.state';
import { BasketService } from '../../../core/services/basket/basket.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [RouterLink, DecimalPipe, CurrencyPipe],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.scss'
})
export class ProductDetailComponent implements OnInit {
  viewState = createViewState<Product | null>(null);
  quantity = signal(1);

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private basketService: BasketService,
    private authService: AuthService,
    private router: Router
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
    if (img) {
      if (img.startsWith('http') || img.startsWith('/')) {
        return img;
      }
      return '/' + img;
    }
    return 'https://placehold.co/600x400/e2e8f0/94a3b8?text=No+Image';
  }

  handleImageError(event: Event): void {
    const imgElement = event.target as HTMLImageElement;
    imgElement.src = 'https://placehold.co/600x400/e2e8f0/94a3b8?text=No+Image';
  }

  incrementQuantity(): void {
    this.quantity.update(q => q + 1);
  }

  decrementQuantity(): void {
    if (this.quantity() > 1) {
      this.quantity.update(q => q - 1);
    }
  }

  addToCart(): void {
    const product = this.viewState.data();
    if (!product) return;

    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.basketService.addItemToBasket({
      productId: product.id,
      productName: product.name,
      price: product.discountedPrice || product.price,
      quantity: this.quantity(),
      imageFile: product.imageFile
    }, this.quantity());
  }
}
