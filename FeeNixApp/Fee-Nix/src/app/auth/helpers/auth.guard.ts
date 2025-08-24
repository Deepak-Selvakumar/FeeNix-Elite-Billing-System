import { AuthenticationService } from '../services';
import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { first, map, catchError } from 'rxjs/operators';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, CanActivate } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Promise<boolean> | import("rxjs").Observable<boolean> {
        const user = this.authenticationService.userValue;
        if (user) {
            return true;
        } else {
            const getURL = window.location.href;
            const urlValue = new URL(getURL);
            debugger;

            const appName = "";
            const randomNumber = "";
            const accessLevel = "";

            if (appName && randomNumber && accessLevel) {
                return this.authenticationService.getUserDetail(appName, randomNumber, accessLevel)
                    .pipe(
                        first(),
                        // Map the observable result to boolean
                        // Use tap for side effects (redirects, clearing storage)
                        // Use catchError for error handling
                        // Import necessary rxjs operators at the top if not already
                        // import { map, catchError } from 'rxjs/operators';
                        map((user) => {
                            if (user) {
                                return true;
                            } else {
                                localStorage.removeItem('sel-bill-user');
                                localStorage.clear();
                                sessionStorage.clear();
                                window.location.replace(`${environment.production ? window.location.origin : environment.redirectURL}/feenix/Login.aspx?Status=1`);
                                return false;
                            }
                        }),
                        catchError((error) => {
                            localStorage.removeItem('sel-bill-user');
                            localStorage.clear();
                            sessionStorage.clear();
                            window.location.replace(`${environment.production ? window.location.origin : environment.redirectURL}/feenix/Login.aspx?Status=1`);
                            return [false];
                        })
                    );
            } else {
                const selBillUser = localStorage.getItem('sel-bill-user');
                let checkUserDetail = selBillUser ? JSON.parse(selBillUser) : null;
                if (checkUserDetail) {
                    if (this.authenticationService.userValue === null) {
                        this.authenticationService.userSubject.next(checkUserDetail);
                    }
                    return true;
                } else {
                    localStorage.removeItem('sel-bill-user');
                    localStorage.clear();
                    sessionStorage.clear();
                    window.location.replace(`${environment.production ? window.location.origin : environment.redirectURL}/scms/Login.aspx?Status=1`);
                    return false;
                }
            }
        }
    }
}
