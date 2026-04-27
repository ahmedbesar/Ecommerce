import { Component, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  username = '';
  password = '';
  loading = signal(false);
  error = signal<string | null>(null);
  showPassword = signal(false);

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    if (!this.username || !this.password) {
      this.error.set('Please enter your username and password.');
      return;
    }

    this.loading.set(true);
    this.error.set(null);

    this.authService.login(this.username, this.password).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/products']);
      },
      error: (err: Error) => {
        this.loading.set(false);
        this.error.set(err.message);
      }
    });
  }

  togglePassword(): void {
    this.showPassword.update((v) => !v);
  }
}
