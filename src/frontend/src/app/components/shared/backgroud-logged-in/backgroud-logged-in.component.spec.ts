import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackgroudLoggedInComponent } from './backgroud-logged-in.component';

describe('BackgroudLoggedInComponent', () => {
  let component: BackgroudLoggedInComponent;
  let fixture: ComponentFixture<BackgroudLoggedInComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BackgroudLoggedInComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BackgroudLoggedInComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
