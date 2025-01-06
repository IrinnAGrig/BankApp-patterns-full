import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RequestMoneyComponent } from './request-money.component';

const routes: Routes = [{
  path: '',
  component: RequestMoneyComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RequestMoneyRoutingModule { }
