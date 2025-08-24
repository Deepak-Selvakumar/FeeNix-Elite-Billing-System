import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-no-data',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatProgressSpinnerModule],
  templateUrl: './no-data.component.html',
  styleUrls: ['./no-data.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class NoDataComponent {
  @Input() loaderStatus: boolean = false;
  @Input() loaderText: string = 'No Data Found';
}