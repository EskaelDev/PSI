import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YearSubjectPickerComponent } from './year-subject-picker.component';

describe('YearSubjectPickerComponent', () => {
  let component: YearSubjectPickerComponent;
  let fixture: ComponentFixture<YearSubjectPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ YearSubjectPickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(YearSubjectPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
