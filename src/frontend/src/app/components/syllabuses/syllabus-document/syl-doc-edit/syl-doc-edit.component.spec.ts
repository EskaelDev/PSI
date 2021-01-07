import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SylDocEditComponent } from './syl-doc-edit.component';

describe('SylDocEditComponent', () => {
  let component: SylDocEditComponent;
  let fixture: ComponentFixture<SylDocEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SylDocEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SylDocEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
