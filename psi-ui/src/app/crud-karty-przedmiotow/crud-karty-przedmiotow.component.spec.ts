import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrudKartyPrzedmiotowComponent } from './crud-karty-przedmiotow.component';

describe('CrudKartyPrzedmiotowComponent', () => {
  let component: CrudKartyPrzedmiotowComponent;
  let fixture: ComponentFixture<CrudKartyPrzedmiotowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrudKartyPrzedmiotowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrudKartyPrzedmiotowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
