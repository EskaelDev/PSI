import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoDocEditComponent } from './lo-doc-edit.component';

describe('LoDocEditComponent', () => {
  let component: LoDocEditComponent;
  let fixture: ComponentFixture<LoDocEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoDocEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoDocEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
