import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { RequestMoneyComponent } from './request-money.component';
import { RequestMoneyRoutingModule } from './request-money-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    RequestMoneyComponent
  ],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    RequestMoneyRoutingModule
  ]
})
export class RequestMoneyModule { }
