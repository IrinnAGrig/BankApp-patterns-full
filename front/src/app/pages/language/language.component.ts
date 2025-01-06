import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { TranslationService } from 'src/app/shared/services/translation.service';
import { UserDetails } from 'src/app/shared/services/user/user.model';
import { UserService } from 'src/app/shared/services/user/user.service';

export interface Language{
  image: string; title: string; selected: boolean;acronym: string;
}

@Component({
  selector: 'app-language',
  templateUrl: './language.component.html',
  styleUrls: ['./language.component.css']
})
export class LanguageComponent {
  user!: UserDetails;
  indexLang: number = 0;
  languages: Language[] = [
    {
      image: './assets/images/languages/english.png',
      title: 'English',
      acronym: 'en',
      selected: false
    },
    {
      image: './assets/images/languages/french.png',
      title: 'French',
      acronym: 'fr',
      selected: false
    },
    {
      image: './assets/images/languages/italian.png',
      title: 'Italian',
      acronym: 'it',
      selected: false
    },
    {
      image: './assets/images/languages/romanian.png',
      title: 'Romanian',
      acronym: 'ro',
      selected: false
    },
    {
      image: './assets/images/languages/russian.png',
      title: 'Russian',
      acronym: 'ru',
      selected: false
    }
  ];

  constructor(private router: Router, private userService: UserService, private translate: TranslationService){
    this.userService.userDetails.subscribe(res => {
      this.user = res;
      this.translate.useTranslation('settings');
      let lang = this.languages.findIndex(el => el.acronym == res.language);
      if(lang != -1){
        this.languages[lang].selected = true;
      }
    }) 
  }
  setAllLanguagesUnselected() {
    this.languages.forEach(language => {
        language.selected = false;
    });
}

  selectLanguage(index: number){
    this.setAllLanguagesUnselected();
    this.indexLang = index;
    this.languages[index].selected = true;
  }
  saveLanguage(){
    this.user.language = this.languages[this.indexLang].acronym;
    this.userService.editProfile(this.user).subscribe(res => {
      if(!res.hasError){
        this.router.navigate(['/settings'])
      }
    });
  }
}
