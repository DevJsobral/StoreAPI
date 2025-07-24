import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ImageUploadService {
  private apiKey = 'a5e6e1da6cf1299d5ec892928bcffe33';
  private uploadUrl = `https://api.imgbb.com/1/upload?key=${this.apiKey}`;

  constructor(private http: HttpClient) {}

  uploadImage(imageFile: File): Observable<string> {
    const formData = new FormData();
    formData.append('image', imageFile);

    return this.http.post<any>(this.uploadUrl, formData).pipe(
      map(response => response.data.url as string)
    );
  }
}
