import { Component, OnDestroy } from '@angular/core';

import { TranslationService } from '../../services/translation.service';
import { RequestsService } from '../../services/requests/requests.service';
import { INotification, NotificationService } from '../../services/notification.service';
import { Subscription } from 'rxjs';
import { TransactionService } from '../../services/transaction/transactions.service';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent implements OnDestroy{
  dat = '';
  notifications = 0;
  private subscription: Subscription[] = [];

  constructor(private translate: TranslationService,
    private notificationService: NotificationService
  ){
    this.translate.useTranslation('home');

    this.subscription.push(
      this.notificationService.notificationsNumber$.subscribe(res =>{
        this.notifications = res;
      }    
    ));
  }
  ngOnDestroy(): void {
    this.subscription.forEach(sub => {
      sub.unsubscribe();
    })
  }
}
