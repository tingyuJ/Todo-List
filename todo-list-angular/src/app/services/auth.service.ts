import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private _storage = sessionStorage;
  private KEY_USERNAME = 'todo_list_username';
  private KEY_TOKEN = 'todo_list_token';

  constructor(
    private http: HttpClient,
    private router: Router,
  ) { }

  setLogin(username: string, token: string) {
    this._storage.setItem(this.KEY_USERNAME, username);
    this._storage.setItem(this.KEY_TOKEN, token);
  }

  clearLogin() {
    this._storage.removeItem(this.KEY_USERNAME);
    this._storage.removeItem(this.KEY_TOKEN);
  }

  // ======================== getter ========================
  get username(): string {
    return this._storage.getItem(this.KEY_USERNAME) ?? '';
  }
  get accessToken(): string {
    return this._storage.getItem(this.KEY_TOKEN) ?? '';
  }
}