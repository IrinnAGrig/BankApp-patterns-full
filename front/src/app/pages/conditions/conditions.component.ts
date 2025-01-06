import { Component } from '@angular/core';

import { TranslationService } from 'src/app/shared/services/translation.service';

@Component({
  selector: 'app-conditions',
  templateUrl: './conditions.component.html',
  styleUrls: ['./conditions.component.css']
})
export class ConditionsComponent {
  constructor(private translate: TranslationService){
    this.translate.useTranslation('settings');
  }
}
