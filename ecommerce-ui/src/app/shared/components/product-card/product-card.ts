import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DecimalPipe, SlicePipe } from '@angular/common';
import { Product } from '../../../core/models/product.model';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [RouterLink, DecimalPipe, SlicePipe],
  templateUrl: './product-card.html',
  styleUrl: './product-card.scss'
})
export class ProductCardComponent {
  @Input({ required: true }) product!: Product;

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
}
