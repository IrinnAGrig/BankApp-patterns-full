import { Component } from '@angular/core';
import { Router } from '@angular/router';

interface StepIntro {
  numberStep: number;
  imagePath: string;
  title: string;
  description: string;
}
@Component({
  selector: 'app-intro',
  templateUrl: './intro.component.html',
  styleUrls: ['./intro.component.css'],
})
export class IntroComponent {
  activeIntro = false;
  currentStep = 0;
  isIntro = false;

  steps: StepIntro[] = [{
    numberStep: 1,
    imagePath: '../assets/images/Intro-1.svg',
    title: 'Fastest Payment in the world',
    description: 'Integrate multiple payment methoods to help you up the process quickly'
  }, {
    numberStep: 2,
    imagePath: '../assets/images/Intro-2.svg',
    title: 'The most Secoure Platfrom for Customer',
    description: 'Built-in Fingerprint, face recognition and more, keeping you completely safe'
  }, {
    numberStep: 3,
    imagePath: '../assets/images/Intro-3.svg',
    title: 'Paying for Everything is Easy and Convenient',
    description: 'Built-in Fingerprint, face recognition and more, keeping you completely safe'
  }];

  constructor(private router: Router) {
    if (localStorage.getItem('isIntro') !== null) {
      let val = localStorage.getItem('isIntro');
      if(val){ 
        this.isIntro = JSON.parse(val);
      }else{
        this.isIntro = false;
      }
    } else {
      localStorage.setItem('isIntro', JSON.stringify(false));
      this.isIntro = false;
    }

    setTimeout(() => {
      if(!this.isIntro){ 
        this.activeIntro = false;
        this.router.navigate(['/sign-in']);
      }else{
        this.activeIntro = true;
        localStorage.setItem('isIntro', JSON.stringify(false));
      }
    }, 3000);
  
  }
  nextStep() {
    this.currentStep++;
    if (this.currentStep == 3) {
      this.router.navigate(['/sign-in'])
    }
  }
}
