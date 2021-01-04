import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoEditComponent } from './lo-edit.component';

describe('LoEditComponent', () => {
  let component: LoEditComponent;
  let fixture: ComponentFixture<LoEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
