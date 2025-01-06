import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { AddCardRoutingModule } from './add-card-routing.module';
import { AddCardComponent } from './add-card.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    AddCardComponent
  ],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    AddCardRoutingModule
  ]
})
export class AddCardModule { }
