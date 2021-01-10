import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubLearnOutcEditComponent } from './sub-learn-outc-edit.component';

describe('SubLearnOutcEditComponent', () => {
  let component: SubLearnOutcEditComponent;
  let fixture: ComponentFixture<SubLearnOutcEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubLearnOutcEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubLearnOutcEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
