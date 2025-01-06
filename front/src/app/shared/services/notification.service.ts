import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/assets/environment/environment';

export interface INotification {
    id: string;
    type: string;
    timestamp: string;
    message: string;
    availableBalance: number;
    emblem: string;
    isRead: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
    url = environment.apiUrl + '/' + 'transactions';
    private notifications: INotification[] = [];
    notificationsNumber = 0;
    private notificationsNumberSubject = new BehaviorSubject<number>(this.notificationsNumber); 
    private notificationsSubject = new BehaviorSubject<INotification[]>(this.notifications); // Observable pentru notificări
    notifications$ = this.notificationsSubject.asObservable();
    notificationsNumber$ = this.notificationsNumberSubject.asObservable();

  constructor() {}

  private addNotification(message: INotification): void {
    this.notifications.unshift(message); // Adaugă notificarea la începutul listei
    this.notificationsSubject.next(this.notifications); // Actualizează observable-ul
  }
  updateNotifications(message: INotification){
    console.log(message)
    this.addNotification(message);
  }
  updateNotificationsNumber(nr: number){
    this.notificationsNumberSubject.next(nr); 
    this.notificationsNumber = nr;
  }
  addNotificationsNumber(nr: number){
    this.notificationsNumber++;
    this.notificationsNumberSubject.next(this.notificationsNumber); 
  }
  // getNotificationsNumber(): Observable<number> {
  //   return this.http.get<INotification[]>(this.url + "/notif/" + this.idUser + "/All");
  // }
  // getTransactionsNotifications(): Observable<INotification[]> {
  //   return this.http.get<INotification[]>(this.url + "/notif/" + this.idUser + "/All");
  // }
}