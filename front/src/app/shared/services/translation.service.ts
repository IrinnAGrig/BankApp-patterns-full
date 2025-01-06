import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { UserService } from './user/user.service';

@Injectable({
  providedIn: 'root'
})
export class TranslationService {
  constructor(private translate: TranslateService, private http: HttpClient, private userService: UserService) {}

  private createLoader(prefix: string) {
    return new TranslateHttpLoader(this.http, `./assets/i18n/${prefix}/`, '.json');
  }

  useTranslation(prefix: string) {
    const loader = this.createLoader(prefix);
    this.userService.userDetails.subscribe(res => {
         loader.getTranslation(res.language).pipe(
      map(translations => {
        this.translate.setTranslation(res.language, translations, true);
        this.translate.use(res.language);
      }),
      catchError(error => {
        console.error('Translation loading error', error);
        return of({});
      })
    ).subscribe();
    })
  }
}
