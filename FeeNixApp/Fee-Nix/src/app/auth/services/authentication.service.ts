import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../environments/environment';
import { protectedResources } from '../configuration';
import { IUserDetail } from '../../services/sel.interface';
import { SnackBarService } from '../../services/snack-bar/snack-bar.service';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  public userSubject: BehaviorSubject<IUserDetail | null>;
  public user: Observable<IUserDetail | null>;

  constructor(
    private http: HttpClient,
    private snackBarService: SnackBarService
  ) {
    this.userSubject = new BehaviorSubject<IUserDetail | null>(null);
    this.user = this.userSubject.asObservable();
  }

  public get userValue() {
    return this.userSubject.value;
  }

  login(username: string, password: string) {
    return this.http
      .post<any>(`${environment.apiURL}/users/authenticate`, { username, password }, { withCredentials: true })
      .pipe(
        map(user => {
          this.userSubject.next(user);
          this.startRefreshTokenTimer();
          return user;
        })
      );
  }
  getUserDetail(appname: string, randomNumber: string, accessLevel: string) {
    return this.http.get<any>(`${environment.apiURL}${protectedResources.MenuNavigation.resourceUrl}/${randomNumber}/${accessLevel}`, { withCredentials: false })
      .pipe(map(user => {
        if (typeof user === 'string') {
          this.snackBarService.error(user, 'ok');
          return false;
        } else {
          this.userSubject.next(user);
          this.startRefreshTokenTimer();
          localStorage.setItem('sel-bill-user', JSON.stringify(user));
          return user;
        }
      }));
  }

  logout() {
    this.http.post<any>(`${environment.apiURL}${protectedResources.RevokeToken.resourceUrl}`, { token: this.userValue.jwtToken }, { withCredentials: true }).subscribe();

    this.stopRefreshTokenTimer();
    this.userSubject.next(null);
    localStorage.removeItem('sel-bill-user');
    // window.location.replace(`${environment.linkURL}Login.aspx?Status=L`);
    localStorage.clear();
    sessionStorage.clear();
    window.location.replace(`${environment.production ? window.location.origin : environment.redirectURL}/scsel/Login.aspx?Status=L`);
  }

  refreshToken() {
    return this.http.post<any>(`${environment.apiURL}${protectedResources.RefreshToken.resourceUrl}`, { token: this.userValue.jwtToken }, { withCredentials: true })
      .pipe(map(user => {
        this.userValue.jwtToken = user.jwtToken;
        localStorage.setItem('sel-bill-user', JSON.stringify(user));
        this.startRefreshTokenTimer();
        return user;
      }));
  }

private refreshTokenTimeout: any;

private startRefreshTokenTimer() {
    const jwtToken = this.userValue?.jwtToken;
    if (!jwtToken) return;

    const jwtData = JSON.parse(atob(jwtToken.split('.')[1]));
    const expiresIn = jwtData.exp * 1000 - Date.now() - 60000; // 1 minute before expiry

    this.refreshTokenTimeout = setTimeout(() => {
      this.refreshToken().subscribe();
    }, expiresIn);
}

}