import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationCardComponent } from './notification-card.component';

describe('NotificationCardComponent', (): void => {
  let component: NotificationCardComponent;
  let fixture: ComponentFixture<NotificationCardComponent>;

  beforeEach(async(): Promise<void> => {
    await TestBed.configureTestingModule({
      declarations: [ NotificationCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NotificationCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
