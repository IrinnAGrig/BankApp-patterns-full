export interface Card {
    id: string;
    ownerId: string;
    cardNumber: string;
    nameHolder: string;
    expiryDate: string;
    cvv: string;
    type: string; // Silver, Gold, Platinum
    typeCard: string; // Debit sau Credit
    network: string; // Visa, MasterCard, etc.
    balance: number;
    valute: string;
    issuedDate: string; // DateTime in C#
    isActive: boolean; // bool in C#
    stateName: string; // optional, dacă nu e folosit în frontend
}



export interface CardDTO {
    ownerId: string;
    cardNumber: string;
    nameHolder: string;
    expiryDate: string; // Poate fi "Date" dacă îl parsezi ulterior
    cvv: string;
    type: string;
    typeCard: string;
    network: string;
    balance: number; // Match cu C#
    valute: string;
  }
  