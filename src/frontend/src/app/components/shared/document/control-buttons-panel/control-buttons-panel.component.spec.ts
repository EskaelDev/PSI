import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlButtonsPanelComponent } from './control-buttons-panel.component';

describe('ControlButtonsPanelComponent', () => {
  let component: ControlButtonsPanelComponent;
  let fixture: ComponentFixture<ControlButtonsPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ControlButtonsPanelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlButtonsPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
