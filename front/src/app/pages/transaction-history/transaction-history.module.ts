import { NgModule } from '@angular/core';

import { TransactionHistoryComponent } from './transaction-history.component';
import { TransactionHistoryRoutingModule } from './transaction-history-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    TransactionHistoryComponent
  ],
  imports: [
    SharedModule,
    TransactionHistoryRoutingModule
  ]
})
export class TransactionHistoryModule { }
