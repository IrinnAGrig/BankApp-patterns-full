import { Component, AfterViewInit, EventEmitter, Output, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Chart } from 'chart.js/auto';
import { Config, polyfill } from 'mobile-drag-drop'; 

import { TransactionShow } from '../../services/transaction/transactions.model';

@Component({
  selector: 'app-chart-pie',
  templateUrl: './chart-pie.component.html',
  styleUrls: ['./chart-pie.component.css']
})
export class ChartPieComponent implements AfterViewInit, OnChanges {
  @Input() transactions: TransactionShow[] = [];
  @Output() closed = new EventEmitter<boolean>();

  screenHeight: number = 0;
  screenWidth: number = 0;
  chart: any;
  total: number = 0;
  isClosing = false;
  notOpen = false;
  selectedElement : {label: string; value: string;} = {label: 'ENTERTAIMENT', value: '55'};
  labels: string[] = ['ENTERTAIMENT', 'TRANSACTION', 'TRAVEL', 'SERVICES', 'SHOPPING', 'AUTOSERVICE'];
  data: number[] = [0, 0, 0, 0, 0, 0];
  
  private isDragging = false;
  private startY: number = 0;
  private threshold: number = 50; 

  constructor(){
    this.updateScreenSize();
    window.addEventListener('resize', this.updateScreenSize.bind(this));
  }
  
  ngAfterViewInit(): void {
    const config: Config = {
      dragImageSetup: (element: HTMLElement) => element, 
      holdToDrag: 200, 
      dragStartConditionOverride: (event: TouchEvent) => true,
      dragImageTranslateOverride: (event: TouchEvent, hoverCoordinates, hoveredElement, translateDragImageFn) => {
        translateDragImageFn(0, 0); 
      },
    };

    polyfill(config);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['transactions'] ) {
      if(this.transactions.length > 0){
        this.data = [0, 0, 0, 0, 0, 0];
        this.total = 0;
        this.setData();
        this.createChart();
      }
    }
  }

  setData(){
    this.transactions.forEach(el => {
      if (this.labels.findIndex(label => label === el.subtitle) !== -1) {
        let index = this.labels.findIndex(label => label === el.subtitle);
        this.data[index] += el.sum * (-1);
      }
    });
    this.total = this.data.reduce((accumulator, currentValue) => accumulator + currentValue, 0);
  }

  extendElement(){
    this.notOpen = true;
  }

  createChart() {
    const canvas = document.getElementById("MyChart") as HTMLCanvasElement;

    if (canvas) {
        const ctx = canvas.getContext("2d");

        if (ctx) {
            const gradient = ctx.createLinearGradient(0, 0, canvas.width, 0);
            gradient.addColorStop(0, 'rgba(255,148,109,1)');
            gradient.addColorStop(1, 'rgba(255,189,101,1)');
            const gradient1 = ctx.createLinearGradient(0, 0, canvas.width, 0);
            gradient1.addColorStop(0, 'rgba(189,253,200,1)');
            gradient1.addColorStop(1, 'rgba(100,229,195,1)');
            const gradient2 = ctx.createLinearGradient(0, 0, canvas.width, 0);
            gradient2.addColorStop(0, 'rgba(8,230,248,1)');
            gradient2.addColorStop(1, 'rgba(71,190,250,1)');
            const gradient3 = ctx.createLinearGradient(0, 0, canvas.width, 0);
            gradient3.addColorStop(0, 'rgba(245,114,163,1)');
            gradient3.addColorStop(1, 'rgba(213,162,233,1)');
            const gradient4 = ctx.createLinearGradient(0, 0, canvas.width, 0);
            gradient4.addColorStop(0, 'rgba(246,166,166,1)');
            gradient4.addColorStop(1, 'rgba(221,2,2,1)');
            const gradient5 = ctx.createLinearGradient(0, 0, canvas.width, 0);
            gradient5.addColorStop(0, 'rgba(234,246,163,1)');
            gradient5.addColorStop(1, 'rgba(236,255,0,1)');
            
            const data = {
                labels: this.labels,
                datasets: [{
                    data: this.data,
                    cutout: 55,
                    spacing: 15,
                    backgroundColor: [gradient, gradient1, gradient2, gradient3, gradient4, gradient5],
                    borderWidth: 0,
                    borderRadius: 20,
                    hoverOffset: 4
                }]
            };

            const options = {
                animation: {
                    duration: 0
                },
                aspectRatio: 2.5,
                cutout: '70%',
                plugins: {
                    legend: {  
                        display: false,
                    },
                    tooltip: {
                        enabled: false  
                    }
                },
                onClick: (event: any) => {
                    const elements = this.chart?.getElementsAtEventForMode(event, 'nearest', { intersect: true }, true);
                    if (elements.length) {
                        const index = elements[0].index;
                        const label = this.chart?.data.labels[index];
                        const value = this.chart?.data.datasets[0].data[index];
                        const total = this.chart?.data.datasets[0].data.reduce((acc: any, cur: any) => acc + cur, 0);
                        const percentage = total > 0 ? (value / total) * 100 : 0;
                        this.selectedElement.label = label;
                        this.selectedElement.value = percentage.toFixed(2);
                    }
                }
            };

            if (this.chart) {
                this.chart.data = data;
                this.chart.options = options;
                this.chart.update();
            } else {
                this.chart = new Chart(ctx, {
                    type: 'doughnut',
                    data: data,
                    options: options
                });
            }
        } else {
            console.error("Unable to get canvas context.");
        }
    } else {
        console.error("Canvas element not found.");
    }
  }

  onTouchStart(event: TouchEvent): void {
    if (this.isEventInTransactions(event)) return;
    this.isDragging = true;
    this.startY = event.touches[0].clientY; 
}

onTouchMove(event: TouchEvent): void {
    if (!this.isDragging || this.isEventInTransactions(event)) return;

    const currentY = event.touches[0].clientY;
    const distance = currentY - this.startY; 

    if (distance > this.threshold) {
        this.notOpen = false; 
        this.isClosing = true;
        setInterval(() => { this.closed.emit(false); }, 850);
    } else if (distance < -this.threshold) {
        this.notOpen = true;
    }
}

onTouchEnd(event: TouchEvent): void {
    if (this.isEventInTransactions(event)) return;
    this.isDragging = false;
}


private isEventInTransactions(event: TouchEvent): boolean {
    const target = event.target as HTMLElement;
    return target.closest('.transactions') !== null;
}

updateScreenSize() {
    this.screenHeight = window.innerHeight;
    this.screenWidth = window.innerWidth;
}
}
