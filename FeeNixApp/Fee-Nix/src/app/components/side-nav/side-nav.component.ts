import { Component, Input, OnChanges, SimpleChanges, Output, EventEmitter, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { environment } from '../../../environments/environment';
import { protectedResources } from '../../auth/configuration';
import { SelService } from '../../services/sel/sel.service';
import { MenuService } from '../../services/menu/menu.service';
import { IAppList, ImenuList } from '../../services/sel.interface';
import { CommonModule } from '@angular/common';
import { NoDataComponent } from '../no-data/no-data.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

@Component({
    selector: 'app-side-nav',
    standalone: true,
    imports: [CommonModule, NoDataComponent, MatAutocompleteModule, RouterModule, MatIconModule],
    templateUrl: './side-nav.component.html',
    styleUrls: ['./side-nav.component.scss'],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SideNavComponent implements OnChanges {
    @Input() menustatus: boolean = false;
    @Input() selectedApp: IAppList | undefined;
    @Output() menuActiveStatus = new EventEmitter<boolean>();
    menuListStatus: boolean = false;
    menuList: ImenuList[] = []
    selectedSubMenu: any = null;

    constructor(
        private selService: SelService,
        private menuService: MenuService
    ) { }

    ngOnChanges(changes: SimpleChanges) {
        changes['menustatus'] ? this.selectedSubMenu = changes['menustatus'].currentValue ? null : null : '';
        changes['selectedApp'] ? this.getMenuList() : '';
        console.log(changes);
    }

    getMenuList() {
        if (this.selectedApp) {
            this.menuListStatus = true;
            let url: string = `${environment.apiURL}${protectedResources.MenuList.resourceUrl}/${this.selectedApp.personaAppName}/${this.selectedApp.personalID}`;
            this.selService.getData(url).subscribe({
                next: (response: any) => {
                    if (response && response.length > 0) {
                        this.menuList = response;
                        this.menuList.map(val => val.url = val.subMenu.length > 0 && val.subMenu[0].menu.length > 0
                            ? val.subMenu[0].menu[0].url.slice(0, val.subMenu[0].menu[0].url.indexOf("/"))
                            : '');
                        this.menuService.callComponentMethod(this.menuList);
                    }
                    else {
                        this.menuList = [];
                        this.menuService.callComponentMethod('Menu not available');
                    }
                    this.menuListStatus = false;
                    console.log(response, 999);
                },
                error: (err) => {
                    this.menuList = [];
                    this.menuService.callComponentMethod('Menu not available');
                    this.menuListStatus = false;
                    console.log(err, 999);
                }
            });
        }
    }

    subMenu(menu: any) {
        this.selectedSubMenu = null;
        setTimeout(() => {
            this.selectedSubMenu = menu ? menu : null;
            this.selectedSubMenu === null ? this.menuActiveStatus.emit(false) : '';
            console.log(this.selectedSubMenu);
        }, 300);
    }
}