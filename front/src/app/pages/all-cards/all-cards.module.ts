import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AllCardsRoutingModule } from './all-cards-routing.module';
import { AllCardsComponent } from './all-cards.component';
import { SharedModule } from "../../shared/shared.module";

@NgModule({
  declarations: [
    AllCardsComponent
  ],
  imports: [
    CommonModule,
    AllCardsRoutingModule,
    SharedModule
]
})
export class AllCardsModule { }
