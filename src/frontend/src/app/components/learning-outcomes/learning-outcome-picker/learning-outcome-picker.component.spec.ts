import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LearningOutcomePickerComponent } from './learning-outcome-picker.component';

describe('LearningOutcomePickerComponent', () => {
  let component: LearningOutcomePickerComponent;
  let fixture: ComponentFixture<LearningOutcomePickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LearningOutcomePickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LearningOutcomePickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
