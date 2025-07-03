import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDialogueComponent } from './confirm-dialogue.component';

describe('ConfirmDialogueComponent', (): void => {
  let component: ConfirmDialogueComponent;
  let fixture: ComponentFixture<ConfirmDialogueComponent>;

  beforeEach(((): void => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(ConfirmDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
