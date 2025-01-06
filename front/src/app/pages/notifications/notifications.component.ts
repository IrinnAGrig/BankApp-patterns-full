import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { CardService } from 'src/app/shared/services/card/card.service';
import { INotification, NotificationService } from 'src/app/shared/services/notification.service';
import { RequestsModel } from 'src/app/shared/services/requests/requests.model';
import { RequestsService } from 'src/app/shared/services/requests/requests.service';
import { TransactionService } from 'src/app/shared/services/transaction/transactions.service';
import { TranslationService } from 'src/app/shared/services/translation.service';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {
  requests: RequestsModel[] = [];
  selectedSection: 'transactions' | "requests" = 'transactions';
  notifications: INotification[] = [];
  private subscription: Subscription[] = [];

  constructor(private requestService: RequestsService, 
    private cardsService: CardService,
    private translate: TranslationService,
    private transactionService: TransactionService
  ){
    this.translate.useTranslation('statistics');
  }

  ngOnInit(){
    this.requestService.getRequestsByIdUser().subscribe(res => {
      this.requests = res;
    });
    this.transactionService.getTransactionsNotifications().subscribe(res => {
      this.notifications = res;
    })
  }
  doAccept(request: RequestsModel){
    request.status = 'A';
    this.requestService.updateRequests(request).subscribe( res => {
      if(res){
        request.status = 'A';
        request.closed = true;
      }
    });
  }
  selectSection(section: 'transactions' | "requests"): void {
    this.selectedSection = section;
  }
  doDecline(request: RequestsModel){
    request.status = 'D';
    request.closed = true;
    this.requestService.updateRequests(request).subscribe();
  }
}
