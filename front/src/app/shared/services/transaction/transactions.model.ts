export interface TransactionBuy{
    id: string;
    type: 'Buy'| 'Transfer';
    title: string;
    serviceId: string;
    subtitle: string;
    emblem: string;
    sum: number;
    valute: string;
    date: string;
}
export interface TransactionDTO {
    type: string;
    receiverId: string;
    senderId: string;
    title: string;
    subtitle: string;
    emblem: string;
    sum: number;
    valute: string;
}
export interface TransactionBuyDto {
    senderId: string;
    sum: number;
    valute: string;
    cardId: string;
    serviceId: string;
  }
  
export interface ServicesDTO {
    idService: string;
    title: string;
    subtitle: string;
    emblem: string;
}
export interface TransactionShow{
    id: string;
    type: string;
    sum: number;
    valute: string;
    date: string;
    cardId: string;
    title: string;
    subtitle: string;   
    emblem: string; 
}

// export interface TransactionDTO{
//     type: 'Buy' | 'Transfer';
//     info: TransactionTransfer | TransactionBuy;
// }
// export interface TransactionTransfer{
//     receiverId: string;
//     senderId: string;
//     title: string;
//     subtitle: string;
//     emblem: string;
//     sum: number;
//     valute: string;
// }
// export interface TransactionBuy{
//     receiverId: string;
//     senderId: string;
//     title: string;
//     subtitle: string;
//     emblem: string;
//     sum: number;
//     valute: string;
// }
