import { Injectable } from "@angular/core";
import { environment } from "../../../../assets/environment/environment";
import { BehaviorSubject, map, Observable, switchMap, take, tap } from "rxjs";
import { HttpClient } from "@angular/common/http";

import { UserService } from "../user/user.service";
import { RequestsModel } from "./requests.model";
import { NotificationService } from "../notification.service";


@Injectable({ providedIn: 'root' })
export class RequestsService {
    url = environment.apiUrl + '/' + 'requests';
    private idUser: string = "";
    activeRequests = 0;
    private requests: RequestsModel[] = []; // Lista de notificări
    private requestsSubject = new BehaviorSubject<RequestsModel[]>(this.requests); // Observable pentru notificări
    constructor( private http: HttpClient, 
      private userService: UserService,
    private notificationService: NotificationService
  ) { 
      this.userService.userDetails.subscribe(res => {
        this.idUser = res.id;
      })
    }
    addTransaction(tr: RequestsModel): Observable<boolean> {
      tr.senderId = this.idUser;
      
      return this.http.post<boolean>(this.url, tr).pipe(tap(res => {
        if(res){
          this.notificationService.addNotificationsNumber(1);
        }
      }));
  }
    // addTransaction(tr: RequestsModel): Observable<boolean> {
    //     tr.senderId = this.idUser;
    //     return this.http.get<UserDetails>(`${this.userService.url}/${tr.phone}`)
    //         .pipe(
    //             switchMap(userDetails => {
    //                 tr.receiverId = userDetails  ? userDetails.id : 'no';
    //                 return this.http.post<boolean>(this.url, tr);
    //             })
    //         );
    // }
    updateRequests(request: RequestsModel): Observable<boolean>{
      return this.http.put<boolean>(`${this.url}/${request.id}`, request);
    }
    
    getRequestsByIdUser(): Observable<RequestsModel[]> {
        return this.http.get<RequestsModel[]>(`${this.url}/user/${this.idUser}`).pipe(
          map(data => data.reverse())
        );
      }
      getNumberOpened(): Observable<number> {
        return this.http.get<RequestsModel | RequestsModel[]>(`${this.url}/user/${this.idUser}/opened`).pipe(
          map(requests => 
            Array.isArray(requests) ? requests.length : 0
          ), 
          tap(res => console.log(res))
        );
      }
      
}