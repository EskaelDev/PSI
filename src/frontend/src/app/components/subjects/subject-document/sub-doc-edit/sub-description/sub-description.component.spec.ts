import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubDescriptionComponent } from './sub-description.component';

describe('SubDescriptionComponent', () => {
  let component: SubDescriptionComponent;
  let fixture: ComponentFixture<SubDescriptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubDescriptionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubDescriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
