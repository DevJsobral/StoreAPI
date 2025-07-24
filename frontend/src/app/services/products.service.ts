import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Product {
  productId: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  categoryId: number;
  imageURL: string;
  registerDate?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  private apiUrl = 'https://storeapi-0ysq.onrender.com/api/Products';

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  getAll(name?: string, categoryId?: number): Observable<Product[]> {
    let params = new HttpParams();
    if (name) params = params.set('name', name);
    if (categoryId) params = params.set('categoryId', categoryId.toString());

    return this.http.get<Product[]>(`${this.apiUrl}/GetAll`, { params });
  }

  getById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/Get/${id}`);
  }

  create(product: Product): Observable<Product> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token || ''}`
    });

    return this.http.post<Product>(`${this.apiUrl}/Post`, product, { headers });
  }

  update(id: number, product: Product): Observable<Product> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token || ''}`
    });

    return this.http.put<Product>(`${this.apiUrl}/${id}`, product, { headers });
  }

  patchPriceAndStock(id: number, price: number, stock: number): Observable<Product> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token || ''}`
    });

    const patchData = { price, stock };
    return this.http.patch<Product>(`${this.apiUrl}/${id}/UpdatePriceAndStock`, patchData, {headers});
  }

  delete(id: number): Observable<void> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token || ''}`
    });

    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers });
  }
}
