import { Component } from '@angular/core';
import { CommonService } from 'src/app/services/common.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  title = "todo-list-angular";

  ngx: any = {
    bgcolor: 'rgba(57,57,57,0.8)',
    color: '#ffffff',
    size: 'medium', // small, default, medium, large
    type: 'square-jelly-box', // https://github.danielcardoso.net/load-awesome/animations.html
  };
  
  constructor( 
    private common: CommonService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.common.spinner = this.spinner;
  }
}
