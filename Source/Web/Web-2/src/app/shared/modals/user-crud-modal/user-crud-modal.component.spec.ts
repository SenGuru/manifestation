import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserCrudModalComponent } from './user-crud-modal.component';

describe('UserCrudModalComponent', (): void => {
  let component: UserCrudModalComponent;
  let fixture: ComponentFixture<UserCrudModalComponent>;

  beforeEach(((): void => {
    TestBed.configureTestingModule({
      declarations: [ UserCrudModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(UserCrudModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
