import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfileTitleComponent } from './profile-title.component';

describe('ProfileTitleComponent', (): void => {
  let component: ProfileTitleComponent;
  let fixture: ComponentFixture<ProfileTitleComponent>;

  beforeEach(async(): Promise<void> => {
    await TestBed.configureTestingModule({
      declarations: [ ProfileTitleComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfileTitleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
