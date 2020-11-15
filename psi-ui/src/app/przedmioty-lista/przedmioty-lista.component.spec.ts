import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrzedmiotyListaComponent } from './przedmioty-lista.component';

describe('PrzedmiotyListaComponent', () => {
  let component: PrzedmiotyListaComponent;
  let fixture: ComponentFixture<PrzedmiotyListaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrzedmiotyListaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrzedmiotyListaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
