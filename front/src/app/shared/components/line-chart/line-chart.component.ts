import { Component, AfterViewInit, Output, EventEmitter, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Chart } from 'chart.js/auto';

import { TransactionShow } from '../../services/transaction/transactions.model';

@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css']
})
export class LineChartComponent implements AfterViewInit, OnChanges {
  @Input() transactions: TransactionShow[] = [];
  @Output() month = new EventEmitter<number | null>();

  months: string[] = [ 'Jan' , 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
  labels: string[] = [];

  private data: number[] = [];
  private chart: Chart | undefined;

  constructor() {
  }

  ngAfterViewInit(): void {
    this.createChart();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['transactions'] && this.transactions.length > 0) {
      this.updateChartData();
      this.updateChart();
    }
  }

  private getTransactionsForMonth(month: number): TransactionShow[] {
    return this.transactions.filter(transaction => {
      const date = new Date(transaction.date);
      return date.getMonth() === month - 1;
    });
  }

  private updateChartData(): void {
    this.data = [];
    this.labels = [];
    const currentMonthIndex = new Date().getMonth();
    const startIndex = (currentMonthIndex - 5 + 12) % 12;

    for (let i = 0; i < 6; i++) {
      this.labels.push(this.months[(startIndex + i) % 12]);
      let infoAmount = this.getTransactionsForMonth(startIndex + i + 1)
        .filter(transaction => transaction.sum < 0)
        .reduce((total, transaction) => total + transaction.sum, 0);
      this.data.push(infoAmount ? infoAmount * (-1) : 0);
    }
  }

  private createChart(): void {
    const ctx = document.getElementById('myChart') as HTMLCanvasElement;

    this.chart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: this.labels,
        datasets: [{
          data: this.data,
          fill: true,
          backgroundColor: 'rgba(0, 102, 255, 0.2)',
          borderColor: '#0066FF',
          tension: 0.4,
          pointRadius: 6,
          pointBackgroundColor: '#0066FF'
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            display: false
          }
        },
        scales: {
          x: {
            display: false,
            ticks: {
              padding: 10
            }
          },
          y: {
            display: false
          }
        }
      }
    });
  }

  private updateChart(): void {
    if (this.chart) {
      this.chart.destroy();
    }
    this.createChart();
  }

  private emitMonth(month: number): void {
    let labelIndex = this.months.findIndex(el => el == this.labels[month]);
    if (labelIndex !== -1) {
      this.month.emit(labelIndex);
    }
  }

  highlightButton(index: number): void {
    this.labels.forEach((label, i) => {
      const button = document.getElementById('btn-' + i);
      if (button) {
        button.style.backgroundColor = i === index ? 'rgb(0, 102, 255)' : '';
        button.style.color = i === index ? 'white' : '';
      }
    });
    this.emitMonth(index);
  }
}
