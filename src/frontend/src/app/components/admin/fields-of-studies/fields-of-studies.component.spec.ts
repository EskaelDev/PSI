import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldsOfStudiesComponent } from './fields-of-studies.component';

describe('FieldsOfStudiesComponent', () => {
  let component: FieldsOfStudiesComponent;
  let fixture: ComponentFixture<FieldsOfStudiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FieldsOfStudiesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FieldsOfStudiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
