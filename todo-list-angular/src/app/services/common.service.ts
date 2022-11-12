import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment'
import { AuthService } from './auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  private api = environment.WEB_API;
  spinner!: NgxSpinnerService;

  constructor(
    private http: HttpClient,
    private auth: AuthService,
    private toastr: ToastrService,
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
        'Authorization': 'Bearer '+ this.auth.accessToken,
      },
      withCredentials: false,
    }
  }

  // Loading effect
  blockUI() {
    this.spinner.show();
  }

  unBlockUI() {
    this.spinner.hide();
  }

  // Show error message
  toastrError(message?: string) {
    if (!message) 
      message = 'Oops! Something is wrong...';

    this.toastr.error('', message, {
      timeOut: 5000,
      extendedTimeOut: 5000,
      progressBar: true
    });
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