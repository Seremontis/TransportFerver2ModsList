import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemsUserModalComponent } from './items-user-modal.component';

describe('ItemsUserModalComponent', () => {
  let component: ItemsUserModalComponent;
  let fixture: ComponentFixture<ItemsUserModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ItemsUserModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemsUserModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
