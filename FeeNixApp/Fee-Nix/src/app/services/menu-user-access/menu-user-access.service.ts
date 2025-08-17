import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class MenuUserAccessService {
    private messageSource = new BehaviorSubject<boolean>(true);
    currentMessage = this.messageSource.asObservable();

    private componentMethodCallSource = new Subject<boolean>();
    componentMethodCalled$ = this.componentMethodCallSource.asObservable();

    constructor() {}

    changeMessage(message: boolean) {
        this.messageSource.next(message);
    }

    callComponentMethod(message: boolean) {
        this.componentMethodCallSource.next(message);
        this.changeMessage(message);
    }
}