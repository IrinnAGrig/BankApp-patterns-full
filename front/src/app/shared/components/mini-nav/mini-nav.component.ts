import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-mini-nav',
  templateUrl: './mini-nav.component.html',
  styleUrls: ['./mini-nav.component.css']
})
export class MiniNavComponent {
  @Input() title = '';
  @Input() image = '';
  @Input() action = '';
  @Input() backAction = '';

  constructor(private router: Router){}
  
  goToLocation(){
    this.router.navigate(['/' + this.backAction]);
  }
}
