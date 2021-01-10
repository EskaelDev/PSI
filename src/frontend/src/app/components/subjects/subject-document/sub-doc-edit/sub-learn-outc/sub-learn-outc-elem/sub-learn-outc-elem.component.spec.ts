import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubLearnOutcElemComponent } from './sub-learn-outc-elem.component';

describe('SubLearnOutcElemComponent', () => {
  let component: SubLearnOutcElemComponent;
  let fixture: ComponentFixture<SubLearnOutcElemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubLearnOutcElemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubLearnOutcElemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
