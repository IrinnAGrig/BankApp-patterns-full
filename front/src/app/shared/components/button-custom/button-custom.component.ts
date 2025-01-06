import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-button-custom',
  templateUrl: './button-custom.component.html',
  styleUrls: ['./button-custom.component.css']
})
export class ButtonCustomComponent {
  @Input() text: string = '';
  @Input() invalid: boolean = false;
}
