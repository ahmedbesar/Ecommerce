import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrdersService } from '../../core/services/orders/orders.service';
import { AuthService } from '../../core/services/auth.service';
import { Order } from '../../core/models/orders/order.model';
import { PaginationComponent } from '../../shared/components/pagination/pagination.component';
import { PaginationState } from '../../shared/state/pagination.state';
import { createViewState } from '../../shared/state/view.state';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, RouterLink, PaginationComponent],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent implements OnInit {
  viewState = createViewState<Order[]>([]);
  pagination = new PaginationState();

  constructor(private ordersService: OrdersService, private authService: AuthService) { }

  ngOnInit(): void {
    this.pagination.setPageSize(1); // Set to 1 to test pagination easily
    this.fetchOrders();
  }

  fetchOrders(): void {
    this.viewState.setLoading();

    const user = this.authService.getCurrentUser();

    this.ordersService.getOrders({
      pageIndex: this.pagination.pageIndex(),
      pageSize: this.pagination.pageSize(),
      userName: user?.username
    }).subscribe({
      next: (res) => {
        this.viewState.setSuccess(res.data || []);
        this.pagination.setPageIndex(res.pageIndex || 1);
        this.pagination.setPageSize(res.pageSize || 10);
        this.pagination.setTotalCount(res.totalCount || 0);
      },
      error: (err) => {
        console.error('Error fetching orders:', err);
        this.viewState.setError('Could not load your orders. Please try again later.');
      }
    });
  }

  onPageChange(newPageIndex: number): void {
    if (newPageIndex !== this.pagination.pageIndex()) {
      this.pagination.setPageIndex(newPageIndex);
      this.fetchOrders();
    }
  }

  getPaymentMethodName(method: number): string {
    switch (method) {
      case 0: return 'Credit Card';
      case 1: return 'Debit Card';
      case 2: return 'PayPal';
      default: return 'Unknown';
    }
  }
}
