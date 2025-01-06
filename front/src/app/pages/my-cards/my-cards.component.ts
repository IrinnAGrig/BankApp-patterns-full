import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { Card } from 'src/app/shared/services/card/card.model';
import { CardService } from 'src/app/shared/services/card/card.service';
import { TransactionBuy, TransactionShow } from 'src/app/shared/services/transaction/transactions.model';
import { TransactionService } from 'src/app/shared/services/transaction/transactions.service';
import { TranslationService } from 'src/app/shared/services/translation.service';
import { UserDetails } from 'src/app/shared/services/user/user.model';
import { UserService } from 'src/app/shared/services/user/user.service';

@Component({
  selector: 'app-my-cards',
  templateUrl: './my-cards.component.html',
  styleUrls: ['./my-cards.component.css']
})
export class MyCardsComponent{
  amount: number = 0;
  card?: Card;
  user!: UserDetails;
  initialAmount = 0;
  transactions: TransactionShow[] = [];

  constructor(private router: Router, private cardService: CardService,
     private userService: UserService, private transactionService: TransactionService, 
     private translate: TranslationService){
      this.translate.useTranslation('cards');
    this.userService.userDetails.subscribe(res => {
      this.user = res;
      this.amount = res.spendingLimit;
      this.initialAmount = this.amount;
      this.transactionService.getJustPartTransactions(3).subscribe(trans => this.transactions = trans)
      this.cardService.getCardsByIdUser().subscribe(res => {
        this.card = res[0];
      })
    }) 
  }

  goToAllCards(){
    this.router.navigate(['/all-cards']);
  }
  
  setLimit(){
    this.user.spendingLimit = this.amount;
    this.userService.editProfile(this.user).subscribe(()=> this.initialAmount = this.amount);
  }

  cancelLimit(){
    this.amount = this.initialAmount;
  }
}
