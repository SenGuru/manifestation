import { ComponentFixture, TestBed, async } from '@angular/core/testing';

import { AlternateLoginComponent } from './alternate-login.component';

describe('AlternateLoginComponent', () => {
  let component: AlternateLoginComponent;
  let fixture: ComponentFixture<AlternateLoginComponent>;

  // eslint-disable-next-line @typescript-eslint/no-unsafe-argument, @typescript-eslint/no-unsafe-call
  beforeEach(async((): void => {
    TestBed.configureTestingModule({
      declarations: [ AlternateLoginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AlternateLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
