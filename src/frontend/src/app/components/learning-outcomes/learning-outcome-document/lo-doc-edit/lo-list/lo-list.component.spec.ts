import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoListComponent } from './lo-list.component';

describe('LoListComponent', () => {
  let component: LoListComponent;
  let fixture: ComponentFixture<LoListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
