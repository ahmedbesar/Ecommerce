import { Component, Input } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { DecimalPipe, SlicePipe, CurrencyPipe } from '@angular/common';
import { Product } from '../../../core/models/catalog/product.model';
import { BasketService } from '../../../core/services/basket/basket.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [RouterLink, DecimalPipe, SlicePipe, CurrencyPipe],
  templateUrl: './product-card.html',
  styleUrl: './product-card.scss'
})
export class ProductCardComponent {
  @Input({ required: true }) product!: Product;

  constructor(
    private basketService: BasketService,
    private authService: AuthService,
    private router: Router
  ) {}

  get imageUrl(): string {
    if (this.product.imageFile) {
      if (this.product.imageFile.startsWith('http') || this.product.imageFile.startsWith('/')) {
        return this.product.imageFile;
      }
      return '/' + this.product.imageFile;
    }
    return 'https://placehold.co/400x300/e2e8f0/94a3b8?text=No+Image';
  }

  handleImageError(event: Event): void {
    const imgElement = event.target as HTMLImageElement;
    imgElement.src = 'https://placehold.co/400x300/e2e8f0/94a3b8?text=No+Image';
  }

  addToCart(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.basketService.addItemToBasket({
      productId: this.product.id,
      productName: this.product.name,
      price: this.product.discountedPrice || this.product.price,
      quantity: 1,
      imageFile: this.product.imageFile
    });
  }
}
