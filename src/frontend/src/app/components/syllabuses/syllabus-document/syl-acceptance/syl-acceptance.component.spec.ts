import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SylAcceptanceComponent } from './syl-acceptance.component';

describe('SylAcceptanceComponent', () => {
  let component: SylAcceptanceComponent;
  let fixture: ComponentFixture<SylAcceptanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SylAcceptanceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SylAcceptanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
