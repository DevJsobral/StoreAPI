import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Category {
  categoryId: number;
  name: string;
  imageURL: string;
  registerDate?: string;
}

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {
  private apiUrl = 'http://localhost:5216/api/Categories';

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
      // Authorization: 'Bearer ' + yourToken  // se precisar autenticação
    })
  };

  constructor(private http: HttpClient) { }

  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/GetAll`);
  }

  getById(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.apiUrl}/${id}`);
  }

  create(category: Category): Observable<Category> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token || ''}`
    });
    return this.http.post<Category>(`${this.apiUrl}/Post`, category, { headers });
  }

  update(id: number, category: Category): Observable<Category> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token || ''}`
    });
    return this.http.put<Category>(`${this.apiUrl}/${id}`, category,  { headers });
  }

  delete(id: number): Observable<void> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token || ''}`
    });
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers });
  }
}
