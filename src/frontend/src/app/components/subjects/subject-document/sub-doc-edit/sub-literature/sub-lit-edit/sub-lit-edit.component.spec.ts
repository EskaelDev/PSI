import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubLitEditComponent } from './sub-lit-edit.component';

describe('SubLitEditComponent', () => {
  let component: SubLitEditComponent;
  let fixture: ComponentFixture<SubLitEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubLitEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubLitEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
