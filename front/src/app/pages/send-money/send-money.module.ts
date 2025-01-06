import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SendMoneyComponent } from './send-money.component';
import { SendMoneyRoutingModule } from './send-money-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    SendMoneyComponent
  ],
  imports: [
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    SendMoneyRoutingModule
  ]
})
export class SendMoneyModule { }
