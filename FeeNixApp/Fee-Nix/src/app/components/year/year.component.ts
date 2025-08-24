import { CUSTOM_ELEMENTS_SCHEMA, Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { environment } from '../../../environments/environment';
import { protectedResources } from '../../auth/configuration';
import { SelService } from '../../services/sel/sel.service';
import { IYear } from '../../services/sel.interface';
import { YearSelectService } from '../../services/year-select/year-select.service';
import { YearStatusService } from '../../services/year-status/year-status.service';
import { SnackBarService } from '../../services/snack-bar/snack-bar.service';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-year',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatButtonModule,
    MatChipsModule,
    MatSelectModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './year.component.html',
  styleUrls: ['./year.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class YearComponent implements OnInit, OnDestroy {
  yearList: IYear[] = [];
  selectedYear: IYear | null = null;
  yearStatusSubscription: Subscription | undefined;
  yearStatus: boolean = false;
  yearLoaderStatus: boolean = false;

  constructor(
    private selService: SelService,
    private yearSelectService: YearSelectService,
    private yearStatusService: YearStatusService,
    private snackBarService: SnackBarService
  ) {}

  ngOnInit(): void {
    this.getYearStatus();
    this.getYearStatusChangeValue();
    this.getYearList();
  }

  getYearList() {
    this.yearLoaderStatus = true;
    let url: string = `${environment.apiURL}${protectedResources.YearList.resourceUrl}/P`;
    this.selService.getData(url).subscribe({
      next: (response: any) => {
        if (response.value && response.value.length > 0 && response.response.returnNumber === 0) {
          this.yearList = response.value;
          // let getYear = new Date().getMonth()+1 > 3 ? new Date().getFullYear() : new Date().getFullYear()-1;
          // let findIndex = this.yearList.findIndex(val => val.yearNumber === getYear);
          let findIndex = this.yearList.findIndex(val => val.yearActiveFlag === 'Y');
          findIndex >= 0 ? this.selectedYear = this.yearList[findIndex] : '';
          if (this.selectedYear) {
            this.yearSelectService.callComponentMethod(this.selectedYear);
            sessionStorage.setItem("year", JSON.stringify(this.selectedYear));
          }
          this.yearLoaderStatus = false;
        } else {
          this.yearList = [];
          this.yearLoaderStatus = false;
          response.response.returnNumber !== 0 ? this.snackBarService.error(response.response.errorMessage, 'Ok', 4000) : '';
        }
        //console.log(response, 999);
      },
      error: (err) => {
        this.yearList = [];
        this.yearLoaderStatus = false;
        this.snackBarService.error(err.response.errorMessage, 'Ok', 4000);
        console.log(err, 999);
      }
    });
  }

  getYear(event: any) {
    this.selectedYear = event.value;
    if (this.selectedYear) {
      this.yearSelectService.callComponentMethod(this.selectedYear);
      sessionStorage.setItem("year", JSON.stringify(this.selectedYear));
    }
    //console.log(this.selectedYear);
  }

  getYearStatus() {
    this.yearStatusService.currentMessage.subscribe(message => {
      //this.yearStatus = message;
      setTimeout(() => { this.yearStatus = message }, 100);
      //console.log(this.yearStatus);
    });
  }

  getYearStatusChangeValue() {
    this.yearStatusSubscription = this.yearStatusService.componentMethodCalled$.subscribe(message => {
      //this.yearStatus = message;
      setTimeout(() => { this.yearStatus = message }, 100);
      //console.log(this.yearStatus);
    });
  }

  ngOnDestroy() {
    this.yearStatusSubscription?.unsubscribe();
  }
}