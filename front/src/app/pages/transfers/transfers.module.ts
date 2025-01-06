import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { DropdownCardComponent } from 'src/app/shared/components/dropdown-card/dropdown-card.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { TransfersComponent } from './transfers.component';
import { TransfersRoutingModule } from './transfers-routing.module';

@NgModule({
  declarations: [
    TransfersComponent,
    DropdownCardComponent
  ],
  imports: [
    SharedModule,
    FormsModule,
    TransfersRoutingModule
  ]
})
export class TransfersModule { }
