import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrudSyllabusComponent } from './crud-syllabus.component';

describe('CrudSyllabusComponent', () => {
  let component: CrudSyllabusComponent;
  let fixture: ComponentFixture<CrudSyllabusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrudSyllabusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrudSyllabusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
