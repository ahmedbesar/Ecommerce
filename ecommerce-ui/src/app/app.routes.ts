import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  {
    path: 'home',
    loadComponent: () => import('./features/home/home').then((m) => m.HomeComponent)
  },
  {
    path: 'products',
    loadComponent: () => import('./features/catalog/product-list/product-list').then((m) => m.ProductListComponent)
  },
  {
    path: 'products/:id',
    loadComponent: () => import('./features/catalog/product-detail/product-detail').then((m) => m.ProductDetailComponent)
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then((m) => m.LoginComponent)
  },
  {
    path: 'basket',
    canActivate: [authGuard],
    loadComponent: () => import('./features/basket/basket.component').then((m) => m.BasketComponent)
  },
  { path: '**', redirectTo: '/home' }
];
