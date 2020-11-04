import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrudEfektyComponent } from './crud-efekty.component';

describe('CrudEfektyComponent', () => {
  let component: CrudEfektyComponent;
  let fixture: ComponentFixture<CrudEfektyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrudEfektyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrudEfektyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
