import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FosYearFilterComponent } from './fos-year-filter.component';

describe('FosYearFilterComponent', () => {
  let component: FosYearFilterComponent;
  let fixture: ComponentFixture<FosYearFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FosYearFilterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FosYearFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
