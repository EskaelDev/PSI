import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectDocumentComponent } from './subject-document.component';

describe('SubjectDocumentComponent', () => {
  let component: SubjectDocumentComponent;
  let fixture: ComponentFixture<SubjectDocumentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubjectDocumentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubjectDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
