import { Injectable } from "@angular/core";
import { forkJoin, map, Observable, take, tap,} from "rxjs";
import { HttpClient } from "@angular/common/http";

import { environment } from "../../../../assets/environment/environment";
import { ServicesDTO, TransactionBuy, TransactionBuyDto,TransactionShow, TransactionDTO } from "./transactions.model";
import { UserService } from "../user/user.service";
import { INotification } from "../notification.service";


@Injectable({ providedIn: 'root' })
export class TransactionService {
    url = environment.apiUrl + '/' + 'transactions';
    private idUser: string = "";

    constructor( private http: HttpClient, private userService: UserService) { 
      this.userService.userDetails.subscribe(res => {
        this.idUser = res.id;
      })
    }

    addTransactionBuy(tr: TransactionBuyDto): Observable<boolean>{
      tr.senderId = this.idUser;
      console.log(tr)
        return this.http.post<boolean>(this.url + `/add/transaction/buy`, tr);
    }
    addTransactionTransfer(tr: TransactionDTO): Observable<boolean>{
      tr.senderId = this.idUser;
        return this.http.post<boolean>(this.url + `/add`, tr);
    }
    getTransactionsBuy(): Observable<TransactionShow[]> {
      return this.http.get<TransactionShow[]>(this.url + "/" + this.idUser + "/Buy");
    }
    getTransactions(): Observable<TransactionShow[]> {
      return this.http.get<TransactionShow[]>(this.url + "/" + this.idUser + "/All");
    }
    getServices(): Observable<ServicesDTO[]> {
      return this.http.get<ServicesDTO[]>(this.url + "/services");
    }
    getTransactionsNotifications(): Observable<INotification[]> {
      return this.http.get<INotification[]>(this.url + "/notif/" + this.idUser + "/All");
    }
    getNotificationsNumber(): Observable<number> {
      return this.http.get<number>(this.url + "/notif/" + this.idUser + "/All/number");
    }
    getJustPartTransactions(nr: number): Observable<TransactionShow[]> {
      return this.getTransactionsBuy().pipe(
        take(1), 
        map(transactions => transactions.slice(0, nr))
      );
    }
}