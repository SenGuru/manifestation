import { NavGuard } from './nav.guard';
import { TestBed } from '@angular/core/testing';

describe('NavGuard', () => {
  let guard: NavGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(NavGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
