import { SecondsToHourPipe } from './seconds-to-hour.pipe';

describe('SecondsToHourPipe', (): void => {
  it('create an instance', (): void => {
    const pipe = new SecondsToHourPipe();
    expect(pipe).toBeTruthy();
  });
});
