import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { ChangePasswordComponent } from './change-password.component';
import { ChangePasswordRoutingModule } from './change-password-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    ChangePasswordComponent
  ],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    ChangePasswordRoutingModule
  ]
})
export class ChangePasswordModule { }
