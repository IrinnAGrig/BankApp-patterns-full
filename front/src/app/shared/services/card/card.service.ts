import { Injectable } from "@angular/core";
import { catchError, map, Observable, of, switchMap} from "rxjs";
import { HttpClient, HttpParams } from "@angular/common/http";

import { Card, CardDTO } from "./card.model";
import { UserService } from "../user/user.service";
import { environment } from "../../../../assets/environment/environment";

@Injectable({ providedIn: 'root' })
export class CardService {
    url = environment.apiUrl + '/' + 'Card';
    private idUser = '';

    constructor( private http: HttpClient, private userService: UserService ) { 
        this.userService.userDetails.subscribe(res => this.idUser = res.id);
    }

    addCard(card: CardDTO): Observable<boolean>{
        card.valute = '$';
        return this.http.post<boolean>(this.url, card);
    }
    updateCard(card: Card): Observable<boolean>{
        return this.http.post<boolean>(this.url + '/update/' + card.id, card);
    }
    getMoneyOnAnArbitraryCard(amount: number): Observable<boolean> {
        return this.getCardsByIdUser().pipe(
            switchMap(res => {
                let card: Card = res[0];
                console.log(card)
                res[0].balance += amount;
                return this.updateCard(res[0]);
            }),
            map(() => true),
            catchError(() => of(false))
        );
    }

    getCardsByIdUser(): Observable<Card[]>{
        return this.http.get<Card[]>(this.url + '/' + this.idUser);
    }
     findCardByNumberAndName(cardNr: string): Observable<Card> {
          const params = new HttpParams().set('cardNumber', cardNr);
          return this.http.get<Card>(this.url + '/find', { params });
     }
}