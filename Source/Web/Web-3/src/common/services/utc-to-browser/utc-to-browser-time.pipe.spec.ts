import { UtcToBrowserTimePipe } from './utc-to-browser-time.pipe';

describe('UtcToBrowserTimePipe', (): void => {
  it('create an instance', (): void => {
    const pipe = new UtcToBrowserTimePipe();
    expect(pipe).toBeTruthy();
  });
});
