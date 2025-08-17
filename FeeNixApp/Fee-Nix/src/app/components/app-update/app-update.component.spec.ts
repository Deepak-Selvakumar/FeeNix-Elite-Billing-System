import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppUpdateComponent } from './app-update.component';

describe('AppUpdateComponent', () => {
  let component: AppUpdateComponent;
  let fixture: ComponentFixture<AppUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppUpdateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
