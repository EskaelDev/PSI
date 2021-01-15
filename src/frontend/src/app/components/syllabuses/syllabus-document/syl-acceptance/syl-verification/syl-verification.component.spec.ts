import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SylVerificationComponent } from './syl-verification.component';

describe('SylVerificationComponent', () => {
  let component: SylVerificationComponent;
  let fixture: ComponentFixture<SylVerificationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SylVerificationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SylVerificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
