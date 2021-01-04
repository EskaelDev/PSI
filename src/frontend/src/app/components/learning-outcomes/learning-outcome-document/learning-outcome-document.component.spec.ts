import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LearningOutcomeDocumentComponent } from './learning-outcome-document.component';

describe('LearningOutcomeDocumentComponent', () => {
  let component: LearningOutcomeDocumentComponent;
  let fixture: ComponentFixture<LearningOutcomeDocumentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LearningOutcomeDocumentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LearningOutcomeDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
