import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { Card } from 'src/app/shared/services/card/card.model';
import { CardService } from 'src/app/shared/services/card/card.service';
import { TranslationService } from 'src/app/shared/services/translation.service';

@Component({
  selector: 'app-all-cards',
  templateUrl: './all-cards.component.html',
  styleUrls: ['./all-cards.component.css']
})
export class AllCardsComponent {
  cards: Card[] = [];

  constructor(private router: Router, private cardService: CardService, 
    private translate: TranslationService){
    this.translate.useTranslation('cards');
       this.cardService.getCardsByIdUser().subscribe(res => {
            console.log(res);
      this.cards = res;
    }) 
  }
  goToAddCard(){
    this.router.navigate(['/add-card']);
  }
}
