import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import { EditProfileRoutingModule } from './edit-profile-routing.module';
import { EditProfileComponent } from './edit-profile.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    EditProfileComponent
  ],
  imports: [
    SharedModule,
    ReactiveFormsModule,
    EditProfileRoutingModule
  ]
})
export class EditProfileModule { }
