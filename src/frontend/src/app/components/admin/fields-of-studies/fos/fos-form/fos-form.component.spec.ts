import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FosFormComponent } from './fos-form.component';

describe('FosFormComponent', () => {
  let component: FosFormComponent;
  let fixture: ComponentFixture<FosFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FosFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FosFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
