import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectPickerComponent } from './subject-picker.component';

describe('SubjectPickerComponent', () => {
  let component: SubjectPickerComponent;
  let fixture: ComponentFixture<SubjectPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubjectPickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
