import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubCardElemComponent } from './sub-card-elem.component';

describe('SubCardElemComponent', () => {
  let component: SubCardElemComponent;
  let fixture: ComponentFixture<SubCardElemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubCardElemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubCardElemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
