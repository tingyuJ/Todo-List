import { Component, EnvironmentInjector, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { CommonService, Response } from '../services/common.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(
    private router: Router,
    private common: CommonService,
    private auth: AuthService,
  ) { }

  @ViewChild('loginForm') loginForm!: NgForm;
  
  usernameRequired: boolean = false;
  passwordRequired: boolean = false;
  passwordError: boolean = false;

  ngOnInit(): void {
  }

  submit() {
    if (this.loginForm.invalid) {
      this.usernameRequired = !this.loginForm.value.username;
      this.passwordRequired = !this.loginForm.value.password;
      return;
    }
    this.usernameRequired = false;
    this.passwordRequired = false;

    this.common.blockUI();
    this.common.post('User', 'LogInOrSignUp', this.loginForm.value).subscribe(result => {
      var res = <Response>result;
      const username = res.value.data.userName;
      
      if (!username) {
        this.passwordError = true;
        this.common.unBlockUI();
        return;
      }
      this.auth.setLogin(username, res.value.data.token);
      // this.router.navigate(['/']);
      window.location.assign('/');
    }, error => {
      console.error(error);
      this.common.unBlockUI();
      this.common.toastrError();
    }, () => {
      //this.common.unBlockUI();  //=> unblock after home list loaded.
    });
  }
}