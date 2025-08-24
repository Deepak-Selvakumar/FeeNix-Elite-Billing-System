import { Component, OnInit, Input, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { Router } from '@angular/router';
import { BreadcrumbService } from '../../services/breadcrumb/breadcrumb.service';
import { IBreadcrumb } from '../../services/sel.interface';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
    selector: 'app-breadcrumb-list',
    standalone: true,
    imports: [CommonModule, MatButtonModule, MatIconModule],
    templateUrl: './breadcrumb-list.component.html',
    styleUrls: ['./breadcrumb-list.component.scss'],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class BreadcrumbListComponent implements OnInit {
    breadcrumbs: IBreadcrumb[] = []
    //@Input() breadcrumbs: Ibreadcrumb[] = []
    
    constructor(
        private router: Router,
        private breadcrumbService: BreadcrumbService
    ) { }
    
    ngOnInit() {
        this.breadcrumbService.currentMessage.subscribe(message => {
            if (message) {
                //this.breadcrumbs = message;
                setTimeout(() => { this.breadcrumbs = message }, 100);
            }
            //console.log(message, 999);
        });
        //console.log(this.breadcrumbs);
    }
    
    navigate(url: string) {
        this.router.navigate([url]);
    }
}