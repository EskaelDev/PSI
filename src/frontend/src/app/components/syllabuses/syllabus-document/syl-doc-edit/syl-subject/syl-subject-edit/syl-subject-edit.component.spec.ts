import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SylSubjectEditComponent } from './syl-subject-edit.component';

describe('SylSubjectEditComponent', () => {
  let component: SylSubjectEditComponent;
  let fixture: ComponentFixture<SylSubjectEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SylSubjectEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SylSubjectEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
