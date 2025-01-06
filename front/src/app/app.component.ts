import { Component } from '@angular/core';
import { UserService } from './shared/services/user/user.service';
import { NotificationService } from './shared/services/notification.service';
import { TransactionService } from './shared/services/transaction/transactions.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  darkMode = false;
  constructor(private userService: UserService, private notificationService: NotificationService, 
    private transactionService: TransactionService){
     
    this.userService.autoLogin(); 
    this.transactionService.getTransactionsNotifications().subscribe(res => {
        const unreadNotifications = res.filter(notification => !notification.isRead);
        unreadNotifications.forEach(el => {
          this.notificationService.updateNotifications(el);
        })
      })
    this.transactionService.getNotificationsNumber().subscribe(res => {
      this.notificationService.updateNotificationsNumber(res);
    })
    this.darkMode = this.userService.darkMode;
    if (this.darkMode) {
      document.body.classList.add('dark-mode');
      document.body.classList.remove('light-mode');
  } else {
      document.body.classList.add('light-mode');
      document.body.classList.remove('dark-mode');
  }
  }
}
