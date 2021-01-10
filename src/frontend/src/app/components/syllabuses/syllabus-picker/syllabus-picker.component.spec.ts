import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SyllabusPickerComponent } from './syllabus-picker.component';

describe('SyllabusPickerComponent', () => {
  let component: SyllabusPickerComponent;
  let fixture: ComponentFixture<SyllabusPickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SyllabusPickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SyllabusPickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
