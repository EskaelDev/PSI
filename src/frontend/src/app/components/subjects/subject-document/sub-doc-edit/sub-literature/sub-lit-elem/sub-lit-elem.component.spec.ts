import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubLitElemComponent } from './sub-lit-elem.component';

describe('SubLitElemComponent', () => {
  let component: SubLitElemComponent;
  let fixture: ComponentFixture<SubLitElemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubLitElemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubLitElemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
