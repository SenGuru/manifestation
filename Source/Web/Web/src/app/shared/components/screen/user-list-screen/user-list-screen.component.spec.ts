import { ComponentFixture, TestBed, async, waitForAsync } from '@angular/core/testing';

import { UserListScreenComponent } from './user-list-screen.component';

describe('UserListScreenComponent', (): void => {
  let component: UserListScreenComponent;
  let fixture: ComponentFixture<UserListScreenComponent>;

  beforeEach(waitForAsync((): void => {
    TestBed.configureTestingModule({
      declarations: [ UserListScreenComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(UserListScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
