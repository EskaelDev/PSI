import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseSyllabusComponent } from './choose-syllabus.component';

describe('ChooseSyllabusComponent', () => {
  let component: ChooseSyllabusComponent;
  let fixture: ComponentFixture<ChooseSyllabusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChooseSyllabusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChooseSyllabusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
