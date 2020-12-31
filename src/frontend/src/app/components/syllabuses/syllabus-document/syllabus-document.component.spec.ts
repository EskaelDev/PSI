import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SyllabusDocumentComponent } from './syllabus-document.component';

describe('SyllabusDocumentComponent', () => {
  let component: SyllabusDocumentComponent;
  let fixture: ComponentFixture<SyllabusDocumentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SyllabusDocumentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SyllabusDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
