import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { IUserSelect } from '../../services/sel.interface';

@Injectable({
    providedIn: 'root'
})
export class UserSelectService {
    private messageSource = new BehaviorSubject<IUserSelect | null>(null);
    currentMessage = this.messageSource.asObservable();

    private componentMethodCallSource = new Subject<IUserSelect>();
    componentMethodCalled$ = this.componentMethodCallSource.asObservable();

    constructor() {}

    changeMessage(message: IUserSelect) {
        this.messageSource.next(message);
    }

    callComponentMethod(message: IUserSelect) {
        this.componentMethodCallSource.next(message);
        this.changeMessage(message);
    }
}