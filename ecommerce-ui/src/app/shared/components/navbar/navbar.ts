import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';
import { BasketService } from '../../../core/services/basket/basket.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class NavbarComponent {
  authService = inject(AuthService);
  basketService = inject(BasketService);

  basketCount$ = this.basketService.basket$.pipe(
    map(basket => {
      if (!basket) return 0;
      return basket.items.reduce((sum, item) => sum + item.quantity, 0);
    })
  );

  get username(): string {
    return this.authService.getCurrentUser()?.username ?? '';
  }

  logout(): void {
    this.basketService.clearLocalBasket();
    this.authService.logout();
  }
}
