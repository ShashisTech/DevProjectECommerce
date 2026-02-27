import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient) { }

  login(payload: any) {
    return this.http.post(environment.apiBase + '/api/auth/login', payload);
  }

  register(payload: any) {
    return this.http.post(environment.apiBase + '/api/auth/register', payload);
  }
}
