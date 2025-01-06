import { Component } from '@angular/core';

import { TransactionBuy, TransactionShow } from 'src/app/shared/services/transaction/transactions.model';
import { TransactionService } from 'src/app/shared/services/transaction/transactions.service';
import { TranslationService } from 'src/app/shared/services/translation.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {
  transactionsOriginal: TransactionShow[] = [];
  transactions: TransactionShow[] = [];
  search: string = '';

  constructor(private transactionService: TransactionService, 
    private translate: TranslationService){
      this.translate.useTranslation('home');
    this.transactionService.getTransactionsBuy().subscribe(transactions => {
      this.transactionsOriginal = transactions;
      this.transactions = transactions;
    })
  }

  onSearch(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const searchValue = inputElement.value;

    if (!searchValue) {
      this.transactions = this.transactionsOriginal;
      return;
    }

    const lowerCaseSearchValue = searchValue.toLowerCase();
    this.transactions = this.transactionsOriginal.filter(transaction => {
      const { title, subtitle, sum } = transaction;
      return title.toLowerCase().includes(lowerCaseSearchValue) ||
             subtitle.toLowerCase().includes(lowerCaseSearchValue) ||
             sum.toString().includes(searchValue);
    });
  }

  clearSearch(): void {
    this.transactions = this.transactionsOriginal;
    this.search = "";
  }
}
