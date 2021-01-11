import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PointsElemComponent } from './points-elem.component';

describe('PointsElemComponent', () => {
  let component: PointsElemComponent;
  let fixture: ComponentFixture<PointsElemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PointsElemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PointsElemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
