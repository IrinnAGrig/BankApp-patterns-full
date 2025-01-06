import { NgModule } from '@angular/core';

import { ProfileRoutingModule } from './profile-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { ProfileComponent } from './profile.component';

@NgModule({
  declarations: [
    ProfileComponent
  ],
  imports: [
    SharedModule,
    ProfileRoutingModule
  ]
})
export class ProfileModule { }
