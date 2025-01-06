import { Component, EventEmitter, HostListener, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';

@Component({
  selector: 'app-range-slider',
  templateUrl: './range-slider.component.html',
  styleUrls: ['./range-slider.component.css']
})
export class RangeSliderComponent implements OnInit, OnChanges {
  @Input() min: number = 0;
  @Input() max: number = 100;
  @Input() value: number = 50;
  @Output() valueChange: EventEmitter<number> = new EventEmitter<number>();

  dragging = false;
  fillWidth = 0;
  thumbPosition = 0;

  ngOnInit() {
    this.updateSlider(this.value);
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['value'] ) {
      this.updateSlider(this.value);
    }
  }

  startDrag(event: MouseEvent | TouchEvent) {
    this.dragging = true;
    this.updatePosition(this.getClientX(event));
  }

  stopDrag() {
    this.dragging = false;
  }

  @HostListener('document:mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {
    if (this.dragging) {
      this.updatePosition(event.clientX);
    }
  }

  @HostListener('document:mouseup')
  onMouseUp() {
    this.stopDrag();
  }

  @HostListener('document:touchmove', ['$event'])
  onTouchMove(event: TouchEvent) {
    if (this.dragging) {
      this.updatePosition(this.getClientX(event));
    }
  }

  @HostListener('document:touchend')
  onTouchEnd() {
    this.stopDrag();
  }

  updatePosition(clientX: number) {
    const slider: HTMLElement = document.querySelector('.slider')!;
    const rect = slider.getBoundingClientRect();
    const offsetX = clientX - rect.left;
    const width = rect.width;

    let percentage = (offsetX / width) * 100;
    percentage = Math.max(0, Math.min(percentage, 100)); // Clamp between 0 and 100
    this.value = (percentage / 100) * (this.max - this.min) + this.min;
    this.updateSlider(this.value);
    this.valueChange.emit(Number(this.value.toFixed(0)));
  }

  updateSlider(value: number) {
    this.fillWidth = ((value - this.min) / (this.max - this.min)) * 100;
    this.thumbPosition = this.fillWidth - 1;
  }

  private getClientX(event: MouseEvent | TouchEvent): number {
    if (event instanceof MouseEvent) {
      return event.clientX;
    } else if (event instanceof TouchEvent && event.touches.length > 0) {
      return event.touches[0].clientX;
    }
    return 0;
  }
}