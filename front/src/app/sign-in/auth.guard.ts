import {
    ActivatedRouteSnapshot,
    Router,
    UrlTree,
    CanActivateChild
} from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

import { UserService } from '../shared/services/user/user.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivateChild {
    constructor(private userService: UserService, private router: Router) {}

    canActivateChild(
        route: ActivatedRouteSnapshot,
    ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        return this.userService.userDetails.pipe(
            take(1),
            map(user => {
               
                if (!user || user.id === '') {
                    if (this.router.url !== '/sign-in') {
                        return true;
                    }else{
                        return this.router.createUrlTree(['/sign-in']);
                    }
                }

                if (user.id !== '' && this.router.url === '/sign-in') {
                    return true;
                }
                return true;
            })
        );
    }
}
