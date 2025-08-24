export interface IUserDetail {
    id: number;
    firstName: any;
    lastName: any;
    username: string;
    jwtToken: string;
    userID: string;
    staffid: number;
    defaultSiteId: number;
    orgId: number;
    accessLevel: number;
    admincred: string;
    loginHTD: string;
    sessionYearTxt: string;
    toApps: string;
}

export interface IAppList {
    personalID: number;
    personaName: string;
    personaAppName: string;
    appIcon: string;
    appSURL: string;
    navigateURL: string;
    activeStatus?: boolean;
    color?: string;
}

export interface IBreadcrumb {
label : string;
url : string;
}

export interface ImenuList {
    id :number;
    name:string;
    url : string;
    imgUrl : string;
    subMenu : IsubMenuList[];
}

export interface IsubMenuList {
   title:string;
   menu: ISubMenu[];
}

export interface ISubMenu {
    id : number;
    name:string;
    url:string;
}
export interface IuserSelect {
    userSysId: number;
    academicYear: any;
    userID: string;
    staffid: number;
    userName: string;
    defaultSiteId: number;
    category: string;
    userPassword: any;
    orgId: number;
    orgName: any;
    adsServerName: any;
    staffNumber: any;
    locationId: any;
    displayName: any;
    siteName: any;
    roleNames: any;
    roleIDs: any;
    org_Logo: any;
    toApps: any;
    personalD: any;
    admincred: any;
    loginHID: any;
    sessionYearTxt: any;
    logdecimalype: any;
    accessLevel: any;
    personaName: any;
    defaultPersona: any;
    consultantId: any;
    randNo: any;
}

export interface IYear {
    yearNumber: number;
    appraisalYear: string;
    yearStart: string;
    yearEnd: string;
    yearActiveFlag: string;
}