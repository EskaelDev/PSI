import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SyllabusAcceptanceComponent } from './syllabus-acceptance.component';

describe('SyllabusAcceptanceComponent', () => {
  let component: SyllabusAcceptanceComponent;
  let fixture: ComponentFixture<SyllabusAcceptanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SyllabusAcceptanceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SyllabusAcceptanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
