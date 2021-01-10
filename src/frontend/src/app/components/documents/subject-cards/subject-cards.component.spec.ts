import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectCardsComponent } from './subject-cards.component';

describe('SubjectCardsComponent', () => {
  let component: SubjectCardsComponent;
  let fixture: ComponentFixture<SubjectCardsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubjectCardsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectCardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
