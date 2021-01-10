import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubLessonEditComponent } from './sub-lesson-edit.component';

describe('SubLessonEditComponent', () => {
  let component: SubLessonEditComponent;
  let fixture: ComponentFixture<SubLessonEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubLessonEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubLessonEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
