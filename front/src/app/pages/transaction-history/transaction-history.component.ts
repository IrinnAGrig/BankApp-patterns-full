import { Component } from '@angular/core';

import { TransactionBuy, TransactionShow } from 'src/app/shared/services/transaction/transactions.model';
import { TransactionService } from 'src/app/shared/services/transaction/transactions.service';
import { TranslationService } from 'src/app/shared/services/translation.service';

@Component({
  selector: 'app-transaction-history',
  templateUrl: './transaction-history.component.html',
  styleUrls: ['./transaction-history.component.css']
})
export class TransactionHistoryComponent {
  transactions: TransactionShow[] = [];
  
  constructor( private translate: TranslationService,
    private transactionService: TransactionService){
    this.translate.useTranslation('transactions');
    this.transactionService.getTransactionsBuy().subscribe(transactions => {
      this.transactions = transactions;
    });
  }
}
