import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface OrderItemDetails {
  productName: string;
  price: number;
  quantity: number;
}

export interface OrderResponseWithDetails {
  id: number;
  items: OrderItemDetails[];
  total: number;
  createdAt: string;
}

export interface OrderItem {
  id: number;
  quantity: number;
}

export interface OrderItemRequest {
  productId: number;
  quantity: number;
}

export interface OrderRequest {
  items: OrderItemRequest[];
  total?: number;
}

export interface OrderResponse {
  id: number;
  items: OrderItem[];
  total: number;
  orderDate?: string;
}

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  private apiUrl = 'https://storeapi-javaapp-zoy2d.ondigitalocean.app/api/Orders';

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  getAll(): Observable<OrderResponseWithDetails[]> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token || ''}`
    });

    return this.http.get<OrderResponseWithDetails[]>(`${this.apiUrl}/GetAll`, { headers });
  }

  getById(id: number): Observable<OrderResponse> {
    return this.http.get<OrderResponse>(`${this.apiUrl}/${id}`, this.httpOptions);
  }

  create(order: OrderRequest): Observable<OrderResponse> {
    return this.http.post<OrderResponse>(`${this.apiUrl}/Post`, order, this.httpOptions);
  }

  delete(id: number): Observable<void> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token || ''}`
    });

    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers });
  }
}
