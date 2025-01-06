import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

import { TranslationService } from 'src/app/shared/services/translation.service';
import { UserDetails } from 'src/app/shared/services/user/user.model';
import { UserService } from 'src/app/shared/services/user/user.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent {
  @ViewChild('fileInput') fileInput!: ElementRef;

  editForm!: FormGroup;
  userData!: UserDetails;
  image: string = "";

  constructor(private userService: UserService, private router: Router, private translate: TranslationService){
    this.translate.useTranslation('settings');
    this.userService.userDetails.subscribe(res =>{
      this.userData = res;
      this.image = res.image;
       this.editForm = new FormGroup({
          name: new FormControl(res.fullName),
          phone: new FormControl(res.phone),
          email: new FormControl(res.email),
          birthDate: new FormControl(res.birthDate)
      });
    })

  }
  triggerFileInput() {
    this.fileInput.nativeElement.click(); 
  }
  saveData(){
    let value = this.editForm.value;
    let data: UserDetails = {
      fullName: value.name,
      phone: value.phone,
      id: this.userData.id,
      image: this.image,
      email: value.email,
      birthDate: value.birthDate,
      passwordHash: this.userData.passwordHash,
      role: this.userData.role,
      language: this.userData.language,
      spendingLimit: this.userData.spendingLimit,
      totalBalance: this.userData.totalBalance,
      historyTransfers: this.userData.historyTransfers
    }

    this.userService.editProfile(data).subscribe(res =>{
      if(res){
        this.userService.updateUserDetails(data);
        this.router.navigate(['/profile']);
      }else{

      }
    })
  }
  convertToBase64(event: any): void {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onloadend = () => {
      const base64String: string = reader.result as string;
      this.image = base64String;
    };
    if (file)
      reader.readAsDataURL(file);
  }
}
