import { Component } from '@angular/core';

import { RequestsService } from 'src/app/shared/services/requests/requests.service';
import { TranslationService } from 'src/app/shared/services/translation.service';
import { UserService } from 'src/app/shared/services/user/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {
  notifications = 99;
  userData: {name: string, image: string} = {name: '', image: ''};

  constructor(private userService: UserService, private translate: TranslationService, 
    private requestsService: RequestsService
  ){
    this.translate.useTranslation('settings');
    this.userService.userDetails.subscribe(res =>{
      this.userData.name = res.fullName;
      this.userData.image = res.image;
    })
    this.requestsService.getNumberOpened().subscribe(res => this.notifications = res)
  }
}
