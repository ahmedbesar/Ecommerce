import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginatedResult, Product } from '../models/product.model';

export interface ProductQueryParams {
  pageIndex?: number;
  pageSize?: number;
  sort?: string;
  brandId?: string;
  typeId?: string;
  search?: string;
}

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly baseUrl = `${environment.catalogApiUrl}/Products`;

  constructor(private http: HttpClient) {}

  getProducts(params: ProductQueryParams = {}): Observable<PaginatedResult<Product>> {
    let httpParams = new HttpParams();
    if (params.pageIndex != null) httpParams = httpParams.set('pageIndex', params.pageIndex);
    if (params.pageSize != null) httpParams = httpParams.set('pageSize', params.pageSize);
    if (params.sort) httpParams = httpParams.set('sort', params.sort);
    if (params.brandId) httpParams = httpParams.set('brandId', params.brandId);
    if (params.typeId) httpParams = httpParams.set('typeId', params.typeId);
    if (params.search) httpParams = httpParams.set('search', params.search);

    return this.http.get<PaginatedResult<Product>>(this.baseUrl, { params: httpParams });
  }

  getProductById(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}/${id}`);
  }
}
