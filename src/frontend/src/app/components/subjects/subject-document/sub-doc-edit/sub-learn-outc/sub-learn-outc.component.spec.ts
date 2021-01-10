import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubLearnOutcComponent } from './sub-learn-outc.component';

describe('SubLearnOutcComponent', () => {
  let component: SubLearnOutcComponent;
  let fixture: ComponentFixture<SubLearnOutcComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubLearnOutcComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubLearnOutcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
