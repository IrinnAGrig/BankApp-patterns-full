import { Component } from '@angular/core';

import { Card } from 'src/app/shared/services/card/card.model';
import { CardService } from 'src/app/shared/services/card/card.service';
import { ServicesDTO, TransactionBuy, TransactionBuyDto, TransactionDTO } from 'src/app/shared/services/transaction/transactions.model';
import { TransactionService } from 'src/app/shared/services/transaction/transactions.service';
import { TranslationService } from 'src/app/shared/services/translation.service';
import { UserDetails } from 'src/app/shared/services/user/user.model';

@Component({
  selector: 'app-transfers',
  templateUrl: './transfers.component.html',
  styleUrls: ['./transfers.component.css']
})
export class TransfersComponent {
  doTransfer = false;
  services: ServicesDTO[] = [];
//   services = [{
//     title: 'APPLE',
//     subtitle: 'SHOPPING',
//     emblem: './assets/images/transactions/apple.png'
//   },{
//     title: 'SPOTIFY',
//     subtitle: 'ENTERTAIMENT',
//      emblem: './assets/images/transactions/spotify.png'
//   },{
//     title: 'GROCERY',
//     subtitle: 'FOOD',
//      emblem: './assets/images/transactions/grocery.png'
//   },{
//     title: 'NETFLIX',
//     subtitle: 'ENTERTAIMENT',
//      emblem: './assets/images/transactions/netflix.png'
//   },{
//     title: 'GAMES',
//     subtitle: 'ENTERTAIMENT',
//      emblem: './assets/images/transactions/games.png'
//   },{
//     title: 'COMUNAL',
//     subtitle: 'SERVICES',
//     emblem: './assets/images/transactions/comunal.png'
//   },
//   {
//     title: 'CAR',
//     subtitle: 'AUTOSERVICE',
//     emblem: './assets/images/transactions/car.png'
//   },
//   {
//     title: 'VACATION',
//     subtitle: 'TRAVEL',
//     emblem: './assets/images/transactions/travel-icon.png'
//   }
// ]
firstSelectedCard!: Card;
secondSelectedCard!: Card;
cards: Card[] = [];
amount: number = 0;
limit = 0;
errorLimit = false;
selectedService: ServicesDTO | null = null;

  constructor(private translate: TranslationService, 
    private transactionService: TransactionService, 
    private cardService: CardService,
  ){
    this.translate.useTranslation('transactions');
    this.cardService.getCardsByIdUser().subscribe(res =>{
      this.firstSelectedCard = res[0];
      this.secondSelectedCard = res[1];
      this.cards = res;
    })
    this.getServices();
    let mode = localStorage.getItem('userDetails');
    if(mode){
        this.limit = (JSON.parse(mode) as UserDetails).spendingLimit;
    }
  }
  getServices(){
    this.transactionService.getServices().subscribe(res => this.services = res)
  }
  selectService(service: any){
    this.doTransfer = false;
    if(this.selectedService == service){
      this.selectedService = null;
      this.amount = 0;
    }else{
      this.selectedService = service;
      this.amount = 0;
    }
  }
  appearTransferForm(val: boolean){
    this.doTransfer = val;
    this.selectedService = null;
  }
  transfromCard(nr: string){
    return "*" + nr.slice(11, 15);
  }
  changeCardsFirst(card: Card){
    if(card == this.secondSelectedCard){
      this.firstSelectedCard = card;
      this.secondSelectedCard = card == this.cards[0] ? this.cards[1] : this.cards[0];
    }
  }
  changeCardsSecond(card: Card){
    if(card == this.firstSelectedCard){
      this.secondSelectedCard = card;
      this.firstSelectedCard = card == this.cards[0] ? this.cards[1] : this.cards[0];
    }
  }
  saveTransaction(){
    if(this.limit < this.amount){
      this.errorLimit = true;
    }else{
      this.errorLimit = false;  
      let value: TransactionBuyDto = {
        senderId: '',
        serviceId: this.selectedService?.idService || 'no' ,
        sum: this.amount,
        valute: '$',
        cardId: this.firstSelectedCard.id
      }
      this.transactionService.addTransactionBuy(value).subscribe(res => {
        if(res){
          this.selectedService = null;
        }
        // for(let i=0; i<this.cards.length;){
        //   if(this.cards[i].balance > this.amount){
        //     this.cards[i].balance -= this.amount;
        //     this.cardService.updateCard(this.cards[i]).subscribe();
        //     break;
        //   }else{
        //     this.amount = this.amount - this.cards[i].balance;
        //     this.cards[i].balance = 0;
        //     this.cardService.updateCard(this.cards[i]).subscribe();
        //     i++;
        //   }
        // }
      })
    }
    
  }
  performTransferBetweenCards(){
    if(this.amount > 0){
      let actualAmount = this.firstSelectedCard.balance - this.amount;
      if(actualAmount >= 0){
        this.firstSelectedCard.balance = this.firstSelectedCard.balance - this.amount;
      }else{
        this.firstSelectedCard.balance = 0;
      }
      this.secondSelectedCard.balance += this.amount;
      this.cardService.updateCard(this.firstSelectedCard).subscribe( () => 
      this.cardService.updateCard(this.secondSelectedCard).subscribe( () => {
        this.doTransfer = false;
        this.amount = 0;
      }))
    }
  }
}
