import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SsoCallbackScreenComponent } from './sso-callback-screen.component';

describe('SsoCallbackScreenComponent', (): void => {
  let component: SsoCallbackScreenComponent;
  let fixture: ComponentFixture<SsoCallbackScreenComponent>;

  beforeEach(async(): Promise<void> => {
    await TestBed.configureTestingModule({
      declarations: [ SsoCallbackScreenComponent ]
    })
    .compileComponents();
  });

  beforeEach((): void => {
    fixture = TestBed.createComponent(SsoCallbackScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
