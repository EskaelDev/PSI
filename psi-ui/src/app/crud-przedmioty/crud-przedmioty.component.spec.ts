import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrudPrzedmiotyComponent } from './crud-przedmioty.component';

describe('CrudPrzedmiotyComponent', () => {
  let component: CrudPrzedmiotyComponent;
  let fixture: ComponentFixture<CrudPrzedmiotyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrudPrzedmiotyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrudPrzedmiotyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
