import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrudKontaComponent } from './crud-konta.component';

describe('CrudKontaComponent', () => {
  let component: CrudKontaComponent;
  let fixture: ComponentFixture<CrudKontaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrudKontaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrudKontaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
