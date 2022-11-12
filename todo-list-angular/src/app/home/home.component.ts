import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { CommonService, Response } from '../services/common.service';

interface ListItem {
  id: string;
  checked: boolean | null;
  text: string;
  username: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(
    private auth: AuthService,
    private common: CommonService,
  ) { }

  isLoggedIn: boolean = false;
  username: string = "";
  listItems: ListItem[] = [];
  currentRow: number = -1;

  ngOnInit(): void {
    if (this.auth.username) {
      this.isLoggedIn = true;
      this.username = this.auth.username;
      this.getList();
    }
  }

  getList() {
    this.common.blockUI();
    this.common.get('List', 'GetList/' + this.username).subscribe((result) => {
      var res = <Response>result;
      this.listItems = res.value.data;

      const emptyItem: ListItem = {
        id: "",
        username: this.username,
        checked: null,
        text: "",
      };
      this.listItems.push(emptyItem);
    }, error => {
      console.error(error);
      this.common.unBlockUI();
      this.common.toastrError();
    }, () => {
      this.common.unBlockUI();
    });
  }

  editItem(i: number) {
    //do it after two way binding is done.(Angular zone)
    setTimeout(() => {

      //not doing anything if text is blank
      if (!this.listItems[i].text) {
        this.getList();
        return;
      }

      //insert a new one
      if (this.listItems[i].checked == null) {
        this.listItems[i].checked = false;

        this.common.post('List', 'CreateListItem', this.listItems[i]).subscribe(() => {
          this.getList();
        }, error => {
          console.error(error);
          this.common.toastrError('Create failed...');
        });
        return;
      }

      //update
      this.common.post('List', 'UpdateListItem', this.listItems[i]).subscribe(() => {
        this.getList();
      }, error => {
        console.error(error);
        this.common.toastrError('Update failed...');
      });

    });
  }

  pointedRow(i: number) {
    this.currentRow = i;
  }

  leaveRow() {
    this.currentRow = -1;
  }

  delete(i: number) {
    this.common.post('List', 'DeleteListItem', this.listItems[i]).subscribe(() => {
      this.getList();
    }, error => {
      console.error(error);
      this.common.toastrError('Delete failed...');
    });

  }

  hitEnter(e: Event) {
    var ele = e.target as HTMLElement;
    ele.blur();
  }
  
}

