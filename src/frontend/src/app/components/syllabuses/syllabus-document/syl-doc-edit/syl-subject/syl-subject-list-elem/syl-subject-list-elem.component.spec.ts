import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SylSubjectListElemComponent } from './syl-subject-list-elem.component';

describe('SylSubjectListElemComponent', () => {
  let component: SylSubjectListElemComponent;
  let fixture: ComponentFixture<SylSubjectListElemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SylSubjectListElemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SylSubjectListElemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
