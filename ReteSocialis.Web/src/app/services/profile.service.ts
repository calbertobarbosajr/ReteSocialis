import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ProfileService {
  private apiUrl = 'https://localhost:5001/api/profile'; // ajuste conforme seu backend

  constructor(private http: HttpClient) {}

  /** Envia o avatar recortado para o backend */
  uploadAvatar(file: Blob): Observable<any> {
    const formData = new FormData();
    formData.append('avatar', file, 'avatar.png');
    return this.http.post(`${this.apiUrl}/avatar`, formData);
  }

  /** Busca perfil do usuário logado */
  getMyProfile(): Observable<any> {
    return this.http.get(`${this.apiUrl}/me`);
  }
}
