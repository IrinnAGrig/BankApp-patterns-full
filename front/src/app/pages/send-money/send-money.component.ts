import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Card } from 'src/app/shared/services/card/card.model';
import { CardService } from 'src/app/shared/services/card/card.service';
import { TransactionBuy, TransactionDTO} from 'src/app/shared/services/transaction/transactions.model';
import { TransactionService } from 'src/app/shared/services/transaction/transactions.service';
import { TranslationService } from 'src/app/shared/services/translation.service';
import { RecentUser, RecentUserSimple } from 'src/app/shared/services/user/user.model';
import { UserService } from 'src/app/shared/services/user/user.service';

@Component({
  selector: 'app-send-money',
  templateUrl: './send-money.component.html',
  styleUrls: ['./send-money.component.css']
})
export class SendMoneyComponent {
  cards: Card[] = [];
  recentUsers: RecentUser[] = [];
  amount = 0;
  selectedUser: RecentUser | null = null;
  selectedCard: Card| null = null;
  addNew = false;
  form: FormGroup;

  constructor( private translate: TranslationService,
  private userService: UserService,
      private cardService: CardService,
    private transactionService: TransactionService){
        this.form = new FormGroup({
         name: new FormControl('',Validators.required),
         cardNumber: new FormControl('',[Validators.required, Validators.pattern(/^\d{16}$/)]),
        })
      this.translate.useTranslation('home');
      this.userService.userDetails.subscribe(res => {
        this.cardService.getCardsByIdUser().subscribe(cards => {
          this.cards = cards
        })
        this.userService.getHistoryUsers().subscribe(res => {
          this.recentUsers = res;

        })
      })
  }
  sendMoney(){
    if(this.selectedUser && this.selectedCard){
      let value: TransactionDTO = {
        type: 'Transfer',
        receiverId: this.selectedUser?.id,
        senderId: '',
        title: 'MONEYTRANSFER',
        subtitle: 'TRANSACTION',
        emblem: 'none',
        sum: this.amount,
        valute: "$"
      }

      this.transactionService.addTransactionTransfer(value).subscribe(res =>{
        this.userService.userDetails.subscribe(user => {
          if(this.addNew){
            this.cardService.findCardByNumberAndName(this.selectedCard!.cardNumber).subscribe(card => {
              let val: RecentUserSimple = {
                id: card.ownerId,
                cardNumber: card.cardNumber
              }
              user.historyTransfers.push(val);
              this.userService.editProfile(user).subscribe()
            })
          }else{
            if(this.selectedUser){
              let val: RecentUserSimple = {
                id: this.selectedUser.id,
                cardNumber: this.selectedUser.cardNumber
              }  
              this.userService.editProfile(user).subscribe()
            }     
          }      
        })
      })
    }
  }
  chooseCard(card: Card){
    this.selectedCard = card;
  }
  deselectCard(){
    this.selectedCard = null;
  }
  deselectUser(){
    this.selectedUser = null;
  }
  deselectAddNewUser(){
    this.addNew = false;
    this.form.reset();
  }
  addNewUserTransfer(){
    this.addNew = true;
  }
}
