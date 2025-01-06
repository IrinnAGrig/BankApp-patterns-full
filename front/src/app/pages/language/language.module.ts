import { NgModule } from '@angular/core';

import { LanguageComponent } from './language.component';
import { LanguageRoutingModule } from './language-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    LanguageComponent
  ],
  imports: [
    SharedModule,
    LanguageRoutingModule
  ]
})
export class LanguageModule { }
