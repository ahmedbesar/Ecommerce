import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class NavbarComponent {
  authService = inject(AuthService);

  get username(): string {
    return this.authService.getCurrentUser()?.username ?? '';
  }

  logout(): void {
    this.authService.logout();
  }
}
