import { NgModule } from '@angular/core';

import { ConditionsComponent } from './conditions.component';
import { ConditionsRoutingModule } from './conditions-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    ConditionsComponent
  ],
  imports: [
    SharedModule,
    ConditionsRoutingModule
  ]
})
export class ContidionsModule { }
