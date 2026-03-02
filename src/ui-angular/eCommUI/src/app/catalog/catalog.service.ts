import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  private baseUrl = environment.catalogApiUrl;

  constructor(private http: HttpClient) {}

  getCategories(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/categories`);
  }

  getProducts(categoryId?: number): Observable<any[]> {
    const url = categoryId ? `${this.baseUrl}/products?categoryId=${categoryId}` : `${this.baseUrl}/products`;
    return this.http.get<any[]>(url);
  }

  getProductById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/products/${id}`);
  }

  getAllProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Products`);
  }
}
