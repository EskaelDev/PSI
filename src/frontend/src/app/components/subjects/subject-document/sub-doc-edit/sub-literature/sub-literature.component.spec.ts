import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubLiteratureComponent } from './sub-literature.component';

describe('SubLiteratureComponent', () => {
  let component: SubLiteratureComponent;
  let fixture: ComponentFixture<SubLiteratureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubLiteratureComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubLiteratureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
