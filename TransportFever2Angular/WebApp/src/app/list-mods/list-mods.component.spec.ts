import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListModsComponent } from './list-mods.component';

describe('ListModsComponent', () => {
  let component: ListModsComponent;
  let fixture: ComponentFixture<ListModsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListModsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListModsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
