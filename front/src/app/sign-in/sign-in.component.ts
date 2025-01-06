import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { UserService } from '../shared/services/user/user.service';
import { LoginData, SignUpData } from '../shared/services/user/user.model';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent {
  isSignIn = true;
  loginForm!: FormGroup;
  seePassword = false;
  errors = {
    invalidUser: false,
    emailIncorrect: false,
    passwordShort: false,
    emailRequired: false,
    passwordRequired: false,
    nameRequired: false,
    phoneRequired: false
  };

  constructor(private fb: FormBuilder, private userService: UserService, private router: Router) {
    this.resetForm();
    this.changeForm(true);
  }

  changeForm(type: boolean) {
    this.isSignIn = type;
    if (type) {
      this.loginForm.setValue({
        name: 'None',
        phone: 'None',
        email: '',
        password: ''
      });
    } else {
      this.resetForm();
    }
    this.resetErrors();
  }
  resetForm() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      name: ['', [Validators.required]],
      phone: ['', [Validators.required]]
    });
  }
  onSubmit() {
    let value = this.loginForm.value;
    if (this.loginForm.invalid) {
      if (value.email.length == 0) {
        this.errors.emailRequired = true;
      } else {
        this.errors.emailRequired = false;
        const emailRegex = new RegExp('^([a-zA-Z0-9._%+-]+)@([a-zA-Z0-9.-]+)\\.([a-zA-Z]{2,})$');
        if (!emailRegex.test(value.email)) {
          this.errors.emailIncorrect = true;
        } else {
          this.errors.emailIncorrect = false;
        }
      }
      if (!this.isSignIn) {
        if (value.phone.length == 0) {
          this.errors.phoneRequired = true;
        } else {
          this.errors.phoneRequired = false;
        }
        if (value.name.length == 0) {
          this.errors.nameRequired = true;
        } else {
          this.errors.nameRequired = false;
        }
      }
      this.verifyPassword(value.password);

    } else {
      if(this.isSignIn){
        let data: LoginData = { email: value.email, password: value.password};
        this.userService.signIn(data).subscribe(res => {
          if(res){
            localStorage.setItem('userDetails', JSON.stringify(res));
          }else{
            this.errors.invalidUser = true;
          }
          
          if (!this.errors.invalidUser) {
            this.router.navigate(['/home']);
          }
        });
      }else{
        let data: SignUpData = { email: value.email, password: value.password, fullName: value.name, phone: value.phone};
        this.userService.signUp(data).subscribe(res => {
        
          if(res){
            localStorage.setItem('userDetails', JSON.stringify(res));
            this.router.navigate(['/home']);
          }else{
            this.errors.invalidUser = true;
          }
          
        });
      }
      
    }
  }
  areAllErrorsFalse(): boolean {
    return Object.values(this.errors).every(value => value === false);
  }
  verifyPassword(pass: string){
    if (pass.length == 0) {
      this.errors.passwordRequired = true;
    } else {
      this.errors.passwordRequired = false;
      if (pass.length < 6) {
        this.errors.passwordShort = true;
      } else {
        this.errors.passwordShort = false;
      }
    }
  }
 
  resetErrors(): void {
    this.errors.invalidUser = false;
    this.errors.emailIncorrect = false;
    this.errors.passwordShort = false;
    this.errors.emailRequired = false;
    this.errors.passwordRequired = false;
  }
}
