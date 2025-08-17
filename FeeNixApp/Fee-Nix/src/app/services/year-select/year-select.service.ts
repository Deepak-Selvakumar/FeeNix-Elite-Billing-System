import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { IYear } from '../services/sel.interface';

@Injectable({
    providedIn: 'root'
})
export class YearSelectService {
    private messageSource = new BehaviorSubject<IYear | null>(null);
    currentMessage = this.messageSource.asObservable();

    private componentMethodCallSource = new Subject<IYear>();
    componentMethodCalled$ = this.componentMethodCallSource.asObservable();

    constructor() {}

    changeMessage(message: IYear) {
        this.messageSource.next(message);
    }

    callComponentMethod(message: IYear) {
        this.componentMethodCallSource.next(message);
        this.changeMessage(message);
    }
}