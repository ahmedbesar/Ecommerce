import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { PaginatedOrders, OrderParams, Order } from '../../models/orders/order.model';

@Injectable({ providedIn: 'root' })
export class OrdersService {
  private readonly baseUrl = environment.ocelotGateWayApiUrl;

  constructor(private http: HttpClient) { }

  getOrders(params: OrderParams = { pageIndex: 1, pageSize: 10 }): Observable<PaginatedOrders> {
    let httpParams = new HttpParams();
    if (params.pageIndex != null) httpParams = httpParams.set('pageIndex', params.pageIndex);
    if (params.pageSize != null) httpParams = httpParams.set('pageSize', params.pageSize);
    if (params.sort) httpParams = httpParams.set('sort', params.sort);
    if (params.userName) httpParams = httpParams.set('userName', params.userName);

    return this.http.get<PaginatedOrders>(`${this.baseUrl}/api/Orders`, { params: httpParams });
  }

  getOrdersByUserName(userName: string): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.baseUrl}/api/Orders/${userName}`);
  }

  createOrder(order: Order): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}/api/Orders`, order);
  }

  updateOrder(order: Order): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/api/Orders`, order);
  }

  deleteOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/api/Orders/${id}`);
  }
}
