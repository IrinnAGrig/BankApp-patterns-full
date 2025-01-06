import { NgModule } from '@angular/core';

import { IntroRoutingModule } from './intro-routing.module';
import { IntroComponent } from './intro.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    IntroComponent
  ],
  imports: [
    SharedModule,
    IntroRoutingModule
  ]
})
export class IntroModule { }
