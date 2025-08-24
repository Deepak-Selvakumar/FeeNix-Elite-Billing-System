import { Component, OnInit, Input, Output, EventEmitter, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { environment } from '../../../environments/environment';
import { AuthenticationService } from '../../auth/services';
import { protectedResources } from '../../auth/configuration';
import { SelService } from '../../services/sel/sel.service';
import { UserSelectService } from '../../services/user-select/user-select.service';
import { IUserDetail, IuserSelect, IAppList } from '../../services/sel.interface';
import { UserSelectComponent } from '../user-select/user-select.component';
import { CommonModule } from '@angular/common';
import { NoDataComponent } from '../no-data/no-data.component';
import { MatMenuModule } from '@angular/material/menu';

@Component({
    selector: 'app-header',
    standalone: true,
    imports: [CommonModule, NoDataComponent, MatMenuModule],
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss'],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class HeaderComponent implements OnInit {
    @Input() menuStatus: boolean = false;
    userDetail: IUserDetail | null = null;
    userSelected: IuserSelect | null = null;
    @Output() menuActiveStatus = new EventEmitter<boolean>();
    @Output() appActiveStatus = new EventEmitter<IAppList>();
    appList: IAppList[] = [];
    appListStatus: boolean = false;
    selectedApp: IAppList | null = null;
    appRedirectLoaderStatus: boolean = false;
    appRedirectText: string = '';
    profileLink: any[] = [
        { name: 'My Profile', icon: 'my-account.png', url: 'PersonalProfile/PersonalProfileHome.aspx' },
        { name: 'Change Photo', icon: 'my-account.png', url: 'PersonalProfile/ChangePhoto.aspx' },
        { name: 'Change Password', icon: 'logout.png', url: 'Home/move.aspx?PAccess=PG#ToApps=SGsel' },
        // { name: 'Select User', icon: 'my-account.png', url: 'userSelect' },
        { name: 'Logout', icon: 'logout.png', url: 'logout' }
    ];
    onlineUserCount: number | 0 = 0;

    constructor(
        public dialog: MatDialog,
        private authenticationService: AuthenticationService,
        private selService: SelService,
        private userSelectService: UserSelectService
    ) {}

    ngOnInit(): void {
        this.authenticationService.user.subscribe(user => {
            this.userDetail = user ? user : null;
            this.getAppLink();
            // console.log(user);
        });
    }

    getAppLink() {
        this.appListStatus = true;
        if (!this.userDetail) {
            this.appList = [];
            this.appListStatus = false;
            return;
        }
        let url: string = `${environment.apiURL}${protectedResources.PersonaList.resourceUrl}/${this.userDetail.id}/${this.userDetail.accessLevel}/${this.userDetail.defaultSiteId}/C`;
        this.selService.getData(url).subscribe({
            next: (response: any) => {
                if (response && response.value.length > 0) {
                    this.appList = response.value;
                    // this.appList.map(val => { val.appIcon = `${environment.linkURL}${val.appIcon.replace("../", '')}`; val.appSURL = val.appSURL.replace("../", ''); val.navigateURL = val.navigateURL.replace("../../", '') });
                    // this.appList.map(val => { val.appIcon = `${environment.linkURL}${val.appIcon.replace("../", '')}` });
                    this.appList.map(val => { val.appIcon = `${environment.production ? window.location.origin : environment.redirectURL}/scsel/${val.appIcon.replace("../../", '')}` });
                    this.appList = this.appList.map(val => ({ ...val, activeStatus: false, color: '' }));
                    this.appList.map(val => {
                        switch (val.personaName) {
                            case 'HelpDesk':
                                val.color = '#1d47fc';
                                break;
                            case 'Academics':
                                val.color = '#a23720';
                                break;
                            case 'SEL Bill':
                                val.color = '#ffaf00';
                                val.activeStatus = environment.production === false ? true : false;
                                environment.production === false ? this.selectedApp = val : '';
                                this.appActiveStatus.emit(val);
                                break;
                            default:
                                val.color = '#1d47fc';
                        }
                    });
                    this.appListStatus = false;
                    // console.log(this.appList, 999);
                } else {
                    this.appList = [];
                    this.appListStatus = false;
                }
                // console.log(response, 999);
            },
            error: (err) => {
                this.appList = [];
                this.appListStatus = false;
                console.log(err, 999);
            }
        });
    }

    getMenuStatus() {
        this.menuStatus = !this.menuStatus;
        this.menuActiveStatus.emit(this.menuStatus);
    }

    selectApp(data: IAppList) {
        this.appList.map(val => val.activeStatus = false);
        let findIndex = this.appList.findIndex(val => val.personaName === data.personaName);
        findIndex >= 0 ? this.appList[findIndex].activeStatus = true : '';
        // window.open(`${environment.linkURL}${data.url}`, "_blank");
        this.redirectApp(data);
    }

    redirectApp(toAppData: IAppList) {
        this.appRedirectText = `Redirect to ${toAppData.personaName} App, please wait...`;
        this.appRedirectLoaderStatus = true;
        let url: string = `${environment.apiURL}${protectedResources.MenuNavigation.resourceUrl}`;
        
        
                if (!this.userDetail || !this.selectedApp) {
            // Handle the case where userDetail or selectedApp is null
            this.appRedirectLoaderStatus = false;
            return;
        }
        let data: any = {
            userId: `${this.userDetail.id}`,
            fromMenuName: `${this.selectedApp.personaAppName}`,
            toMenuName: `${toAppData.personaAppName}`,
            siteId: `${this.userDetail.defaultSiteId}`,
            loginHistoryID: `${this.userDetail.loginHTD}`,
            isAdminCred: `${this.userDetail.admincred}`,
            sessionYearStr: `${this.userDetail.sessionYearTxt}`
        }; 
        // console.log(toAppData, url, data);
        this.selService.sendData(url, data).subscribe({
            next: (response: any) => {
                // this.appRedirectLoaderStatus = false;
                localStorage.removeItem('sel-bill-user');
                if (response.randomNumber && response.randomNumber.length > 0) {
                    const navigateURL = toAppData.navigateURL ? toAppData.navigateURL.replace("../../", "") : "";
                    let redirectURL: string = `${environment.production ? window.location.origin : environment.redirectURL}/${navigateURL}?RndNo=${response.randomNumber}&AccessLevel=${this.userDetail?.accessLevel}`;
                    window.location.replace(`${redirectURL}`);
                    // let redirectURL: string = `${environment.redirectURL}/${toAppData.navigateURL.replace("../../", "")}?RndNo=${response.randomNumber}&AccessLevel=${this.userDetail.accessLevel}`;
                
                    // console.log(redirectURL);
                } else {
                    // window.location.replace(`${environment.linkURL}Login.aspx?Status=L`);
                    localStorage.clear();
                    sessionStorage.clear();
                    window.location.replace(`${environment.production ? window.location.origin : environment.redirectURL}/scsel/Login.aspx?Status=L`);
                }
                // console.log(response, 999);
            },
            error: (err) => {
                // this.appRedirectLoaderStatus = false;
                localStorage.removeItem('sel-bill-user');
                // window.location.replace(`${environment.linkURL}Login.aspx?Status=L`);
                localStorage.clear();
                sessionStorage.clear();
                window.location.replace(`${environment.production ? window.location.origin : environment.redirectURL}/scsel/Login.aspx?Status=L`);
                console.log(err, 999);
            }
        });
    }

    openUserSelectDialog(): void {
        const dialogRef = this.dialog.open(UserSelectComponent, {
            width: '490px',
            data: this.userSelected,
            panelClass: 'popupSection'
        });
        dialogRef.afterClosed().subscribe(user => {
            this.userSelected = user;
            this.userSelectService.callComponentMethod(this.userSelected);
            // console.log(this.userSelected);
        });
    }

    openLink(url: string) {
        url === 'userSelect' ? this.openUserSelectDialog() : url === 'logout' ? this.logout() : window.open(`${environment.linkURL}${url}`, "_blank");
    }

    logout() {
        this.authenticationService.logout();
    }
}