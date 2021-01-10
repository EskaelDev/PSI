import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubChangeSupervisorComponent } from './sub-change-supervisor.component';

describe('SubChangeSupervisorComponent', () => {
  let component: SubChangeSupervisorComponent;
  let fixture: ComponentFixture<SubChangeSupervisorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubChangeSupervisorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubChangeSupervisorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
