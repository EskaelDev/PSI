import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PointsLimitComponent } from './points-limit.component';

describe('PointsLimitComponent', () => {
  let component: PointsLimitComponent;
  let fixture: ComponentFixture<PointsLimitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PointsLimitComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PointsLimitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
