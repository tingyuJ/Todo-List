import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment'

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  private api = environment.WEB_API;

  constructor(
    private http: HttpClient,
  ) { }

  get(controller: string, action: string) {
    return this.http.get(this.api + "/" + controller + "/" + action, this.getHttpOption())
  }

  post(controller: string, action: string, data: any) {
    return this.http.post(this.api + "/" + controller + "/" + action, data, this.getHttpOption())
  }

  getHttpOption() {
    return {
      headers: {
        'Content-Type': 'application/json',
      },
      withCredentials: false,
    }
  }
}

export interface Response {
  contentType: any;
  serializerSettings: any;
  statusCode: any;
  value: {
    data: any;
  }
}