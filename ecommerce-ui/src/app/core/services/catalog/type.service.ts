import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ProductType } from '../../models/product.model';

@Injectable({ providedIn: 'root' })
export class TypeService {
  private readonly baseUrl = environment.catalogApiUrl;

  constructor(private http: HttpClient) {}

  getTypes(): Observable<ProductType[]> {
    return this.http.get<ProductType[]>(`${this.baseUrl}/Types`);
  }
}
