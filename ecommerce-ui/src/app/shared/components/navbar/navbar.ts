import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { BasketService } from '../../../core/services/basket/basket.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class NavbarComponent {
  authService = inject(AuthService);
  basketService = inject(BasketService);

  get username(): string {
    return this.authService.getCurrentUser()?.username ?? '';
  }

  logout(): void {
    this.authService.logout();
  }

  getBasketCount(): number {
    const basket = this.basketService.getCurrentBasketValue();
    if (!basket) return 0;
    return basket.items.reduce((sum, item) => sum + item.quantity, 0);
  }
}
