import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../environments/environment';

export interface EditAccountDto {
  username: string;
  email: string;
}

@Injectable({ providedIn: 'root' })
export class AccountService {
  private readonly apiUrl = `${environment.apiBaseUrl}/account`;

  constructor(private http: HttpClient) {}

  getProfile() {
    return this.http.get<any>(`${this.apiUrl}/me`);
  }

  updateAccount(data: EditAccountDto) {
    return this.http.put(`${this.apiUrl}/update`, data);
  }

  changePassword(currentPassword: string, newPassword: string) {
    return this.http.put(`${this.apiUrl}/change-password`, {
      currentPassword,
      newPassword
    });
  }
}