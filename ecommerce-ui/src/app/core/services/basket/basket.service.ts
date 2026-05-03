import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, BehaviorSubject } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { CustomerBasket, BasketItem } from '../../models/basket/basket.model';
import { AuthService } from '../auth.service';

@Injectable({ providedIn: 'root' })
export class BasketService {
  private readonly baseUrl = environment.ocelotGateWayApiUrl; // Basket API goes through Ocelot

  // Local state for the basket so components can react to changes
  private basketSource = new BehaviorSubject<CustomerBasket | null>(null);
  basket$ = this.basketSource.asObservable();

  constructor(private http: HttpClient, private authService: AuthService) { }

  getCurrentBasketValue(): CustomerBasket | null {
    return this.basketSource.value;
  }

  getBasket(): void {
    const user = this.authService.getCurrentUser();

    if (!user) return;
    this.http.get<CustomerBasket>(`${this.baseUrl}/Basket/${user.username}`).subscribe({
      next: basket => {
        this.basketSource.next(basket);
      },
      error: (err) => {
        console.error(err);
        this.basketSource.next(null);
      }
    });
  }

  setBasket(basket: CustomerBasket): Observable<CustomerBasket> {
    return this.http.put<CustomerBasket>(`${this.baseUrl}/Basket`, basket).pipe(
      tap(response => {
        this.basketSource.next(response);
      })
    );
  }

  deleteBasket(): Observable<void> {
    const user = this.authService.getCurrentUser();
    if (!user) throw new Error('User not logged in');

    return this.http.delete<void>(`${this.baseUrl}/Basket/${user.username}`).pipe(
      tap(() => this.basketSource.next(null))
    );
  }

  addItemToBasket(item: BasketItem, quantity = 1): void {
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    const itemIndex = basket.items.findIndex(i => i.productId === item.productId);

    if (itemIndex === -1) {
      item.quantity = quantity;
      basket.items.push(item);
    } else {
      basket.items[itemIndex].quantity += quantity;
    }

    this.setBasket(basket).subscribe();
  }

  removeItemFromBasket(productId: string, quantity = 1): void {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;

    const item = basket.items.find(i => i.productId === productId);
    if (item) {
      item.quantity -= quantity;
      if (item.quantity <= 0) {
        basket.items = basket.items.filter(i => i.productId !== productId);
      }

      if (basket.items.length > 0) {
        this.setBasket(basket).subscribe();
      } else {
        this.deleteBasket().subscribe();
      }
    }
  }

  private createBasket(): CustomerBasket {
    const user = this.authService.getCurrentUser();
    if (!user) throw new Error('User not logged in');

    const basket: CustomerBasket = {
      userName: user.username,
      items: []
    };
    return basket;
  }
}
