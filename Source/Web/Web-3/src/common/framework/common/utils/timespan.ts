/*  sasi: temporarily included this class for TimeSpan implementation  
*  from https://gist.github.com/ranwahle/e78a8859ff78bdd68055017fc5875fcb
*  guru to check if https://github.com/electricessence/TypeScript.NET/blob/master/source/System/Time/TimeSpan.ts  is better suited 
*/

const MILLISECONDS_IN_A_SECOND = 1000;
const SECONDS_IN_A_MINUTE = 60;
const MINUTES_IN_AN_HOUR = 60;
const HOURS_IN_A_DAY = 24;
const DAYS_IN_A_WEEK = 7;

const MILLISECONDS_IN_A_MINUTE = MILLISECONDS_IN_A_SECOND * SECONDS_IN_A_MINUTE;
const MILLISECONDS_IN_AN_HOUR = MILLISECONDS_IN_A_MINUTE * MINUTES_IN_AN_HOUR;
const MILLISECONDS_IN_A_DAY = MILLISECONDS_IN_AN_HOUR * HOURS_IN_A_DAY;
const MILLISECONDS_IN_A_WEEK = MILLISECONDS_IN_A_DAY * DAYS_IN_A_WEEK;

export class TimeSpan {

	private _milliseconds: number;
	private _totalMilliSeconds: number;
	private _seconds: number;
	private _minutes: number;
	private _hours: number;
	private _days: number;

	public constructor(milliSeconds = 0) {
		this._seconds = 0;
		this._minutes = 0;
		this._hours = 0;
		this._days = 0;
		this._totalMilliSeconds = 0;

		this._milliseconds = milliSeconds;
	}

	public get days(): number {
		return this._days;
	}

	public get hours(): number {
		return this._hours;
	}

	public get minutes(): number {
		return this._minutes;
	}

	public get seconds(): number {
		return this._seconds;
	}

	public get milliseconds(): number {
		return this._milliseconds;
	}

	public get totalMilliSeconds(): number {
		return this._totalMilliSeconds;
	}

	public get totalSeconds(): number {
		return Math.floor(this._totalMilliSeconds / MILLISECONDS_IN_A_SECOND);
	}

	public get totalMinutes(): number {
		return Math.floor(this._totalMilliSeconds / MILLISECONDS_IN_A_MINUTE);
	}

	public get totalHours(): number {
		return Math.floor(this._totalMilliSeconds / MILLISECONDS_IN_AN_HOUR);
	}

	public set days(value: number) {
		if (isNaN(value)) {
			value = 0;
		}
		this._days = value;
		this.calcMilliSeconds();
	}

	public set hours(value: number) {
		if (isNaN(value)) {
			value = 0;
		}
		this._hours = value;
		this.calcMilliSeconds();
	}

	public set minutes(value: number) {
		if (isNaN(value)) {
			value = 0;
		}
		this._minutes = value;
		this.calcMilliSeconds();
	}

	public set seconds(value: number) {
		this._seconds = value;
		this.calcMilliSeconds();
	}

	public set milliseconds(value: number) {
		if (isNaN(value)) {
			value = 0;
		}
		this._milliseconds = value;
		this.calcMilliSeconds();
	}

	public static Subtract(date1: number, date2: number): TimeSpan {
		const milliSeconds: number = date1 - date2;

		return new TimeSpan(milliSeconds);

	}

	public static Day(): TimeSpan {
		return new TimeSpan(MILLISECONDS_IN_A_DAY);
	}
	public static Hour(): TimeSpan { return new TimeSpan(MILLISECONDS_IN_AN_HOUR); }
	public static Week(): TimeSpan { return new TimeSpan(MILLISECONDS_IN_A_WEEK); }
	public static Month(): TimeSpan {
		const now: Date = new Date();
		const aMonthAgo: Date = new Date();
		aMonthAgo.setMonth(aMonthAgo.getMonth() - 1);
		return new TimeSpan(now.getMilliseconds() - aMonthAgo.getMilliseconds());
	}

	public addTo(date: Date): Date {
		date.setMilliseconds(date.getMilliseconds() + this.totalMilliSeconds);

		return date;
	}

	public subtructFrom(date: Date): Date {
		date.setMilliseconds(date.getMilliseconds() - this.totalMilliSeconds);

		return date;
	}

	public floorValue(origValue: number, maxValue: number): {
		modulu: number;
		addition: number;
	} {
		return { modulu: origValue % maxValue, addition: Math.floor(origValue / maxValue) };
	}

	public calcMilliSeconds(): void {

		const newMilliSecond = this.floorValue(this._milliseconds, MILLISECONDS_IN_A_SECOND);
		this._milliseconds = newMilliSecond.modulu;
		this._seconds += newMilliSecond.addition;

		const newSecond = this.floorValue(this._seconds, SECONDS_IN_A_MINUTE);
		this._seconds = newSecond.modulu;
		this._minutes += newSecond.addition;

		const newminutes = this.floorValue(this._minutes, MINUTES_IN_AN_HOUR);
		this._minutes = newminutes.modulu;
		this._hours += newminutes.addition;

		const newDays = this.floorValue(this._hours, HOURS_IN_A_DAY);
		this._hours = newDays.modulu;
		this._days += newDays.addition;

		this._totalMilliSeconds = this.days * MILLISECONDS_IN_A_DAY + this.hours * MILLISECONDS_IN_AN_HOUR + this.minutes * MILLISECONDS_IN_A_MINUTE
			+ this.seconds * MILLISECONDS_IN_A_SECOND + this.milliseconds;
	}

}