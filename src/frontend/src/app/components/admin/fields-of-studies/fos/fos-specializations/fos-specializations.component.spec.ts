import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FosSpecializationsComponent } from './fos-specializations.component';

describe('FosSpecializationsComponent', () => {
  let component: FosSpecializationsComponent;
  let fixture: ComponentFixture<FosSpecializationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FosSpecializationsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FosSpecializationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
