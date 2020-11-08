import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrudKierunkiComponent } from './crud-kierunki.component';

describe('CrudKierunkiComponent', () => {
  let component: CrudKierunkiComponent;
  let fixture: ComponentFixture<CrudKierunkiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrudKierunkiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrudKierunkiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
