import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private _storage = sessionStorage;
  private KEY_USERNAME = "todo_list_username";

  constructor(
    private http: HttpClient,
    private router: Router,
  ) { }

  setLogin(username: string) {
    this._storage.setItem(this.KEY_USERNAME, username);
  }

  clearLogin() {
    this._storage.removeItem(this.KEY_USERNAME);
  }

  // ======================== getter ========================
  get username() {
    return this._storage.getItem(this.KEY_USERNAME);
  }
}