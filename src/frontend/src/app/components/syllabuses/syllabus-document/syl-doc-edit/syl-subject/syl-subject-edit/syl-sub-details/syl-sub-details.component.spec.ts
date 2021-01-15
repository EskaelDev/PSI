import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SylSubDetailsComponent } from './syl-sub-details.component';

describe('SylSubDetailsComponent', () => {
  let component: SylSubDetailsComponent;
  let fixture: ComponentFixture<SylSubDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SylSubDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SylSubDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
