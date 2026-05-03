import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ProductBrand } from '../../models/catalog/product.model';

@Injectable({ providedIn: 'root' })
export class BrandService {
  private readonly baseUrl = environment.ocelotGateWayApiUrl;

  constructor(private http: HttpClient) { }

  getBrands(): Observable<ProductBrand[]> {
    return this.http.get<ProductBrand[]>(`${this.baseUrl}/Brands`);
  }
}
