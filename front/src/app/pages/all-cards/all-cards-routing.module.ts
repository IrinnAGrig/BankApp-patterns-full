import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AllCardsComponent } from './all-cards.component';

const routes: Routes = [{
  path: '',
  component: AllCardsComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AllCardsRoutingModule { }
