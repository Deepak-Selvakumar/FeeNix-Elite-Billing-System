import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { environment } from '../../../environments/environment';
import { MenuService } from '../services/menu.service';
import { MenuUserAccessService } from '../services/menu-user-access/menu-user-access.service';
import { SnackBarService } from '../services/snack-bar/snack-bar.service';
import { IMenuList } from '../services/sel.interface';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  menuList: IMenuList[] = [];

  constructor(
    private menuService: MenuService,
    private menuUserAccessService: MenuUserAccessService,
    private snackBarService: SnackBarService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.menuService.currentMessage.subscribe(message => {
      if (message && typeof message === 'string') {
        this.menuUserAccessService.callComponentMethod(false);
        this.snackBarService.error(message, 'Ok', 4000);

        setTimeout(() => {
          localStorage.removeItem('sel-bill-user');
          localStorage.clear();
          sessionStorage.clear();
          window.location.replace(
            `${environment.production ? window.location.origin : environment.redirectURL}/scsel/Login.aspx?status=L`
          );
        }, 1000);

        return false;
      } 
      else if (message && typeof message === 'object') {
        this.menuList = message;

        let getURL =
          state.url.indexOf("?") > 0
            ? state.url.slice(1, state.url.indexOf("?"))
            : state.url.slice(1);

        // Check if the current URL exists in the menu permissions
        let menuStatus: boolean = this.menuList.some(val =>
          val.subMenu.menu.some(menulist => menulist.url === getURL)
        );

        if (menuStatus) {
          this.menuUserAccessService.callComponentMethod(true);
          return true;
        } else {
          this.menuUserAccessService.callComponentMethod(false);
          this.snackBarService.error('You do not have access to this screen', 'Ok', 4000);

          setTimeout(() => {
            localStorage.removeItem('sel-bill-user');
            localStorage.clear();
            sessionStorage.clear();
            window.location.replace(
              `${environment.production ? window.location.origin : environment.redirectURL}/scsel/Login.aspx?status=L`
            );
          }, 1000);

          return false;
        }
      }
    });

    return true;
  }
}
