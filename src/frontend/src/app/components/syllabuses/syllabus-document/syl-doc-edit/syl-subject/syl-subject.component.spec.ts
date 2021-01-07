import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SylSubjectComponent } from './syl-subject.component';

describe('SylSubjectComponent', () => {
  let component: SylSubjectComponent;
  let fixture: ComponentFixture<SylSubjectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SylSubjectComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SylSubjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
