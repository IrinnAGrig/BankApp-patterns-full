import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { RequestsModel } from 'src/app/shared/services/requests/requests.model';
import { RequestsService } from 'src/app/shared/services/requests/requests.service';
import { TranslationService } from 'src/app/shared/services/translation.service';

@Component({
  selector: 'app-request-money',
  templateUrl: './request-money.component.html',
  styleUrls: ['./request-money.component.css']
})
export class RequestMoneyComponent {
  editForm!: FormGroup;

  constructor(private requestService: RequestsService, private router: Router, private translate: TranslationService){
    this.translate.useTranslation('home');
    this.editForm = new FormGroup({
      name: new FormControl('', Validators.required),
      phone: new FormControl('', Validators.required),
      email: new FormControl('', Validators.required),
      dueDate: new FormControl(null, Validators.required),
      amount: new FormControl(0, Validators.required)
    });
  }

  addRequest(){
    if(this.editForm.valid){
      let value = this.editForm.value;
      let request: RequestsModel = {
        id: Math.random().toFixed(8),
        name: value.name,
        receiverId: '',
        senderId: '',
        amount: value.amount,
        valute: "$",
        dueDate: value.dueDate,
        phone: value.phone,
        email: value.email,
        dateSent: (new Date()).toISOString(),
        closed: false,
        status: 'P',
        dateClosed: "",
        isRead: false
      }

      this.requestService.addTransaction(request).subscribe(res => {
        if(res){
          this.router.navigate(['/home']);
        }
      });
    }
  }
}
