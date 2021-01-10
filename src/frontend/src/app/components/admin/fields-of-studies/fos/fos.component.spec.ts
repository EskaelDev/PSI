import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FosComponent } from './fos.component';

describe('FosComponent', () => {
  let component: FosComponent;
  let fixture: ComponentFixture<FosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FosComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
