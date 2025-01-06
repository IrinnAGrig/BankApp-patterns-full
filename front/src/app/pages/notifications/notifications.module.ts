import { NgModule } from '@angular/core';

import { NotificationsComponent } from './notifications.component';
import { NotificationsRoutingModule } from './notifications-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    NotificationsComponent
  ],
  imports: [
    SharedModule,
    NotificationsRoutingModule
  ]
})
export class NotificationsModule { }
