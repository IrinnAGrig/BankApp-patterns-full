import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { CardService } from 'src/app/shared/services/card/card.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { RequestsService } from 'src/app/shared/services/requests/requests.service';
import { TransactionShow } from 'src/app/shared/services/transaction/transactions.model';
import { TransactionService } from 'src/app/shared/services/transaction/transactions.service';
import { TranslationService } from 'src/app/shared/services/translation.service';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css']
})
export class StatisticsComponent {
  showChart = false;
  balance = 0;
  transactions: TransactionShow [] = [];
  chartTransactions: TransactionShow [] = [];
  chartPieTransactions: TransactionShow [] = [];
  notifications = 0;
  private subscription: Subscription[] = [];

  constructor(private translate: TranslationService, 
    private cardService: CardService,
    private transactionService: TransactionService,
    private router: Router, 
    private requestsService: RequestsService,
   private notificationService: NotificationService){
    this.subscription.push(
      this.notificationService.notificationsNumber$.subscribe(res =>{
        this.notifications = res;
      }    
    ));
    this.translate.useTranslation('statistics');
    this.cardService.getCardsByIdUser().subscribe(res => {
      res.forEach(el => this.balance += el.balance)
    })
    this.transactionService.getJustPartTransactions(5).subscribe(trans => {
      this.transactions = trans;
    })
    this.transactionService.getTransactionsBuy().subscribe(trans => {
      this.chartTransactions = trans;
    })
    this.requestsService.getNumberOpened().subscribe(res => this.notifications += res);
  }

  goToTransactions(){
    this.router.navigate(['/transaction-history']);
  }
  clodePieChart(mode: boolean){
    this.showChart = mode;
  }
  openPieChart(month: number | null){
    if(typeof month == 'number'){
      this.showChart = true;
      this.transactionService.getTransactionsBuy().subscribe(res => {
        this.chartPieTransactions = this.filterTransactionsByMonth(month, res);
      })
    }
  }
  goToLocation(){
    this.router.navigate(['/home']);
  }
  filterTransactionsByMonth(month: number, tran: TransactionShow[]): TransactionShow[] {
    return tran.filter(transaction => {
      const transactionDate = new Date(transaction.date);
      return transactionDate.getMonth() === month && transactionDate.getFullYear() === (new Date()).getFullYear();
    });
  }
}
