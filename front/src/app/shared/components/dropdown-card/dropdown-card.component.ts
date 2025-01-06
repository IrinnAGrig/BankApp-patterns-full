import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';

import { Card } from '../../services/card/card.model';

@Component({
  selector: 'app-dropdown-card',
  templateUrl: './dropdown-card.component.html',
  styleUrls: ['./dropdown-card.component.css']
})
export class DropdownCardComponent implements OnChanges{
  @Input() cardList: Card[] = []; 
  @Input() currentSelectedCard?: Card;
  @Output() selectedCard = new EventEmitter<Card>();

  shownCards: Card[] = [];
  isOpen = false;

  constructor(){ }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['cardList'] && this.cardList.length > 0) {
      this.shownCards = this.cardList;
      this.removeSelectedCard();
    }
    if (changes['currentSelectedCard'] && this.cardList.length > 0) {
      this.shownCards = this.cardList;
      this.removeSelectedCard();
    }
  }

  removeSelectedCard(): void {
    if (this.currentSelectedCard) {
      this.shownCards = this.cardList.filter(card => card !== this.currentSelectedCard);
      this.selectedCard.emit(this.currentSelectedCard);
    }
  }
  selectCard(card: Card){
    this.currentSelectedCard = card;
    this.removeSelectedCard();
  }

  toggleDropdown() {
    this.isOpen = !this.isOpen;
  }
  modifyCard(cardNr: string){
    return cardNr.slice(11, 15);
  }
}
