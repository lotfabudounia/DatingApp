import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  protected accountservice = inject(AccountService)
  protected creds:any={}
  

  login() {
  this.accountservice.login(this.creds).subscribe({
    next: result =>
      {
        console.log(result);
        this.creds={};
      }, 
    error: error => alert(error.massage)    
  })
  }

  logout(){
    this.accountservice.logout();
  }
}
