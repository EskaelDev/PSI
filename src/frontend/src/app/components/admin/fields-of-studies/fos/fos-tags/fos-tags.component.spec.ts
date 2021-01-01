import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FosTagsComponent } from './fos-tags.component';

describe('FosTagsComponent', () => {
  let component: FosTagsComponent;
  let fixture: ComponentFixture<FosTagsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FosTagsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FosTagsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
