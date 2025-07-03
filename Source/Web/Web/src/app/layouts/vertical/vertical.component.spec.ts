import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerticalComponent } from './vertical.component';

describe('VerticalComponent', (): void => {
  let component: VerticalComponent;
  let fixture: ComponentFixture<VerticalComponent>;

  beforeEach(async(): Promise<void> => {
    await TestBed.configureTestingModule({
      declarations: [ VerticalComponent ]
    })
    .compileComponents();
  });

  beforeEach((): void => {
    fixture = TestBed.createComponent(VerticalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
