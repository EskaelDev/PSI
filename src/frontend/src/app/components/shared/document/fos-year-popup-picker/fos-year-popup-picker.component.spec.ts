import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FosYearPopupPickerComponent } from './fos-year-popup-picker.component';

describe('FosYearPopupPickerComponent', () => {
  let component: FosYearPopupPickerComponent;
  let fixture: ComponentFixture<FosYearPopupPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FosYearPopupPickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FosYearPopupPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
