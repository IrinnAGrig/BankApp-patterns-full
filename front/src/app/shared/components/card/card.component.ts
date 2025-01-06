import { Component, Input } from '@angular/core';

import { Card, CardDTO } from '../../services/card/card.model';

@Component({
  selector: 'app-card-mastercard',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent {
  @Input() card!: CardDTO;

  constructor(){
  }
}
