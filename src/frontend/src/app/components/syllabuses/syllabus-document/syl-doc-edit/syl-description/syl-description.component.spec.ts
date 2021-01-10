import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SylDescriptionComponent } from './syl-description.component';

describe('SylDescriptionComponent', () => {
  let component: SylDescriptionComponent;
  let fixture: ComponentFixture<SylDescriptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SylDescriptionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SylDescriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
