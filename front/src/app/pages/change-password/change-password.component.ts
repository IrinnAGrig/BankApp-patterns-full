import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { TranslationService } from 'src/app/shared/services/translation.service';
import { UserService } from 'src/app/shared/services/user/user.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {
  passwordForm: FormGroup;
  seePassword = false;
  errors = {
    short: {
      oldPassword: false,
      newPassword: false,
      repetedNewPassword: false
    },
    required: {
      oldPassword: false,
      newPassword: false,
      repetedNewPassword: false
    },
    passwordsDontMatch: false,
    oldNewMatch: false,
    failedChange: false,
    oldPassIncorrect: false
  };
  private id: string = '';

  constructor(private userService: UserService, 
    private router: Router,
    private translate: TranslationService){
    this.translate.useTranslation('settings');
    this.userService.userDetails.subscribe(res => {
      this.id = res.id;
    }) 
    this.passwordForm = new FormGroup({
      oldPassword: new FormControl('', Validators.required),
      newPassword: new FormControl('', Validators.required),
      newRepeatedPassword: new FormControl('', Validators.required),
  });
  }

  onSubmit(){
    this.restartErrors();
    let value = this.passwordForm.value;
    this.verifyOldPassword(value.oldPassword);
    this.verifyNewPassword(value.newPassword);
    this.verifyReapetedNewPassword(value.newRepeatedPassword);
    if(value.newPassword != value.newRepeatedPassword){
      this.errors.passwordsDontMatch = true;
    }else{
      if(this.passwordForm.valid)
        this.userService.changePassword(this.id, value.oldPassword, value.newPassword).subscribe(res => {
          switch(res.error) {
              case 'match-oldnew-pass': 
                  res.hasError ? this.errors.oldNewMatch = true : null; 
                  break;
              case 'failed-update': 
                  res.hasError ? this.errors.failedChange = true : null; 
                  break;
              case 'pass-incorrect': 
                  res.hasError ? this.errors.oldPassIncorrect = true : null; 
                  break;
              default: 
                  break;
          }
      
          if (!res.hasError) {
              this.router.navigate(['/settings']);
          }
      });
      
    }
  }
  verifyOldPassword(pass: string){
    if (pass == null || pass == "") {
      this.errors.required.oldPassword = true;
    } else {
      this.errors.required.oldPassword = false;
      if (pass.length < 6) {
        this.errors.short.oldPassword = true;
      } else {
        this.errors.short.oldPassword = false;
      }
    }
  }
  verifyNewPassword(pass: string){
    if (pass == null || pass == "") {
      this.errors.required.newPassword = true;
    } else {
      this.errors.required.newPassword= false;
      if (pass.length < 6) {
        this.errors.short.newPassword= true;
      } else {
        this.errors.short.newPassword = false;
      }
    }
  }
  verifyReapetedNewPassword(pass: string){
    if (pass == null || pass == "") {
      this.errors.required.repetedNewPassword = true;
    } else {
      this.errors.required.repetedNewPassword= false;
      if (pass.length < 6) {
        this.errors.short.repetedNewPassword = true;
      } else {
        this.errors.short.repetedNewPassword = false;
      }
    }
  }
  restartErrors(){
    this.errors = {
      short: {
        oldPassword: false,
        newPassword: false,
        repetedNewPassword: false
      },
      required: {
        oldPassword: false,
        newPassword: false,
        repetedNewPassword: false
      },
      passwordsDontMatch: false,
      oldNewMatch: false,
      failedChange: false,
      oldPassIncorrect: false
    };
  }
}
