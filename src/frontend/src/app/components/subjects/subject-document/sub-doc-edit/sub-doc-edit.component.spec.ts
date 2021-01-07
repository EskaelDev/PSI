import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubDocEditComponent } from './sub-doc-edit.component';

describe('SubDocEditComponent', () => {
  let component: SubDocEditComponent;
  let fixture: ComponentFixture<SubDocEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubDocEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubDocEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
