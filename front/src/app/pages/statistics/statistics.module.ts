import { NgModule } from '@angular/core';

import { StatisticsRoutingModule } from './statistics-routing.module';
import { StatisticsComponent } from './statistics.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    StatisticsComponent
  ],
  imports: [
    SharedModule,
    StatisticsRoutingModule
  ]
})
export class StatisticsModule { }
