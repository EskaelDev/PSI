import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubLessonsComponent } from './sub-lessons.component';

describe('SubLessonsComponent', () => {
  let component: SubLessonsComponent;
  let fixture: ComponentFixture<SubLessonsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubLessonsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubLessonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
