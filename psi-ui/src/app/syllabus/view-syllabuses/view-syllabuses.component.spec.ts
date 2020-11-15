import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewSyllabusesComponent } from './view-syllabuses.component';

describe('ViewSyllabusesComponent', () => {
  let component: ViewSyllabusesComponent;
  let fixture: ComponentFixture<ViewSyllabusesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewSyllabusesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewSyllabusesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
