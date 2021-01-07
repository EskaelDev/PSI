import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubLessonElemComponent } from './sub-lesson-elem.component';

describe('SubLessonElemComponent', () => {
  let component: SubLessonElemComponent;
  let fixture: ComponentFixture<SubLessonElemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubLessonElemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubLessonElemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
