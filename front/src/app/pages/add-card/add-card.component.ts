import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { Card, CardDTO } from 'src/app/shared/services/card/card.model';
import { CardService } from 'src/app/shared/services/card/card.service';
import { TranslationService } from 'src/app/shared/services/translation.service';
import { UserService } from 'src/app/shared/services/user/user.service';

@Component({
  selector: 'app-add-card',
  templateUrl: './add-card.component.html',
  styleUrls: ['./add-card.component.css']
})
export class AddCardComponent {
  addForm!: FormGroup;
  type = 'none';
  isCredit: boolean = false;
  card: CardDTO;

  constructor(private router: Router, private cardService: CardService,
     private userService: UserService, private translate: TranslationService){
    this.translate.useTranslation('cards');
    let name='', id='';
    this.userService.userDetails.subscribe(res => {
      name = res.fullName;
      id = res.id;
    }) 
    this.addForm = new FormGroup({
      typeCard: new FormControl("Debit", Validators.required),
      name: new FormControl(name),
      expiryDate: new FormControl('', Validators.required),
      cvv: new FormControl(null,[Validators.required, Validators.pattern(/^\d{4}$/)] ),
      card: new FormControl('',[Validators.required, Validators.pattern(/^\d{16}$/)])
     });
     this.card = {
      ownerId: id,
      nameHolder: name,
      type: 'none',
      typeCard: 'none',
      network: "",
      cardNumber: '',
      cvv: '',
      expiryDate: '',
      balance: 0,
      valute: '', 
     }
     this.addForm.valueChanges.subscribe(values => {
      this.card.type = values.type != null ? values.type : '';
      this.card.typeCard = values.typeCard != null ? values.typeCard : '';
        this.card.cvv = values.cvv != null ? values.cvv : '';
        this.card.nameHolder = values.name != null ? values.name : '';
        this.card.expiryDate = values.expiryDate != null ? values.expiryDate : '';
        this.card.cardNumber = values.card != null ? values.card : '';
       if(this.card.cardNumber.length <= 1){
        this.determineCardType();
       }
    });
  }
  onTypeCardChange(): void {
    const typeCard = this.addForm.get('typeCard')?.value;
    this.isCredit = typeCard === 'Credit';

    if (this.isCredit) {
      this.addForm.get('type')?.setValidators(Validators.required);
    } else {
      this.addForm.get('type')?.clearValidators();
      this.addForm.get('type')?.setValue('');
    }

    this.addForm.get('type')?.updateValueAndValidity();
  }
  determineCardType(){
    if(this.card.cardNumber[0] == '4'){
      this.card.network = 'visa';
    }else if(this.card.cardNumber[0] == '5'){
      this.card.network = 'mastercard';
    }else{
      this.card.network = 'none';
    }
  }
  transformCardToCardDTO(card: Card): CardDTO {
     const { id, ...cardDTO } = card;
     return cardDTO;
  }

  addCard() {
    //const cardDto: CardDTO = this.transformCardToCardDTO(this.card);
    this.cardService.addCard(this.card).subscribe(res => {
      if(res){
        this.router.navigate(['/all-cards'])
      }
    })
  }
  transformCardNumber(nr: string) : string{
    let newstr = '';
    nr = nr.replace(/\s+/g, '');
    for (let i = 0; i < nr.length; i += 4) {
      newstr += nr.slice(i, i + 4) + ' ';
    }
    if(nr.length == 16){
      let crypted = '';
      for (let i = 0; i < newstr.length; i++) {
        if(i<14 && newstr[i] != ' ' ){
           crypted += '*';
        }else if(i<14 && newstr[i] == ' ' ){
          crypted += ' ';
        }else{
          crypted += newstr[i];
        }
      }
      newstr = crypted;
    }
    return newstr.trim();
  }
}
