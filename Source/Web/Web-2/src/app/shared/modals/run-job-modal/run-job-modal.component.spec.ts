import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RunJobModalComponent } from './run-job-modal.component';

describe('RunJobModalComponent', (): void => {
  let component: RunJobModalComponent;
  let fixture: ComponentFixture<RunJobModalComponent>;

  beforeEach(async(): Promise<void> => {
    await TestBed.configureTestingModule({
      imports: [RunJobModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RunJobModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
