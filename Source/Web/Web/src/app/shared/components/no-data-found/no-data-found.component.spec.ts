import { ComponentFixture, TestBed, async } from '@angular/core/testing';

import { NoDataFoundComponent } from './no-data-found.component';

describe('NoDataFoundComponent', (): void => {
  let component: NoDataFoundComponent;
  let fixture: ComponentFixture<NoDataFoundComponent>;

  // eslint-disable-next-line
  beforeEach(async((): void => {
    TestBed.configureTestingModule({
      declarations: [ NoDataFoundComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(NoDataFoundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
