import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { Card } from 'src/app/shared/services/card/card.model';
import { CardService } from 'src/app/shared/services/card/card.service';
import { TransactionBuy, TransactionShow } from 'src/app/shared/services/transaction/transactions.model';
import { TransactionService } from 'src/app/shared/services/transaction/transactions.service';
import { TranslationService } from 'src/app/shared/services/translation.service';
import { UserService } from 'src/app/shared/services/user/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  card?: Card; 
  transactions: TransactionShow[] = [];
  userData: {name: string, image: string} = {name: '', image: ''};

  constructor(private router: Router, private translate: TranslationService,
    private transactionService: TransactionService, private userService: UserService,
     private cardService: CardService){
    this.translate.useTranslation('home');
    this.userService.userDetails.subscribe(res => {
      this.userData.name = res.fullName;
      this.userData.image = res.image;
      this.transactionService.getJustPartTransactions(4).subscribe(transactions => {
        this.transactions = transactions;
      })
      this.cardService.getCardsByIdUser().subscribe(cards => {
        this.card = cards[0];
      })
    })
  }
  goToTransactions(){
    this.router.navigate(['/transaction-history']);
  }
  goToSearch(){
    this.router.navigate(['/search']);
  }
  goToSendMoney(){
    this.router.navigate(['/send-money']);
  }
  goToRequestMoney(){
    this.router.navigate(['/request-money']);
  }
  gotoCards(){
    this.router.navigate(['/all-cards']);
  }
}
