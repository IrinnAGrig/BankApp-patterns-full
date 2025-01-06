import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'transformCardNumber'
})
export class TransformCardNumberPipe implements PipeTransform {

  transform(value: string): string {
    if (!value) return value;
    let newstr = '';
    value = value.replace(/\s+/g, '');
    for (let i = 0; i < value.length; i += 4) {
      newstr += value.slice(i, i + 4) + ' ';
    }
    let crypted = '';
    if(newstr.length == 20){
      for (let i = 0; i < newstr.length; i++) {
        if(i<14 && newstr[i] != ' ' ){
           crypted += '*';
        }else if(i<14 && newstr[i] == ' ' ){
          crypted += ' ';
        }else{
          crypted += newstr[i];
        }
      }
    }else{
      crypted = newstr;
    }
      
    return  crypted.trim(); 
  }
}