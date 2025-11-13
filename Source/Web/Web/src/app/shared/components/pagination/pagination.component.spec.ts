import { ComponentFixture, TestBed, async } from '@angular/core/testing';

import { PaginationComponent } from './pagination.component';

describe('PaginationComponent', (): void => {
  let component: PaginationComponent;
  let fixture: ComponentFixture<PaginationComponent>;

  // eslint-disable-next-line
  beforeEach(async((): void => {
    TestBed.configureTestingModule({
      declarations: [ PaginationComponent ]
    })
    .compileComponents();
  }));

  beforeEach((): void => {
    fixture = TestBed.createComponent(PaginationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', (): void => {
    expect(component).toBeTruthy();
  });
});
