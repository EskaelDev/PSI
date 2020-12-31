import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FosYearPickerComponent } from './fos-year-picker.component';

describe('FosYearPickerComponent', () => {
  let component: FosYearPickerComponent;
  let fixture: ComponentFixture<FosYearPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FosYearPickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FosYearPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
