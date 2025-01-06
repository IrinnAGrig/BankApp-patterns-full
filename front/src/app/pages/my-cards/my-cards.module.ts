import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MyCardsRoutingModule } from './my-cards-routing.module';
import { MyCardsComponent } from './my-cards.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { RangeSliderComponent } from 'src/app/shared/components/range-slider/range-slider.component';

@NgModule({
  declarations: [
    MyCardsComponent,
    RangeSliderComponent
  ],
  imports: [
    SharedModule,
    FormsModule,
    MyCardsRoutingModule
  ]
})
export class MyCardsModule { }
