import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoListElemComponent } from './lo-list-elem.component';

describe('LoListElemComponent', () => {
  let component: LoListElemComponent;
  let fixture: ComponentFixture<LoListElemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoListElemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoListElemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
