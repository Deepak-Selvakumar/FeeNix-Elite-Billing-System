// import { AuthenticationService } from '../services';
// import { environment } from '../../environments/environment';
// import { Injectable } from '@angular/core';
// import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';

// @Injectable({ providedIn: 'root' })
// export class AuthGuard implements CanActivate {
//     constructor(
//         private router: Router,
//         private authenticationService: AuthenticationService
//     ) { }

//     canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
//         const user = this.authenticationService.userValue;
//         if (user) {
//             return true;
//         }
//         else {
//             var getURL = window.location.href;
//             var urlValue = new URL(getURL);
//             debugger;

//             // var appName = urlValue.searchParams.get("Toapp");
//             // var randomNumber = urlValue.searchParams.get("RndNo");
//             // var accessLevel = urlValue.searchParams.get("AccessLevel");
//             var appName = "nI9yW7gqDAp87hYjOUV8sPOF";
//             var randomNumber = "2";
//             var accessLevel = "1";

//             if (appName && randomNumber && accessLevel) {
//                 this.authenticationService.getUserDetail(appName, randomNumber, accessLevel)
//                     .subscribe(
//                         user => {
//                             if (user) {
//                                 return true;
//                             }
//                             else {
//                                 localStorage.removeItem('esl-bill-user');
//                                 window.location.replace(`${environment.linkURL}Login.aspx?Status=1`);
//                                 localStorage.clear();
//                                 window.location.replace(`${environment.production ? window.location.origin : environment.redirectURL}/scms/Login.aspx?Status=1`);
//                                 return false;
//                             }
//                         },
//                         error => {
//                             localStorage.removeItem('esl-bill-user');
//                             window.location.replace(`${environment.linkURL}Login.aspx?Status=1`);
//                             localStorage.clear();
//                             window.location.replace(`${environment.production ? window.location.origin : environment.redirectURL}/scms/Login.aspx?Status=1`);
//                             return false;
//                         }
//                     );
//             }
//             else {
//                 let checkUserDetail = JSON.parse(localStorage.getItem('esl-bill-user'));
//                 if (checkUserDetail) {
//                     this.authenticationService.userValue = null;
//                     this.authenticationService.userSubject.next(checkUserDetail);
//                     return true;
//                 }
//                 else {
//                     localStorage.removeItem('esl-bill-user');
//                     window.location.replace(`${environment.linkURL}Login.aspx?Status=1`);
//                     localStorage.clear();
//                     window.location.replace(`${environment.production ? window.location.origin : environment.redirectURL}/scms/Login.aspx?Status=1`);
//                     return false;
//                 }
//             }
//         }
//     }
// }
