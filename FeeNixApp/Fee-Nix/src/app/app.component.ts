import { Component, OnInit } from '@angular/core';
import { SwUpdate } from '@angular/service-worker';
import { AppUpdateComponent } from './components/app-update/app-update.component';
import { AuthenticationService } from './auth/services';
import { environment } from '../environments/environment';
import { RouterOutlet } from '@angular/router';
import { NoDataComponent } from './components/no-data/no-data.component';
import { SideNavComponent } from './components/side-nav/side-nav.component';
import { HeaderComponent } from './components/header/header.component';
import { IAppList, IUserDetail } from './services/sel.interface';
import { BreadcrumbListComponent } from './components/breadcrumb-list/breadcrumb-list.component';
import { YearComponent } from './components/year/year.component';
import { CommonModule } from '@angular/common';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { MenuUserAccessService } from './services/menu-user-access/menu-user-access.service';

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [
        CommonModule,
        RouterOutlet,
        NoDataComponent,
        SideNavComponent,
        HeaderComponent,
        BreadcrumbListComponent,
        YearComponent
    ],
    templateUrl: './app.component.html',
    styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
    title = 'Stabil.App';

    menuStatus: boolean = false;
    userDetail: IUserDetail | null = null;
    selectedApp: IAppList | null = null;
    menuUserAccess: boolean = false;

    constructor(
        private updates: SwUpdate,
        private bottomSheet: MatBottomSheet,
        private authenticationService: AuthenticationService,
        private menuUserAccessService: MenuUserAccessService
    ) { }

    ngOnInit(): void {
        this.authenticationService.user.subscribe(user => {
            this.userDetail = user ? user : null;
            //console.log(user);
        });
        
        this.menuUserAccessService.currentMessage.subscribe((message: boolean) => {
            this.menuUserAccess = message;
            //console.log(this.menuUserAccess);
        });
        
        this.appUpdate();
    }

    receiveMenuStatus($event: boolean) {
        this.menuStatus = $event;
    }

    receiveAppStatus($event: IAppList) {
        console.log($event);
        this.selectedApp = $event;
    }

    appUpdate() {
        if (environment.production) {
            if (this.updates.isEnabled) {
                this.updates.versionUpdates.subscribe(evt => {
                    switch (evt.type) {
                        case 'VERSION_DETECTED':
                            console.log(`Downloading new app version: ${evt.version.hash}`);
                            break;
                        case 'VERSION_READY':
                            const bottomSheetRef = this.bottomSheet.open(AppUpdateComponent, {
                                data: 'New app version available to update please confirm'
                            });
                            
                            bottomSheetRef.afterDismissed().subscribe((data: string) => {
                                if (data) {
                                    document.location.reload();
                                }
                            });
                            
                            console.log(`Current app version: ${evt.currentVersion.hash}`);
                            console.log(`New app version ready for use: ${evt.latestVersion.hash}`);
                            break;
                        case 'VERSION_INSTALLATION_FAILED':
                            console.log(`Failed to install app version ${evt.version.hash}: ${evt.error}`);
                            break;
                    }
                });
            }
        } else {
            if (window.navigator && navigator.serviceWorker) {
                navigator.serviceWorker.getRegistrations()
                    .then(function(registrations) {
                        for (let registration of registrations) {
                            registration.unregister();
                        }
                    });
            }
        }
    }
}