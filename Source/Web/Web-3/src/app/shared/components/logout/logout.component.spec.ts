import { ComponentFixture, TestBed, async } from '@angular/core/testing';

import { LogoutComponent } from './logout.component';

describe('LogoutComponent', (): void => {
  let component: LogoutComponent;
  let fixture: ComponentFixture<LogoutComponent>;

  // eslint-disable-next-line
  beforeEach(async((): void => {
    TestBed.configureTestingModule({
      declarations: [ LogoutComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(LogoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
