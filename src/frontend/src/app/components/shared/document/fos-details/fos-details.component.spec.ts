import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FosDetailsComponent } from './fos-details.component';

describe('FosDetailsComponent', () => {
  let component: FosDetailsComponent;
  let fixture: ComponentFixture<FosDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FosDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FosDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
