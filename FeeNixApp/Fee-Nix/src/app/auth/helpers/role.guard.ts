import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { environment } from '../../../environments/environment';
import { MenuService } from '../../services/menu/menu.service';
import { MenuUserAccessService } from '../../services/menu-user-access/menu-user-access.service';
import { SnackBarService } from '../../services/snack-bar/snack-bar.service';
import { ImenuList } from '../../services/sel.interface';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  menuList: ImenuList[] = [];

  constructor(
    private menuService: MenuService,
    private menuUserAccessService: MenuUserAccessService,
    private snackBarService: SnackBarService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    let messageValue: string | ImenuList[] | null = null;
    this.menuService.currentMessage.subscribe(message => {
      messageValue = message;
    });

    if (messageValue && typeof messageValue === 'string') {
      this.menuUserAccessService.callComponentMethod(false);
      this.snackBarService.error(messageValue, 'Ok', 4000);

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
    else if (messageValue && typeof messageValue === 'object') {
      this.menuList = messageValue as ImenuList[];

      let getURL =
        state.url.indexOf("?") > 0
          ? state.url.slice(1, state.url.indexOf("?"))
          : state.url.slice(1);

      // Check if the current URL exists in the menu permissions
      let menuStatus: boolean = this.menuList.some(val =>
        val.subMenu.some(subMenu => subMenu.menu.some(menuList => menuList.url === getURL)))

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
    return true;
  }
}
