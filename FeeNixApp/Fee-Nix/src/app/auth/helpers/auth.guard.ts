import { AuthenticationService } from '../services';
import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { first } from 'rxjs/operators';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, CanActivate } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const user = this.authenticationService.userValue;
        if (user) {
            return true;
        }
        else {
            var getURL = window.location.href;
            var urlValue = new URL(getURL);
            debugger;

            var appName = "";
            var randomNumber = "";
            var accessLevel = "";

            if (appName && randomNumber && accessLevel) {
                this.authenticationService.getUserDetail(appName, randomNumber, accessLevel)
                .pipe(first())
                    .subscribe({
                        next:(user) => { 
                            if (user) {
                                return true;
                            }
                            else {
                                localStorage.removeItem('sel-bill-user');
                                localStorage.clear();
                                sessionStorage.clear();
                                window.location.replace(`${environment.production ? window.location.origin : environment.redirectURL}/feenix/Login.aspx?Status=1`);
                                return false;
                            }
                        },
                       error: error => {
                            localStorage.removeItem('sel-bill-user'); 
                            localStorage.clear();
                            sessionStorage.clear();
                            window.location.replace(`${environment.production ? window.location.origin : environment.redirectURL}/feenix/Login.aspx?Status=1`);
                            return false;
                        }
            });
            }
            else {
                let checkUserDetail = JSON.parse(localStorage.getItem('sel-bill-user'));
                if (checkUserDetail) {
                    this.authenticationService.userValue === null? this.authenticationService.userSubject.next(checkUserDetail):'';
                    return true;
                }
                else {
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
