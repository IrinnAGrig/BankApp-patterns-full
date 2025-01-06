import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatPrice'
})
export class FormatPricePipe implements PipeTransform {
  transform(value: number | string, decimalPlaces: number = 2): string {
    if (value === null || value === undefined || isNaN(Number(value))) {
      return '';
    }

    const numberValue = typeof value === 'string' ? parseFloat(value) : value;

    const [integerPart, decimalPart] = numberValue.toFixed(decimalPlaces).split('.');

    const integerWithCommas = integerPart.replace(/\B(?=(\d{3})+(?!\d))/g, ' ');

    const formattedDecimalPart = (decimalPart || '00').padEnd(decimalPlaces, '0');

    return `${integerWithCommas}.${formattedDecimalPart}`;
  }
}
