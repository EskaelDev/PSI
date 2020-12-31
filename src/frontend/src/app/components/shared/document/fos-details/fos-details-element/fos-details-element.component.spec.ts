import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FosDetailsElementComponent } from './fos-details-element.component';

describe('FosDetailsElementComponent', () => {
  let component: FosDetailsElementComponent;
  let fixture: ComponentFixture<FosDetailsElementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FosDetailsElementComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FosDetailsElementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
