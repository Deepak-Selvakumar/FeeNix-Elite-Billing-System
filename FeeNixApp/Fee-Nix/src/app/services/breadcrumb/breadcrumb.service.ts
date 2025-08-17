import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { IBreadcrumb } from '../services/sel.interface';

@Injectable({
    providedIn: 'root'
})
export class BreadcrumbService {
    private messageSource = new BehaviorSubject<IBreadcrumb[] | null>(null);
    currentMessage = this.messageSource.asObservable();

    private componentMethodCallSource = new Subject<IBreadcrumb[]>();
    componentMethodCalled$ = this.componentMethodCallSource.asObservable();

    constructor() {}

    changeMessage(message: IBreadcrumb[]) {
        this.messageSource.next(message);
    }

    callComponentMethod(message: IBreadcrumb[]) {
        this.componentMethodCallSource.next(message);
        this.changeMessage(message);
    }
}