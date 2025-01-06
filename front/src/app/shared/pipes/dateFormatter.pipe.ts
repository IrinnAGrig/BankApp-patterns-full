import { Pipe, PipeTransform } from '@angular/core';
import { formatDate } from '@angular/common';

@Pipe({
  name: 'dateFormatter'
})
export class DateFormatterPipe implements PipeTransform {

  transform(value: string | Date, format: string = 'dd.MM.yyyy'): string {
    if (!value) {
      return '';
    }

    const date = new Date(value);
    return formatDate(date, format, 'en-US'); // Folosește 'en-US' pentru formatul dorit
  }

}
