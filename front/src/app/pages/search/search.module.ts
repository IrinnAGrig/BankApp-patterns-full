import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { SearchComponent } from './search.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { SearchRoutingModule } from './search-routing.module';

@NgModule({
  declarations: [
    SearchComponent
  ],
  imports: [
    FormsModule,
    SharedModule,
    SearchRoutingModule
  ]
})
export class SearchModule { }
