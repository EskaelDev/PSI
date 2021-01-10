import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubLessonsProgramComponent } from './sub-lessons-program.component';

describe('SubLessonsProgramComponent', () => {
  let component: SubLessonsProgramComponent;
  let fixture: ComponentFixture<SubLessonsProgramComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubLessonsProgramComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubLessonsProgramComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
