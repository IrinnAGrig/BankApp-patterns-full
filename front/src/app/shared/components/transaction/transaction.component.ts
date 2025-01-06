import { Component, Input, OnChanges } from '@angular/core';

import { TransactionShow } from '../../services/transaction/transactions.model';
import { TranslationService } from '../../services/translation.service';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css']
})
export class TransactionComponent implements OnChanges{
  @Input() transactions: TransactionShow[] = [];
  @Input() type: 'simple' | 'advanced' | 'months' = 'simple';

  filteredTransactions: { label: string, year: number, transactions: TransactionShow[] }[] = [];
  
  constructor(private translate: TranslationService){
    this.translate.useTranslation('transactions');
  }

  ngOnChanges(): void {
    if ( this.type === 'advanced') {
      this.filterTransactions();
    } else if ( this.type === 'months'){
      this.filterTransactionsByMonth();
    } else {
      this.filteredTransactions = [{ label: 'All', year: 0,  transactions: this.transactions }];
    }
  }
  
  filterTransactions(): void {
    const today = new Date();
    const oneDayMs = 24 * 60 * 60 * 1000;
  
    const todayTransactions = this.transactions.filter(transaction => 
      new Date(transaction.date).toDateString() === today.toDateString()
    );

    const last7DaysTransactions = this.transactions.filter(transaction => {
      const transactionDate = new Date(transaction.date);
      const isInLast7Days = (today.getTime() - transactionDate.getTime()) < 7 * oneDayMs;
      return isInLast7Days && transactionDate.toDateString() !== today.toDateString();
    });

    const last30DaysTransactions = this.transactions.filter(transaction => {
      const transactionDate = new Date(transaction.date);
      const isInLast30Days = (today.getTime() - transactionDate.getTime()) < 30 * oneDayMs;
      return isInLast30Days && transactionDate.toDateString() !== today.toDateString() && !last7DaysTransactions.includes(transaction);
    });
  
    this.filteredTransactions = [
      { label: 'TODAY', year: 0, transactions: todayTransactions },
      { label: 'LAST-7DAYS', year: 0, transactions: last7DaysTransactions },
      { label: 'LAST-MONTH', year: 0, transactions: last30DaysTransactions }
    ];
  }
  filterTransactionsByMonth(): void {
    const formatMonth = (date: Date) => {
      const options = { month: 'long' } as const;
      return new Intl.DateTimeFormat('en-US', options).format(date);
    };
  
    const formatYear = (date: Date) => {
      const options = { year: 'numeric' } as const;
      return new Intl.DateTimeFormat('en-US', options).format(date);
    };
  
    const transactionsByMonth = new Map<string, { month: string, year: number, transactions: TransactionShow[] }>();
  
    this.transactions.forEach(transaction => {
      const transactionDate = new Date(transaction.date);
      const month = formatMonth(transactionDate);
      const year = parseInt(formatYear(transactionDate), 10);
      const key = `${month} ${year}`;
  
      if (!transactionsByMonth.has(key)) {
        transactionsByMonth.set(key, { month, year, transactions: [] });
      }
      transactionsByMonth.get(key)?.transactions.push(transaction);
    });
  
    this.filteredTransactions = Array.from(transactionsByMonth.values()).map(({ month, year, transactions }) => ({
      label: month,
      year: year,
      transactions: transactions
    }));
  }
  
  
}
