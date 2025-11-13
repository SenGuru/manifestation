import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GlobalSearchDetailsComponent } from './global-search-details.component';

describe('GlobalSearchDetailsComponent', (): void => {
  let component: GlobalSearchDetailsComponent;
  let fixture: ComponentFixture<GlobalSearchDetailsComponent>;

  // eslint-disable-next-line
  beforeEach(async((): void => {
    TestBed.configureTestingModule({
      declarations: [ GlobalSearchDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(GlobalSearchDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
