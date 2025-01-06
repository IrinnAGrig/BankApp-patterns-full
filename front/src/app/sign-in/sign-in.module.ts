import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { SignInRoutingModule } from './sign-in-routing.module';
import { SignInComponent } from './sign-in.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    SignInComponent
  ],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    SignInRoutingModule
  ]
})
export class SignInModule { }
