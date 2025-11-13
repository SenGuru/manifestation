import { ComponentFixture, TestBed, async } from '@angular/core/testing';

import { AlternateLoginComponent } from './alternate-login.component';

describe('AlternateLoginComponent', (): void => {
  let component: AlternateLoginComponent;
  let fixture: ComponentFixture<AlternateLoginComponent>;

  // eslint-disable-next-line
  beforeEach(async((): void => {
    TestBed.configureTestingModule({
      declarations: [ AlternateLoginComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(AlternateLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
