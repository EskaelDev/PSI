import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcceptSyllabusComponent } from './accept-syllabus.component';

describe('AcceptSyllabusComponent', () => {
  let component: AcceptSyllabusComponent;
  let fixture: ComponentFixture<AcceptSyllabusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcceptSyllabusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcceptSyllabusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
