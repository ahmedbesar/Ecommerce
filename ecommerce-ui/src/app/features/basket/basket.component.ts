import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BasketService } from '../../core/services/basket/basket.service';
import { BasketItem, CustomerBasket } from '../../core/models/basket/basket.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-basket',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss'
})
export class BasketComponent implements OnInit {
  basket$: Observable<CustomerBasket | null>;

  constructor(public basketService: BasketService) {
    this.basket$ = this.basketService.basket$;
  }

  ngOnInit(): void {
    this.basketService.getBasket();
  }

  incrementQuantity(item: BasketItem): void {
    this.basketService.addItemToBasket(item, 1);
  }

  decrementQuantity(item: BasketItem): void {
    this.basketService.removeItemFromBasket(item.productId, 1);
  }

  removeItem(item: BasketItem): void {
    this.basketService.removeItemFromBasket(item.productId, item.quantity);
  }

  getBasketTotal(basket: CustomerBasket): number {
    return basket.items.reduce((total: number, item: BasketItem) => total + (item.price * item.quantity), 0);
  }
}
