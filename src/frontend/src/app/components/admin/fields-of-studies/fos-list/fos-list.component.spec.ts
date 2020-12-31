import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FosListComponent } from './fos-list.component';

describe('FosListComponent', () => {
  let component: FosListComponent;
  let fixture: ComponentFixture<FosListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FosListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FosListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
