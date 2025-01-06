import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { TranslationService } from 'src/app/shared/services/translation.service';
import { UserService } from 'src/app/shared/services/user/user.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent {
  lang = 'English';
  darkMode = false;
  languages = [
    {
      title: 'English',
      acronym: 'en',
    },
    {
      title: 'French',
      acronym: 'fr',
    },
    {
      title: 'Italian',
      acronym: 'it',
    },
    {
      title: 'Romanian',
      acronym: 'ro',
    },
    {
      title: 'Russian',
      acronym: 'ru',
    },
    {
      title: 'Spanish',
      acronym: 'es',
    },
  ];

  constructor(private router: Router, private userService: UserService, private translate: TranslationService){
    this.darkMode = this.userService.darkMode;
    this.translate.useTranslation('settings');
    this.userService.userDetails.subscribe(res => {
      this.lang = this.languages.find(el => el.acronym == res.language)?.title || 'English';
    })
  }

  goToChangePassword(){
    this.router.navigate(['/change-password']);
  }
  goToConditions(){
    this.router.navigate(['/conditions']);
  }
  logout(){
    this.userService.logout();
  }
  goHome(){
    this.router.navigate(['/home']);
  }
  changeMode(){
    this.darkMode = !this.darkMode;
    localStorage.setItem('dark-mode', JSON.stringify(this.darkMode));
    this.userService.darkMode = this.darkMode;
    if (this.darkMode) {
      document.body.classList.add('dark-mode');
      document.body.classList.remove('light-mode');
    } else {
        document.body.classList.add('light-mode');
        document.body.classList.remove('dark-mode');
    }
    window.location.reload();
  }
}
