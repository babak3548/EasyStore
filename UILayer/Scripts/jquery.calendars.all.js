﻿

(function ($) {


    function Calendars() {
        this.regional = {
            '': { invalidCalendar: 'Calendar {0} not found',
                invalidDate: 'Invalid {0} date',
                invalidMonth: 'Invalid {0} month',
                invalidYear: 'Invalid {0} year',
                differentCalendars: 'Cannot mix {0} and {1} dates'
            }
        };
        this.local = this.regional[''];
        this.calendars = {};
        this._localCals = {};
    }

    $.extend(Calendars.prototype, {


        instance: function (name, language) {
            name = (name || 'gregorian').toLowerCase();
            language = language || '';
            var cal = this._localCals[name + '-' + language];
            if (!cal && this.calendars[name]) {
                cal = new this.calendars[name](language);
                this._localCals[name + '-' + language] = cal;
            }
            if (!cal) {
                throw (this.local.invalidCalendar || this.regional[''].invalidCalendar).
				replace(/\{0\}/, name);
            }
            return cal;
        },


        newDate: function (year, month, day, calendar, language) {
            calendar = (year != null && year.year ? year.calendar() : (typeof calendar == 'string' ?
			this.instance(calendar, language) : calendar)) || this.instance();
            return calendar.newDate(year, month, day);
        }
    });


    function CDate(calendar, year, month, day) {
        this._calendar = calendar;
        this._year = year;
        this._month = month;
        this._day = day;
        if (this._calendar._validateLevel == 0 &&
			!this._calendar.isValid(this._year, this._month, this._day)) {
            throw ($.calendars.local.invalidDate || $.calendars.regional[''].invalidDate).
			replace(/\{0\}/, this._calendar.local.name);
        }
    }


    function pad(value, length) {
        value = '' + value;
        return '000000'.substring(0, length - value.length) + value;
    }

    $.extend(CDate.prototype, {


        newDate: function (year, month, day) {
            return this._calendar.newDate((year == null ? this : year), month, day);
        },


        year: function (year) {
            return (arguments.length == 0 ? this._year : this.set(year, 'y'));
        },


        month: function (month) {
            return (arguments.length == 0 ? this._month : this.set(month, 'm'));
        },


        day: function (day) {
            return (arguments.length == 0 ? this._day : this.set(day, 'd'));
        },


        date: function (year, month, day) {
            if (!this._calendar.isValid(year, month, day)) {
                throw ($.calendars.local.invalidDate || $.calendars.regional[''].invalidDate).
				replace(/\{0\}/, this._calendar.local.name);
            }
            this._year = year;
            this._month = month;
            this._day = day;
            return this;
        },


        leapYear: function () {
            return this._calendar.leapYear(this);
        },


        epoch: function () {
            return this._calendar.epoch(this);
        },


        formatYear: function () {
            return this._calendar.formatYear(this);
        },


        monthOfYear: function () {
            return this._calendar.monthOfYear(this);
        },


        weekOfYear: function () {
            return this._calendar.weekOfYear(this);
        },


        daysInYear: function () {
            return this._calendar.daysInYear(this);
        },


        dayOfYear: function () {
            return this._calendar.dayOfYear(this);
        },


        daysInMonth: function () {
            return this._calendar.daysInMonth(this);
        },


        dayOfWeek: function () {
            return this._calendar.dayOfWeek(this);
        },


        weekDay: function () {
            return this._calendar.weekDay(this);
        },


        extraInfo: function () {
            return this._calendar.extraInfo(this);
        },


        add: function (offset, period) {
            return this._calendar.add(this, offset, period);
        },


        set: function (value, period) {
            return this._calendar.set(this, value, period);
        },


        compareTo: function (date) {
            if (this._calendar.name != date._calendar.name) {
                throw ($.calendars.local.differentCalendars || $.calendars.regional[''].differentCalendars).
				replace(/\{0\}/, this._calendar.local.name).replace(/\{1\}/, date._calendar.local.name);
            }
            var c = (this._year != date._year ? this._year - date._year :
			this._month != date._month ? this.monthOfYear() - date.monthOfYear() :
			this._day - date._day);
            return (c == 0 ? 0 : (c < 0 ? -1 : +1));
        },


        calendar: function () {
            return this._calendar;
        },


        toJD: function () {
            return this._calendar.toJD(this);
        },


        fromJD: function (jd) {
            return this._calendar.fromJD(jd);
        },


        toJSDate: function () {
            return this._calendar.toJSDate(this);
        },


        fromJSDate: function (jsd) {
            return this._calendar.fromJSDate(jsd);
        },


        toString: function () {
            return (this.year() < 0 ? '-' : '') + pad(Math.abs(this.year()), 4) +
			'-' + pad(this.month(), 2) + '-' + pad(this.day(), 2);
        }
    });


    function BaseCalendar() {
        this.shortYearCutoff = '+10';
    }

    $.extend(BaseCalendar.prototype, {
        _validateLevel: 0, // "Stack" to turn validation on/off


        newDate: function (year, month, day) {
            if (year == null) {
                return this.today();
            }
            if (year.year) {
                this._validate(year, month, day,
				$.calendars.local.invalidDate || $.calendars.regional[''].invalidDate);
                day = year.day();
                month = year.month();
                year = year.year();
            }
            return new CDate(this, year, month, day);
        },


        today: function () {
            return this.fromJSDate(new Date());
        },


        epoch: function (year) {
            var date = this._validate(year, this.minMonth, this.minDay,
			$.calendars.local.invalidYear || $.calendars.regional[''].invalidYear);
            return (date.year() < 0 ? this.local.epochs[0] : this.local.epochs[1]);
        },


        formatYear: function (year) {
            var date = this._validate(year, this.minMonth, this.minDay,
			$.calendars.local.invalidYear || $.calendars.regional[''].invalidYear);
            return (date.year() < 0 ? '-' : '') + pad(Math.abs(date.year()), 4)
        },


        monthsInYear: function (year) {
            this._validate(year, this.minMonth, this.minDay,
			$.calendars.local.invalidYear || $.calendars.regional[''].invalidYear);
            return 12;
        },


        monthOfYear: function (year, month) {
            var date = this._validate(year, month, this.minDay,
			$.calendars.local.invalidMonth || $.calendars.regional[''].invalidMonth);
            return (date.month() + this.monthsInYear(date) - this.firstMonth) %
			this.monthsInYear(date) + this.minMonth;
        },


        fromMonthOfYear: function (year, ord) {
            var m = (ord + this.firstMonth - 2 * this.minMonth) %
			this.monthsInYear(year) + this.minMonth;
            this._validate(year, m, this.minDay,
			$.calendars.local.invalidMonth || $.calendars.regional[''].invalidMonth);
            return m;
        },


        daysInYear: function (year) {
            var date = this._validate(year, this.minMonth, this.minDay,
			$.calendars.local.invalidYear || $.calendars.regional[''].invalidYear);
            return (this.leapYear(date) ? 366 : 365);
        },


        dayOfYear: function (year, month, day) {
            var date = this._validate(year, month, day,
			$.calendars.local.invalidDate || $.calendars.regional[''].invalidDate);
            return date.toJD() - this.newDate(date.year(),
			this.fromMonthOfYear(date.year(), this.minMonth), this.minDay).toJD() + 1;
        },


        daysInWeek: function () {
            return 7;
        },


        dayOfWeek: function (year, month, day) {
            var date = this._validate(year, month, day,
			$.calendars.local.invalidDate || $.calendars.regional[''].invalidDate);
            return (Math.floor(this.toJD(date)) + 2) % this.daysInWeek();
        },


        extraInfo: function (year, month, day) {
            this._validate(year, month, day,
			$.calendars.local.invalidDate || $.calendars.regional[''].invalidDate);
            return {};
        },


        add: function (date, offset, period) {
            this._validate(date, this.minMonth, this.minDay,
			$.calendars.local.invalidDate || $.calendars.regional[''].invalidDate);
            return this._correctAdd(date, this._add(date, offset, period), offset, period);
        },


        _add: function (date, offset, period) {
            this._validateLevel++;
            if (period == 'd' || period == 'w') {
                var jd = date.toJD() + offset * (period == 'w' ? this.daysInWeek() : 1);
                var d = date.calendar().fromJD(jd);
                this._validateLevel--;
                return [d.year(), d.month(), d.day()];
            }
            try {
                var y = date.year() + (period == 'y' ? offset : 0);
                var m = date.monthOfYear() + (period == 'm' ? offset : 0);
                var d = date.day();
                var resyncYearMonth = function (calendar) {
                    while (m < calendar.minMonth) {
                        y--;
                        m += calendar.monthsInYear(y);
                    }
                    var yearMonths = calendar.monthsInYear(y);
                    while (m > yearMonths - 1 + calendar.minMonth) {
                        y++;
                        m -= yearMonths;
                        yearMonths = calendar.monthsInYear(y);
                    }
                };
                if (period == 'y') {
                    if (date.month() != this.fromMonthOfYear(y, m)) { // Hebrew
                        m = this.newDate(y, date.month(), this.minDay).monthOfYear();
                    }
                    m = Math.min(m, this.monthsInYear(y));
                    d = Math.min(d, this.daysInMonth(y, this.fromMonthOfYear(y, m)));
                }
                else if (period == 'm') {
                    resyncYearMonth(this);
                    d = Math.min(d, this.daysInMonth(y, this.fromMonthOfYear(y, m)));
                }
                var ymd = [y, this.fromMonthOfYear(y, m), d];
                this._validateLevel--;
                return ymd;
            }
            catch (e) {
                this._validateLevel--;
                throw e;
            }
        },


        _correctAdd: function (date, ymd, offset, period) {
            if (!this.hasYearZero && (period == 'y' || period == 'm')) {
                if (ymd[0] == 0 || // In year zero
					(date.year() > 0) != (ymd[0] > 0)) { // Crossed year zero
                    var adj = { y: [1, 1, 'y'], m: [1, this.monthsInYear(-1), 'm'],
                        w: [this.daysInWeek(), this.daysInYear(-1), 'd'],
                        d: [1, this.daysInYear(-1), 'd']
                    }[period];
                    var dir = (offset < 0 ? -1 : +1);
                    ymd = this._add(date, offset * adj[0] + dir * adj[1], adj[2]);
                }
            }
            return date.date(ymd[0], ymd[1], ymd[2]);
        },


        set: function (date, value, period) {
            this._validate(date, this.minMonth, this.minDay,
			$.calendars.local.invalidDate || $.calendars.regional[''].invalidDate);
            var y = (period == 'y' ? value : date.year());
            var m = (period == 'm' ? value : date.month());
            var d = (period == 'd' ? value : date.day());
            if (period == 'y' || period == 'm') {
                d = Math.min(d, this.daysInMonth(y, m));
            }
            return date.date(y, m, d);
        },


        isValid: function (year, month, day) {
            this._validateLevel++;
            var valid = (this.hasYearZero || year != 0);
            if (valid) {
                var date = this.newDate(year, month, this.minDay);
                valid = (month >= this.minMonth && month - this.minMonth < this.monthsInYear(date)) &&
				(day >= this.minDay && day - this.minDay < this.daysInMonth(date));
            }
            this._validateLevel--;
            return valid;
        },


        toJSDate: function (year, month, day) {
            var date = this._validate(year, month, day,
			$.calendars.local.invalidDate || $.calendars.regional[''].invalidDate);
            return $.calendars.instance().fromJD(this.toJD(date)).toJSDate();
        },


        fromJSDate: function (jsd) {
            return this.fromJD($.calendars.instance().fromJSDate(jsd).toJD());
        },


        _validate: function (year, month, day, error) {
            if (year.year) {
                if (this._validateLevel == 0 && this.name != year.calendar().name) {
                    throw ($.calendars.local.differentCalendars || $.calendars.regional[''].differentCalendars).
					replace(/\{0\}/, this.local.name).replace(/\{1\}/, year.calendar().local.name);
                }
                return year;
            }
            try {
                this._validateLevel++;
                if (this._validateLevel == 1 && !this.isValid(year, month, day)) {
                    throw error.replace(/\{0\}/, this.local.name);
                }
                var date = this.newDate(year, month, day);
                this._validateLevel--;
                return date;
            }
            catch (e) {
                this._validateLevel--;
                throw e;
            }
        }
    });


    function GregorianCalendar(language) {
        this.local = this.regional[language || ''] || this.regional[''];
    }

    GregorianCalendar.prototype = new BaseCalendar;

    $.extend(GregorianCalendar.prototype, {
        name: 'Gregorian', // The calendar name
        jdEpoch: 1721425.5, // Julian date of start of Gregorian epoch: 1 January 0001 CE
        daysPerMonth: [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31], // Days per month in a common year
        hasYearZero: false, // True if has a year zero, false if not
        minMonth: 1, // The minimum month number
        firstMonth: 1, // The first month in the year
        minDay: 1, // The minimum day number

        regional: { // Localisations
            '': {
                name: 'Gregorian', // The calendar name
                epochs: ['BCE', 'CE'],
                monthNames: ['January', 'February', 'March', 'April', 'May', 'June',
			'July', 'August', 'September', 'October', 'November', 'December'],
                monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
                dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
                dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
                dateFormat: 'mm/dd/yyyy', // See format options on parseDate
                firstDay: 0, // The first day of the week, Sun = 0, Mon = 1, ...
                isRTL: false // True if right-to-left language, false if left-to-right
            }
        },


        leapYear: function (year) {
            var date = this._validate(year, this.minMonth, this.minDay,
			$.calendars.local.invalidYear || $.calendars.regional[''].invalidYear);
            var year = date.year() + (date.year() < 0 ? 1 : 0); // No year zero
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        },


        weekOfYear: function (year, month, day) {

            var checkDate = this.newDate(year, month, day);
            checkDate.add(4 - (checkDate.dayOfWeek() || 7), 'd');
            return Math.floor((checkDate.dayOfYear() - 1) / 7) + 1;
        },


        daysInMonth: function (year, month) {
            var date = this._validate(year, month, this.minDay,
			$.calendars.local.invalidMonth || $.calendars.regional[''].invalidMonth);
            return this.daysPerMonth[date.month() - 1] +
			(date.month() == 2 && this.leapYear(date.year()) ? 1 : 0);
        },


        weekDay: function (year, month, day) {
            return (this.dayOfWeek(year, month, day) || 7) < 6;
        },


        toJD: function (year, month, day) {
            var date = this._validate(year, month, day,
			$.calendars.local.invalidDate || $.calendars.regional[''].invalidDate);
            year = date.year();
            month = date.month();
            day = date.day();
            if (year < 0) { year++; } // No year zero

            if (month < 3) {
                month += 12;
                year--;
            }
            var a = Math.floor(year / 100);
            var b = 2 - a + Math.floor(a / 4);
            return Math.floor(365.25 * (year + 4716)) +
			Math.floor(30.6001 * (month + 1)) + day + b - 1524.5;
        },


        fromJD: function (jd) {

            var z = Math.floor(jd + 0.5);
            var a = Math.floor((z - 1867216.25) / 36524.25);
            a = z + 1 + a - Math.floor(a / 4);
            var b = a + 1524;
            var c = Math.floor((b - 122.1) / 365.25);
            var d = Math.floor(365.25 * c);
            var e = Math.floor((b - d) / 30.6001);
            var day = b - d - Math.floor(e * 30.6001);
            var month = e - (e > 13.5 ? 13 : 1);
            var year = c - (month > 2.5 ? 4716 : 4715);
            if (year <= 0) { year--; } // No year zero
            return this.newDate(year, month, day);
        },


        toJSDate: function (year, month, day) {
            var date = this._validate(year, month, day,
			$.calendars.local.invalidDate || $.calendars.regional[''].invalidDate);
            var jsd = new Date(date.year(), date.month() - 1, date.day());
            jsd.setHours(0);
            jsd.setMinutes(0);
            jsd.setSeconds(0);
            jsd.setMilliseconds(0);

            jsd.setHours(jsd.getHours() > 12 ? jsd.getHours() + 2 : 0);
            return jsd;
        },


        fromJSDate: function (jsd) {
            return this.newDate(jsd.getFullYear(), jsd.getMonth() + 1, jsd.getDate());
        }
    });


    $.calendars = new Calendars();


    $.calendars.cdate = CDate;


    $.calendars.baseCalendar = BaseCalendar;


    $.calendars.calendars.gregorian = GregorianCalendar;

})(jQuery);

(function ($) {
    $.extend($.calendars.regional[''], {
        invalidArguments: 'Invalid arguments',
        invalidFormat: 'Cannot format a date from another calendar',
        missingNumberAt: 'Missing number at position {0}',
        unknownNameAt: 'Unknown name at position {0}',
        unexpectedLiteralAt: 'Unexpected literal at position {0}',
        unexpectedText: 'Additional text found at end'
    });
    $.calendars.local = $.calendars.regional[''];

    $.extend($.calendars.cdate.prototype, {


        formatDate: function (format) {
            return this._calendar.formatDate(format || '', this);
        }
    });

    $.extend($.calendars.baseCalendar.prototype, {

        UNIX_EPOCH: $.calendars.instance().newDate(1970, 1, 1).toJD(),
        SECS_PER_DAY: 24 * 60 * 60,
        TICKS_EPOCH: $.calendars.instance().jdEpoch, // 1 January 0001 CE
        TICKS_PER_DAY: 24 * 60 * 60 * 10000000,

        ATOM: 'yyyy-mm-dd',
        COOKIE: 'D, dd M yyyy',
        FULL: 'DD, MM d, yyyy',
        ISO_8601: 'yyyy-mm-dd',
        JULIAN: 'J',
        RFC_822: 'D, d M yy',
        RFC_850: 'DD, dd-M-yy',
        RFC_1036: 'D, d M yy',
        RFC_1123: 'D, d M yyyy',
        RFC_2822: 'D, d M yyyy',
        RSS: 'D, d M yy',
        TICKS: '!',
        TIMESTAMP: '@',
        W3C: 'yyyy-mm-dd',


        formatDate: function (format, date, settings) {
            if (typeof format != 'string') {
                settings = date;
                date = format;
                format = '';
            }
            if (!date) {
                return '';
            }
            if (date.calendar() != this) {
                throw $.calendars.local.invalidFormat || $.calendars.regional[''].invalidFormat;
            }
            format = format || this.local.dateFormat;
            settings = settings || {};
            var dayNamesShort = settings.dayNamesShort || this.local.dayNamesShort;
            var dayNames = settings.dayNames || this.local.dayNames;
            var monthNamesShort = settings.monthNamesShort || this.local.monthNamesShort;
            var monthNames = settings.monthNames || this.local.monthNames;
            var calculateWeek = settings.calculateWeek || this.local.calculateWeek;

            var doubled = function (match, step) {
                var matches = 1;
                while (iFormat + matches < format.length && format.charAt(iFormat + matches) == match) {
                    matches++;
                }
                iFormat += matches - 1;
                return Math.floor(matches / (step || 1)) > 1;
            };

            var formatNumber = function (match, value, len, step) {
                var num = '' + value;
                if (doubled(match, step)) {
                    while (num.length < len) {
                        num = '0' + num;
                    }
                }
                return num;
            };

            var formatName = function (match, value, shortNames, longNames) {
                return (doubled(match) ? longNames[value] : shortNames[value]);
            };
            var output = '';
            var literal = false;
            for (var iFormat = 0; iFormat < format.length; iFormat++) {
                if (literal) {
                    if (format.charAt(iFormat) == "'" && !doubled("'")) {
                        literal = false;
                    }
                    else {
                        output += format.charAt(iFormat);
                    }
                }
                else {
                    switch (format.charAt(iFormat)) {
                        case 'd': output += formatNumber('d', date.day(), 2); break;
                        case 'D': output += formatName('D', date.dayOfWeek(),
						dayNamesShort, dayNames); break;
                        case 'o': output += formatNumber('o', date.dayOfYear(), 3); break;
                        case 'w': output += formatNumber('w', date.weekOfYear(), 2); break;
                        case 'm': output += formatNumber('m', date.month(), 2); break;
                        case 'M': output += formatName('M', date.month() - this.minMonth,
						monthNamesShort, monthNames); break;
                        case 'y':
                            output += (doubled('y', 2) ? date.year() :
							(date.year() % 100 < 10 ? '0' : '') + date.year() % 100);
                            break;
                        case 'Y':
                            doubled('Y', 2);
                            output += date.formatYear();
                            break;
                        case 'J': output += date.toJD(); break;
                        case '@': output += (date.toJD() - this.UNIX_EPOCH) * this.SECS_PER_DAY; break;
                        case '!': output += (date.toJD() - this.TICKS_EPOCH) * this.TICKS_PER_DAY; break;
                        case "'":
                            if (doubled("'")) {
                                output += "'";
                            }
                            else {
                                literal = true;
                            }
                            break;
                        default:
                            output += format.charAt(iFormat);
                    }
                }
            }
            return output;
        },


        parseDate: function (format, value, settings) {
            if (value == null) {
                throw $.calendars.local.invalidArguments || $.calendars.regional[''].invalidArguments;
            }
            value = (typeof value == 'object' ? value.toString() : value + '');
            if (value == '') {
                return null;
            }
            format = format || this.local.dateFormat;
            settings = settings || {};
            var shortYearCutoff = settings.shortYearCutoff || this.shortYearCutoff;
            shortYearCutoff = (typeof shortYearCutoff != 'string' ? shortYearCutoff :
			this.today().year() % 100 + parseInt(shortYearCutoff, 10));
            var dayNamesShort = settings.dayNamesShort || this.local.dayNamesShort;
            var dayNames = settings.dayNames || this.local.dayNames;
            var monthNamesShort = settings.monthNamesShort || this.local.monthNamesShort;
            var monthNames = settings.monthNames || this.local.monthNames;
            var jd = -1;
            var year = -1;
            var month = -1;
            var day = -1;
            var doy = -1;
            var shortYear = false;
            var literal = false;

            var doubled = function (match, step) {
                var matches = 1;
                while (iFormat + matches < format.length && format.charAt(iFormat + matches) == match) {
                    matches++;
                }
                iFormat += matches - 1;
                return Math.floor(matches / (step || 1)) > 1;
            };

            var getNumber = function (match, step) {
                var isDoubled = doubled(match, step);
                var size = [2, 3, isDoubled ? 4 : 2, isDoubled ? 4 : 2, 10, 11, 20]['oyYJ@!'.indexOf(match) + 1];
                var digits = new RegExp('^-?\\d{1,' + size + '}');
                var num = value.substring(iValue).match(digits);
                if (!num) {
                    throw ($.calendars.local.missingNumberAt || $.calendars.regional[''].missingNumberAt).
					replace(/\{0\}/, iValue);
                }
                iValue += num[0].length;
                return parseInt(num[0], 10);
            };

            var calendar = this;
            var getName = function (match, shortNames, longNames, step) {
                var names = (doubled(match, step) ? longNames : shortNames);
                for (var i = 0; i < names.length; i++) {
                    if (value.substr(iValue, names[i].length) == names[i]) {
                        iValue += names[i].length;
                        return i + calendar.minMonth;
                    }
                }
                throw ($.calendars.local.unknownNameAt || $.calendars.regional[''].unknownNameAt).
				replace(/\{0\}/, iValue);
            };

            var checkLiteral = function () {
                if (value.charAt(iValue) != format.charAt(iFormat)) {
                    throw ($.calendars.local.unexpectedLiteralAt ||
					$.calendars.regional[''].unexpectedLiteralAt).replace(/\{0\}/, iValue);
                }
                iValue++;
            };
            var iValue = 0;
            for (var iFormat = 0; iFormat < format.length; iFormat++) {
                if (literal) {
                    if (format.charAt(iFormat) == "'" && !doubled("'")) {
                        literal = false;
                    }
                    else {
                        checkLiteral();
                    }
                }
                else {
                    switch (format.charAt(iFormat)) {
                        case 'd': day = getNumber('d'); break;
                        case 'D': getName('D', dayNamesShort, dayNames); break;
                        case 'o': doy = getNumber('o'); break;
                        case 'w': getNumber('w'); break;
                        case 'm': month = getNumber('m'); break;
                        case 'M': month = getName('M', monthNamesShort, monthNames); break;
                        case 'y':
                            var iSave = iFormat;
                            shortYear = !doubled('y', 2);
                            iFormat = iSave;
                            year = getNumber('y', 2);
                            break;
                        case 'Y': year = getNumber('Y', 2); break;
                        case 'J':
                            jd = getNumber('J') + 0.5;
                            if (value.charAt(iValue) == '.') {
                                iValue++;
                                getNumber('J');
                            }
                            break;
                        case '@': jd = getNumber('@') / this.SECS_PER_DAY + this.UNIX_EPOCH; break;
                        case '!': jd = getNumber('!') / this.TICKS_PER_DAY + this.TICKS_EPOCH; break;
                        case '*': iValue = value.length; break;
                        case "'":
                            if (doubled("'")) {
                                checkLiteral();
                            }
                            else {
                                literal = true;
                            }
                            break;
                        default: checkLiteral();
                    }
                }
            }
            if (iValue < value.length) {
                throw $.calendars.local.unexpectedText || $.calendars.regional[''].unexpectedText;
            }
            if (year == -1) {
                year = this.today().year();
            }
            else if (year < 100 && shortYear) {
                year += (shortYearCutoff == -1 ? 1900 : this.today().year() -
				this.today().year() % 100 - (year <= shortYearCutoff ? 0 : 100));
            }
            if (doy > -1) {
                month = 1;
                day = doy;
                for (var dim = this.daysInMonth(year, month); day > dim; dim = this.daysInMonth(year, month)) {
                    month++;
                    day -= dim;
                }
            }
            return (jd > -1 ? this.fromJD(jd) : this.newDate(year, month, day));
        },


        determineDate: function (dateSpec, defaultDate, currentDate, dateFormat, settings) {
            if (currentDate && typeof currentDate != 'object') {
                settings = dateFormat;
                dateFormat = currentDate;
                currentDate = null;
            }
            if (typeof dateFormat != 'string') {
                settings = dateFormat;
                dateFormat = '';
            }
            var calendar = this;
            var offsetString = function (offset) {
                try {
                    return calendar.parseDate(dateFormat, offset, settings);
                }
                catch (e) {
                    // Ignore
                }
                offset = offset.toLowerCase();
                var date = (offset.match(/^c/) && currentDate ?
				currentDate.newDate() : null) || calendar.today();
                var pattern = /([+-]?[0-9]+)\s*(d|w|m|y)?/g;
                var matches = pattern.exec(offset);
                while (matches) {
                    date.add(parseInt(matches[1], 10), matches[2] || 'd');
                    matches = pattern.exec(offset);
                }
                return date;
            };
            defaultDate = (defaultDate ? defaultDate.newDate() : null);
            dateSpec = (dateSpec == null ? defaultDate :
			(typeof dateSpec == 'string' ? offsetString(dateSpec) : (typeof dateSpec == 'number' ?
			(isNaN(dateSpec) || dateSpec == Infinity || dateSpec == -Infinity ? defaultDate :
			calendar.today().add(dateSpec, 'd')) : calendar.newDate(dateSpec))));
            return dateSpec;
        }
    });

})(jQuery);


(function ($) {


    function CalendarsPicker() {
        this._defaults = {
            calendar: $.calendars.instance(),
            pickerClass: '',
            showOnFocus: true,
            showTrigger: null,
            showAnim: 'show',
            showOptions: {},
            showSpeed: 'normal',
            popupContainer: null,
            alignment: 'bottom',

            fixedWeeks: false,
            firstDay: null,

            calculateWeek: null,
            monthsToShow: 1,
            monthsOffset: 0,
            monthsToStep: 1,
            monthsToJump: 12,
            useMouseWheel: true,
            changeMonth: true,
            yearRange: 'c-10:c+10',

            showOtherMonths: false,
            selectOtherMonths: false,
            defaultDate: null,
            selectDefaultDate: false,
            minDate: null,
            maxDate: null,
            dateFormat: null,
            autoSize: false,
            rangeSelect: false,
            rangeSeparator: ' - ',
            multiSelect: 0,
            multiSeparator: ',',
            onDate: null,
            onShow: null,
            onChangeMonthYear: null,
            onSelect: null,
            onClose: null,
            altField: null,
            altFormat: null,
            constrainInput: true,
            commandsAsDateFormat: false,
            commands: this.commands
        };
        this.regional = {
            '': {
                renderer: this.defaultRenderer,
                prevText: 'قبلی &gt;',
                prevStatus: 'نمایش ماه قبل',
                prevJumpText: '&lt;&lt;',
                prevJumpStatus: 'نمایش سال قبل',
                nextText: '&lt; بعدی ',
                nextStatus: 'نمایش ماه بعد',
                nextJumpText: '&gt;&gt;',
                nextJumpStatus: 'نمایش سال بعد',
                currentText: 'جاری',
                currentStatus: 'نمایش ماه جاری',
                todayText: 'امروز',
                todayStatus: 'امروز را نشان بده',
                clearText: 'پاک کردن',
                clearStatus: 'تمام تاریخها پاک میشوند',
                closeText: 'بستن',
                closeStatus: 'بستن تقویم',
                yearStatus: 'تغییر سال',
                monthStatus: 'تغییر ماه',
                weekText: 'Wk',
                weekStatus: 'Week of the year',
                dayStatus: 'انتخاب DD, M d, yyyy',
                defaultStatus: 'Select a date',
                isRTL: false
            }
        };
        $.extend(this._defaults, this.regional['']);
        this._disabled = [];
    }

    $.extend(CalendarsPicker.prototype, {
        dataName: 'calendarsPicker',


        markerClass: 'hasCalendarsPicker',

        _popupClass: 'calendars-popup',
        _triggerClass: 'calendars-trigger',
        _disableClass: 'calendars-disable',
        _coverClass: 'calendars-cover',
        _monthYearClass: 'calendars-month-year',
        _curMonthClass: 'calendars-month-',
        _anyYearClass: 'calendars-any-year',
        _curDoWClass: 'calendars-dow-',

        commands: {

            prev: { text: 'prevText', status: 'prevStatus',
                keystroke: { keyCode: 33 },
                enabled: function (inst) {
                    var minDate = inst.curMinDate();
                    return (!minDate || inst.drawDate.newDate().
					add(1 - inst.get('monthsToStep') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay).add(-1, 'd').compareTo(minDate) != -1);
                },
                date: function (inst) {
                    return inst.drawDate.newDate().
					add(-inst.get('monthsToStep') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay);
                },
                action: function (inst) {
                    $.calendars.picker.changeMonth(this, -inst.get('monthsToStep'));
                }
            },
            prevJump: { text: 'prevJumpText', status: 'prevJumpStatus',
                keystroke: { keyCode: 33, ctrlKey: true },
                enabled: function (inst) {
                    var minDate = inst.curMinDate();
                    return (!minDate || inst.drawDate.newDate().
					add(1 - inst.get('monthsToJump') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay).add(-1, 'd').compareTo(minDate) != -1);
                },
                date: function (inst) {
                    return inst.drawDate.newDate().
					add(-inst.get('monthsToJump') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay);
                },
                action: function (inst) {
                    $.calendars.picker.changeMonth(this, -inst.get('monthsToJump'));
                }
            },
            next: { text: 'nextText', status: 'nextStatus',
                keystroke: { keyCode: 34 },
                enabled: function (inst) {
                    var maxDate = inst.get('maxDate');
                    return (!maxDate || inst.drawDate.newDate().
					add(inst.get('monthsToStep') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay).compareTo(maxDate) != +1);
                },
                date: function (inst) {
                    return inst.drawDate.newDate().
					add(inst.get('monthsToStep') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay);
                },
                action: function (inst) {
                    $.calendars.picker.changeMonth(this, inst.get('monthsToStep'));
                }
            },
            nextJump: { text: 'nextJumpText', status: 'nextJumpStatus',
                keystroke: { keyCode: 34, ctrlKey: true },
                enabled: function (inst) {
                    var maxDate = inst.get('maxDate');
                    return (!maxDate || inst.drawDate.newDate().
					add(inst.get('monthsToJump') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay).compareTo(maxDate) != +1);
                },
                date: function (inst) {
                    return inst.drawDate.newDate().
					add(inst.get('monthsToJump') - inst.get('monthsOffset'), 'm').
					day(inst.get('calendar').minDay);
                },
                action: function (inst) {
                    $.calendars.picker.changeMonth(this, inst.get('monthsToJump'));
                }
            },
            current: { text: 'currentText', status: 'currentStatus',
                keystroke: { keyCode: 36, ctrlKey: true },
                enabled: function (inst) {
                    var minDate = inst.curMinDate();
                    var maxDate = inst.get('maxDate');
                    var curDate = inst.selectedDates[0] || inst.get('calendar').today();
                    return (!minDate || curDate.compareTo(minDate) != -1) &&
					(!maxDate || curDate.compareTo(maxDate) != +1);
                },
                date: function (inst) {
                    return inst.selectedDates[0] || inst.get('calendar').today();
                },
                action: function (inst) {
                    var curDate = inst.selectedDates[0] || inst.get('calendar').today();
                    $.calendars.picker.showMonth(this, curDate.year(), curDate.month());
                }
            },
            today: { text: 'todayText', status: 'todayStatus',
                keystroke: { keyCode: 36, ctrlKey: true },
                enabled: function (inst) {
                    var minDate = inst.curMinDate();
                    var maxDate = inst.get('maxDate');
                    return (!minDate || inst.get('calendar').today().compareTo(minDate) != -1) &&
					(!maxDate || inst.get('calendar').today().compareTo(maxDate) != +1);
                },
                date: function (inst) { return inst.get('calendar').today(); },
                action: function (inst) { $.calendars.picker.showMonth(this); }
            },
            clear: { text: 'clearText', status: 'clearStatus',
                keystroke: { keyCode: 35, ctrlKey: true },
                enabled: function (inst) { return true; },
                date: function (inst) { return null; },
                action: function (inst) { $.calendars.picker.clear(this); }
            },
            close: { text: 'closeText', status: 'closeStatus',
                keystroke: { keyCode: 27 }, // Escape
                enabled: function (inst) { return true; },
                date: function (inst) { return null; },
                action: function (inst) { $.calendars.picker.hide(this); }
            },
            prevWeek: { text: 'prevWeekText', status: 'prevWeekStatus',
                keystroke: { keyCode: 38, ctrlKey: true },
                enabled: function (inst) {
                    var minDate = inst.curMinDate();
                    return (!minDate || inst.drawDate.newDate().
					add(-inst.get('calendar').daysInWeek(), 'd').compareTo(minDate) != -1);
                },
                date: function (inst) {
                    return inst.drawDate.newDate().
				add(-inst.get('calendar').daysInWeek(), 'd');
                },
                action: function (inst) {
                    $.calendars.picker.changeDay(
				this, -inst.get('calendar').daysInWeek());
                }
            },
            prevDay: { text: 'prevDayText', status: 'prevDayStatus', // Previous day
                keystroke: { keyCode: 37, ctrlKey: true }, // Ctrl + Left
                enabled: function (inst) {
                    var minDate = inst.curMinDate();
                    return (!minDate || inst.drawDate.newDate().add(-1, 'd').
					compareTo(minDate) != -1);
                },
                date: function (inst) { return inst.drawDate.newDate().add(-1, 'd'); },
                action: function (inst) { $.calendars.picker.changeDay(this, -1); }
            },
            nextDay: { text: 'nextDayText', status: 'nextDayStatus', // Next day
                keystroke: { keyCode: 39, ctrlKey: true }, // Ctrl + Right
                enabled: function (inst) {
                    var maxDate = inst.get('maxDate');
                    return (!maxDate || inst.drawDate.newDate().add(1, 'd').
					compareTo(maxDate) != +1);
                },
                date: function (inst) { return inst.drawDate.newDate().add(1, 'd'); },
                action: function (inst) { $.calendars.picker.changeDay(this, 1); }
            },
            nextWeek: { text: 'nextWeekText', status: 'nextWeekStatus', // Next week
                keystroke: { keyCode: 40, ctrlKey: true }, // Ctrl + Down
                enabled: function (inst) {
                    var maxDate = inst.get('maxDate');
                    return (!maxDate || inst.drawDate.newDate().
					add(inst.get('calendar').daysInWeek(), 'd').compareTo(maxDate) != +1);
                },
                date: function (inst) {
                    return inst.drawDate.newDate().
				add(inst.get('calendar').daysInWeek(), 'd');
                },
                action: function (inst) {
                    $.calendars.picker.changeDay(
				this, inst.get('calendar').daysInWeek());
                }
            }
        },


        defaultRenderer: {

            picker: '<div class="calendars">' +
		'<div class="calendars-nav">{link:prev}{link:today}{link:next}</div>{months}' +
		'{popup:start}<div class="calendars-ctrl">{link:clear}{link:close}</div>{popup:end}' +
		'<div class="calendars-clear-fix"></div></div>',

            monthRow: '<div class="calendars-month-row">{months}</div>',

            month: '<div class="calendars-month"><div class="calendars-month-header">{monthHeader}</div>' +
		'<table><thead>{weekHeader}</thead><tbody>{weeks}</tbody></table></div>',

            weekHeader: '<tr>{days}</tr>',
            // Individual day header: '{day}' to insert day name
            dayHeader: '<th>{day}</th>',
            // One week of the month: '{days}' to insert the week's days, '{weekOfYear}' to insert week of year
            week: '<tr>{days}</tr>',
            // An individual day: '{day}' to insert day value
            day: '<td>{day}</td>',
            // jQuery selector, relative to picker, for a single month
            monthSelector: '.calendars-month',
            // jQuery selector, relative to picker, for individual days
            daySelector: 'td',
            // Class for right-to-left (RTL) languages
            rtlClass: 'calendars-rtl',
            // Class for multi-month datepickers
            multiClass: 'calendars-multi',
            // Class for selectable dates
            defaultClass: '',
            // Class for currently selected dates
            selectedClass: 'calendars-selected',
            // Class for highlighted dates
            highlightedClass: 'calendars-highlight',
            // Class for today
            todayClass: 'calendars-today',
            // Class for days from other months
            otherMonthClass: 'calendars-other-month',
            // Class for days on weekends
            weekendClass: 'calendars-weekend',
            // Class prefix for commands
            commandClass: 'calendars-cmd',
            // Extra class(es) for commands that are buttons
            commandButtonClass: '',
            // Extra class(es) for commands that are links
            commandLinkClass: '',
            // Class for disabled commands
            disabledClass: 'calendars-disabled'
        },


        setDefaults: function (settings) {
            $.extend(this._defaults, settings || {});
            return this;
        },


        _attachPicker: function (target, settings) {
            target = $(target);
            if (target.hasClass(this.markerClass)) {
                return;
            }
            target.addClass(this.markerClass);
            var inst = { target: target, selectedDates: [], drawDate: null, pickingRange: false,
                inline: ($.inArray(target[0].nodeName.toLowerCase(), ['div', 'span']) > -1),
                get: function (name) { // Get a setting value, defaulting if necessary
                    var value = this.settings[name] !== undefined ?
					this.settings[name] : $.calendars.picker._defaults[name];
                    if ($.inArray(name, ['defaultDate', 'minDate', 'maxDate']) > -1) { // Decode date settings
                        value = this.get('calendar').determineDate(
						value, null, this.selectedDates[0], this.get('dateFormat'), inst.getConfig());
                    }
                    else if (name == 'dateFormat') {
                        value = value || this.get('calendar').local.dateFormat;
                    }
                    return value;
                },
                curMinDate: function () {
                    return (this.pickingRange ? this.selectedDates[0] : this.get('minDate'));
                },
                getConfig: function () {
                    return { dayNamesShort: this.get('dayNamesShort'), dayNames: this.get('dayNames'),
                        monthNamesShort: this.get('monthNamesShort'), monthNames: this.get('monthNames'),
                        calculateWeek: this.get('calculateWeek'),
                        shortYearCutoff: this.get('shortYearCutoff')
                    };
                }
            };
            $.data(target[0], this.dataName, inst);
            var inlineSettings = ($.fn.metadata ? target.metadata() : {});
            inst.settings = $.extend({}, settings || {}, inlineSettings || {});
            if (inst.inline) {
                this._update(target[0]);
                if ($.fn.mousewheel) {
                    target.mousewheel(this._doMouseWheel);
                }
            }
            else {
                this._attachments(target, inst);
                target.bind('keydown.' + this.dataName, this._keyDown).
				bind('keypress.' + this.dataName, this._keyPress).
				bind('keyup.' + this.dataName, this._keyUp);
                if (target.attr('disabled')) {
                    this.disable(target[0]);
                }
            }
        },


        options: function (target, name) {
            var inst = $.data(target, this.dataName);
            return (inst ? (name ? (name == 'all' ?
			inst.settings : inst.settings[name]) : $.calendars.picker._defaults) : {});
        },


        option: function (target, settings, value) {
            target = $(target);
            if (!target.hasClass(this.markerClass)) {
                return;
            }
            settings = settings || {};
            if (typeof settings == 'string') {
                var name = settings;
                settings = {};
                settings[name] = value;
            }
            var inst = $.data(target[0], this.dataName);
            if (settings.calendar && settings.calendar != inst.get('calendar')) {
                var discardDate = function (name) {
                    return (typeof inst.settings[name] == 'object' ? null : inst.settings[name]);
                };
                settings = $.extend({ defaultDate: discardDate('defaultDate'),
                    minDate: discardDate('minDate'), maxDate: discardDate('maxDate')
                }, settings);
                inst.selectedDates = [];
                inst.drawDate = null;
            }
            var dates = inst.selectedDates;
            extendRemove(inst.settings, settings);
            this.setDate(target[0], dates, null, false, true);
            inst.pickingRange = false;
            var calendar = inst.get('calendar');
            inst.drawDate = this._checkMinMax(
			(settings.defaultDate ? inst.get('defaultDate') : inst.drawDate) ||
			inst.get('defaultDate') || calendar.today(), inst).newDate();
            if (!inst.inline) {
                this._attachments(target, inst);
            }
            if (inst.inline || inst.div) {
                this._update(target[0]);
            }
        },


        _attachments: function (target, inst) {
            target.unbind('focus.' + this.dataName);
            if (inst.get('showOnFocus')) {
                target.bind('focus.' + this.dataName, this.show);
            }
            if (inst.trigger) {
                inst.trigger.remove();
            }
            var trigger = inst.get('showTrigger');
            inst.trigger = (!trigger ? $([]) :
			$(trigger).clone().addClass(this._triggerClass)
				[inst.get('isRTL') ? 'insertBefore' : 'insertAfter'](target).
				click(function () {
				    if (!$.calendars.picker.isDisabled(target[0])) {
				        $.calendars.picker[$.calendars.picker.curInst == inst ?
							'hide' : 'show'](target[0]);
				    }
				}));
            this._autoSize(target, inst);
            var dates = this._extractDates(inst, target.val());
            if (dates) {
                this.setDate(target[0], dates, null, true);
            }
            if (inst.get('selectDefaultDate') && inst.get('defaultDate') &&
				inst.selectedDates.length == 0) {
                var calendar = inst.get('calendar');
                this.setDate(target[0],
				(inst.get('defaultDate') || calendar.today()).newDate());
            }
        },


        _autoSize: function (target, inst) {
            if (inst.get('autoSize') && !inst.inline) {
                var calendar = inst.get('calendar');
                var date = calendar.newDate(2009, 10, 20); // Ensure double digits
                var dateFormat = inst.get('dateFormat');
                if (dateFormat.match(/[DM]/)) {
                    var findMax = function (names) {
                        var max = 0;
                        var maxI = 0;
                        for (var i = 0; i < names.length; i++) {
                            if (names[i].length > max) {
                                max = names[i].length;
                                maxI = i;
                            }
                        }
                        return maxI;
                    };
                    date.month(findMax(calendar.local[dateFormat.match(/MM/) ? // Longest month
					'monthNames' : 'monthNamesShort']) + 1);
                    date.day(findMax(calendar.local[dateFormat.match(/DD/) ? // Longest day
					'dayNames' : 'dayNamesShort']) + 20 - date.dayOfWeek());
                }
                inst.target.attr('size', date.formatDate(dateFormat).length);
            }
        },


        destroy: function (target) {
            target = $(target);
            if (!target.hasClass(this.markerClass)) {
                return;
            }
            var inst = $.data(target[0], this.dataName);
            if (inst.trigger) {
                inst.trigger.remove();
            }
            target.removeClass(this.markerClass).empty().unbind('.' + this.dataName);
            if (inst.inline && $.fn.mousewheel) {
                target.unmousewheel();
            }
            if (!inst.inline && inst.get('autoSize')) {
                target.removeAttr('size');
            }
            $.removeData(target[0], this.dataName);
        },


        multipleEvents: function (fns) {
            var funcs = arguments;
            return function (args) {
                for (var i = 0; i < funcs.length; i++) {
                    funcs[i].apply(this, arguments);
                }
            };
        },


        enable: function (target) {
            var $target = $(target);
            if (!$target.hasClass(this.markerClass)) {
                return;
            }
            var inst = $.data(target, this.dataName);
            if (inst.inline)
                $target.children('.' + this._disableClass).remove().end().
				find('button,select').attr('disabled', '').end().
				find('a').attr('href', 'javascript:void(0)');
            else {
                target.disabled = false;
                inst.trigger.filter('button.' + this._triggerClass).
				attr('disabled', '').end().
				filter('img.' + this._triggerClass).
				css({ opacity: '1.0', cursor: '' });
            }
            this._disabled = $.map(this._disabled,
			function (value) { return (value == target ? null : value); }); // Delete entry
        },


        disable: function (target) {
            var $target = $(target);
            if (!$target.hasClass(this.markerClass))
                return;
            var inst = $.data(target, this.dataName);
            if (inst.inline) {
                var inline = $target.children(':last');
                var offset = inline.offset();
                var relOffset = { left: 0, top: 0 };
                inline.parents().each(function () {
                    if ($(this).css('position') == 'relative') {
                        relOffset = $(this).offset();
                        return false;
                    }
                });
                var zIndex = $target.css('zIndex');
                zIndex = (zIndex == 'auto' ? 0 : parseInt(zIndex, 10)) + 1;
                $target.prepend('<div class="' + this._disableClass + '" style="' +
				'width: ' + inline.outerWidth() + 'px; height: ' + inline.outerHeight() +
				'px; left: ' + (offset.left - relOffset.left) + 'px; top: ' +
				(offset.top - relOffset.top) + 'px; z-index: ' + zIndex + '"></div>').
				find('button,select').attr('disabled', 'disabled').end().
				find('a').removeAttr('href');
            }
            else {
                target.disabled = true;
                inst.trigger.filter('button.' + this._triggerClass).
				attr('disabled', 'disabled').end().
				filter('img.' + this._triggerClass).
				css({ opacity: '0.5', cursor: 'default' });
            }
            this._disabled = $.map(this._disabled,
			function (value) { return (value == target ? null : value); }); // Delete entry
            this._disabled.push(target);
        },


        isDisabled: function (target) {
            return (target && $.inArray(target, this._disabled) > -1);
        },


        show: function (target) {
            target = target.target || target;
            var inst = $.data(target, $.calendars.picker.dataName);
            if ($.calendars.picker.curInst == inst) {
                return;
            }
            if ($.calendars.picker.curInst) {
                $.calendars.picker.hide($.calendars.picker.curInst, true);
            }
            if (inst) {
                // Retrieve existing date(s)
                inst.lastVal = null;
                inst.selectedDates = $.calendars.picker._extractDates(inst, $(target).val());
                inst.pickingRange = false;
                inst.drawDate = $.calendars.picker._checkMinMax((inst.selectedDates[0] ||
				inst.get('defaultDate') || inst.get('calendar').today()).newDate(), inst);
                $.calendars.picker.curInst = inst;
                // Generate content
                $.calendars.picker._update(target, true);
                // Adjust position before showing
                var offset = $.calendars.picker._checkOffset(inst);
                inst.div.css({ left: offset.left, top: offset.top });
                // And display
                var showAnim = inst.get('showAnim');
                var showSpeed = inst.get('showSpeed');
                showSpeed = (showSpeed == 'normal' && $.ui && $.ui.version >= '1.8' ?
				'_default' : showSpeed);
                var postProcess = function () {
                    var cover = inst.div.find('.' + $.calendars.picker._coverClass); // IE6- only
                    if (cover.length) {
                        var borders = $.calendars.picker._getBorders(inst.div);
                        cover.css({ left: -borders[0], top: -borders[1],
                            width: inst.div.outerWidth() + borders[0],
                            height: inst.div.outerHeight() + borders[1]
                        });
                    }
                };
                if ($.effects && $.effects[showAnim]) {
                    var data = inst.div.data(); // Update old effects data
                    for (var key in data) {
                        if (key.match(/^ec\.storage\./)) {
                            data[key] = inst._mainDiv.css(key.replace(/ec\.storage\./, ''));
                        }
                    }
                    inst.div.data(data).show(showAnim, inst.get('showOptions'), showSpeed, postProcess);
                }
                else {
                    inst.div[showAnim || 'show']((showAnim ? showSpeed : ''), postProcess);
                }
                if (!showAnim) {
                    postProcess();
                }
            }
        },


        _extractDates: function (inst, datesText) {
            if (datesText == inst.lastVal) {
                return;
            }
            inst.lastVal = datesText;
            var calendar = inst.get('calendar');
            var dateFormat = inst.get('dateFormat');
            var multiSelect = inst.get('multiSelect');
            var rangeSelect = inst.get('rangeSelect');
            datesText = datesText.split(multiSelect ? inst.get('multiSeparator') :
			(rangeSelect ? inst.get('rangeSeparator') : '\x00'));
            var dates = [];
            for (var i = 0; i < datesText.length; i++) {
                try {
                    var date = calendar.parseDate(dateFormat, datesText[i]);
                    if (date) {
                        var found = false;
                        for (var j = 0; j < dates.length; j++) {
                            if (dates[j].compareTo(date) == 0) {
                                found = true;
                                break;
                            }
                        }
                        if (!found) {
                            dates.push(date);
                        }
                    }
                }
                catch (e) {
                    // Ignore
                }
            }
            dates.splice(multiSelect || (rangeSelect ? 2 : 1), dates.length);
            if (rangeSelect && dates.length == 1) {
                dates[1] = dates[0];
            }
            return dates;
        },


        _update: function (target, hidden) {
            target = $(target.target || target);
            var inst = $.data(target[0], $.calendars.picker.dataName);
            if (inst) {
                if (inst.inline || $.calendars.picker.curInst == inst) {
                    var onChange = inst.get('onChangeMonthYear');
                    if (onChange && (!inst.prevDate || inst.prevDate.year() != inst.drawDate.year() ||
						inst.prevDate.month() != inst.drawDate.month())) {
                        onChange.apply(target[0], [inst.drawDate.year(), inst.drawDate.month()]);
                    }
                }
                if (inst.inline) {
                    target.html(this._generateContent(target[0], inst));
                }
                else if ($.calendars.picker.curInst == inst) {
                    if (!inst.div) {
                        inst.div = $('<div></div>').addClass(this._popupClass).
						css({ display: (hidden ? 'none' : 'static'), position: 'absolute',
						    left: target.offset().left,
						    top: target.offset().top + target.outerHeight()
						}).
						appendTo($(inst.get('popupContainer') || 'body'));
                        if ($.fn.mousewheel) {
                            inst.div.mousewheel(this._doMouseWheel);
                        }
                    }
                    inst.div.html(this._generateContent(target[0], inst));
                    target.focus();
                }
            }
        },

        /* Update the input field and any alternate field with the current dates.
        @param  target  (element) the control to use
        @param  keyUp   (boolean, internal) true if coming from keyUp processing */
        _updateInput: function (target, keyUp) {
            var inst = $.data(target, this.dataName);
            if (inst) {
                var value = '';
                var altValue = '';
                var sep = (inst.get('multiSelect') ? inst.get('multiSeparator') :
				inst.get('rangeSeparator'));
                var calendar = inst.get('calendar');
                var dateFormat = inst.get('dateFormat') || calendar.local.dateFormat;
                var altFormat = inst.get('altFormat') || dateFormat;
                for (var i = 0; i < inst.selectedDates.length; i++) {
                    value += (keyUp ? '' : (i > 0 ? sep : '') +
					calendar.formatDate(dateFormat, inst.selectedDates[i]));
                    altValue += (i > 0 ? sep : '') +
					calendar.formatDate(altFormat, inst.selectedDates[i]);
                }
                if (!inst.inline && !keyUp) {
                    $(target).val(value);
                }
                $(inst.get('altField')).val(altValue);
                var onSelect = inst.get('onSelect');
                if (onSelect && !keyUp && !inst.inSelect) {
                    inst.inSelect = true; // Prevent endless loops
                    onSelect.apply(target, [inst.selectedDates]);
                    inst.inSelect = false;
                }
            }
        },


        _getBorders: function (elem) {
            var convert = function (value) {
                var extra = ($.browser.msie ? 1 : 0);
                return { thin: 1 + extra, medium: 3 + extra, thick: 5 + extra}[value] || value;
            };
            return [parseFloat(convert(elem.css('border-left-width'))),
			parseFloat(convert(elem.css('border-top-width')))];
        },


        _checkOffset: function (inst) {
            var base = (inst.target.is(':hidden') && inst.trigger ? inst.trigger : inst.target);
            var offset = base.offset();
            var isFixed = false;
            $(inst.target).parents().each(function () {
                isFixed |= $(this).css('position') == 'fixed';
                return !isFixed;
            });
            if (isFixed && $.browser.opera) { // Correction for Opera when fixed and scrolled
                offset.left -= document.documentElement.scrollLeft;
                offset.top -= document.documentElement.scrollTop;
            }
            var browserWidth = (!$.browser.mozilla || document.doctype ?
			document.documentElement.clientWidth : 0) || document.body.clientWidth;
            var browserHeight = (!$.browser.mozilla || document.doctype ?
			document.documentElement.clientHeight : 0) || document.body.clientHeight;
            if (browserWidth == 0) {
                return offset;
            }
            var alignment = inst.get('alignment');
            var isRTL = inst.get('isRTL');
            var scrollX = document.documentElement.scrollLeft || document.body.scrollLeft;
            var scrollY = document.documentElement.scrollTop || document.body.scrollTop;
            var above = offset.top - inst.div.outerHeight() -
			(isFixed && $.browser.opera ? document.documentElement.scrollTop : 0);
            var below = offset.top + base.outerHeight();
            var alignL = offset.left;
            var alignR = offset.left + base.outerWidth() - inst.div.outerWidth() -
			(isFixed && $.browser.opera ? document.documentElement.scrollLeft : 0);
            var tooWide = (offset.left + inst.div.outerWidth() - scrollX) > browserWidth;
            var tooHigh = (offset.top + inst.target.outerHeight() + inst.div.outerHeight() -
			scrollY) > browserHeight;
            if (alignment == 'topLeft') {
                offset = { left: alignL, top: above };
            }
            else if (alignment == 'topRight') {
                offset = { left: alignR, top: above };
            }
            else if (alignment == 'bottomLeft') {
                offset = { left: alignL, top: below };
            }
            else if (alignment == 'bottomRight') {
                offset = { left: alignR, top: below };
            }
            else if (alignment == 'top') {
                offset = { left: (isRTL || tooWide ? alignR : alignL), top: above };
            }
            else { // bottom
                offset = { left: (isRTL || tooWide ? alignR : alignL),
                    top: (tooHigh ? above : below)
                };
            }
            offset.left = Math.max((isFixed ? 0 : scrollX), offset.left - (isFixed ? scrollX : 0));
            offset.top = Math.max((isFixed ? 0 : scrollY), offset.top - (isFixed ? scrollY : 0));
            return offset;
        },


        _checkExternalClick: function (event) {
            if (!$.calendars.picker.curInst) {
                return;
            }
            var target = $(event.target);
            if (!target.parents().andSelf().hasClass($.calendars.picker._popupClass) &&
				!target.hasClass($.calendars.picker.markerClass) &&
				!target.parents().andSelf().hasClass($.calendars.picker._triggerClass)) {
                $.calendars.picker.hide($.calendars.picker.curInst);
            }
        },


        hide: function (target, immediate) {
            var inst = $.data(target, this.dataName) || target;
            if (inst && inst == $.calendars.picker.curInst) {
                var showAnim = (immediate ? '' : inst.get('showAnim'));
                var showSpeed = inst.get('showSpeed');
                showSpeed = (showSpeed == 'normal' && $.ui && $.ui.version >= '1.8' ?
				'_default' : showSpeed);
                var postProcess = function () {
                    inst.div.remove();
                    inst.div = null;
                    $.calendars.picker.curInst = null;
                    var onClose = inst.get('onClose');
                    if (onClose) {
                        onClose.apply(target, [inst.selectedDates]);
                    }
                };
                inst.div.stop();
                if ($.effects && $.effects[showAnim]) {
                    inst.div.hide(showAnim, inst.get('showOptions'), showSpeed, postProcess);
                }
                else {
                    var hideAnim = (showAnim == 'slideDown' ? 'slideUp' :
					(showAnim == 'fadeIn' ? 'fadeOut' : 'hide'));
                    inst.div[hideAnim]((showAnim ? showSpeed : ''), postProcess);
                }
                if (!showAnim) {
                    postProcess();
                }
            }
        },


        _keyDown: function (event) {
            var target = event.target;
            var inst = $.data(target, $.calendars.picker.dataName);
            var handled = false;
            if (inst.div) {
                if (event.keyCode == 9) { // Tab - close
                    $.calendars.picker.hide(target);
                }
                else if (event.keyCode == 13) { // Enter - select
                    $.calendars.picker.selectDate(target,
					$('a.' + inst.get('renderer').highlightedClass, inst.div)[0]);
                    handled = true;
                }
                else { // Command keystrokes
                    var commands = inst.get('commands');
                    for (var name in commands) {
                        var command = commands[name];
                        if (command.keystroke.keyCode == event.keyCode &&
							!!command.keystroke.ctrlKey == !!(event.ctrlKey || event.metaKey) &&
							!!command.keystroke.altKey == event.altKey &&
							!!command.keystroke.shiftKey == event.shiftKey) {
                            $.calendars.picker.performAction(target, name);
                            handled = true;
                            break;
                        }
                    }
                }
            }
            else { // Show on 'current' keystroke
                var command = inst.get('commands').current;
                if (command.keystroke.keyCode == event.keyCode &&
					!!command.keystroke.ctrlKey == !!(event.ctrlKey || event.metaKey) &&
					!!command.keystroke.altKey == event.altKey &&
					!!command.keystroke.shiftKey == event.shiftKey) {
                    $.calendars.picker.show(target);
                    handled = true;
                }
            }
            inst.ctrlKey = ((event.keyCode < 48 && event.keyCode != 32) ||
			event.ctrlKey || event.metaKey);
            if (handled) {
                event.preventDefault();
                event.stopPropagation();
            }
            return !handled;
        },


        _keyPress: function (event) {
            var target = event.target;
            var inst = $.data(target, $.calendars.picker.dataName);
            if (inst && inst.get('constrainInput')) {
                var ch = String.fromCharCode(event.keyCode || event.charCode);
                var allowedChars = $.calendars.picker._allowedChars(inst);
                return (event.metaKey || inst.ctrlKey || ch < ' ' ||
				!allowedChars || allowedChars.indexOf(ch) > -1);
            }
            return true;
        },


        _allowedChars: function (inst) {
            var dateFormat = inst.get('dateFormat');
            var allowedChars = (inst.get('multiSelect') ? inst.get('multiSeparator') :
			(inst.get('rangeSelect') ? inst.get('rangeSeparator') : ''));
            var literal = false;
            var hasNum = false;
            for (var i = 0; i < dateFormat.length; i++) {
                var ch = dateFormat.charAt(i);
                if (literal) {
                    if (ch == "'" && dateFormat.charAt(i + 1) != "'") {
                        literal = false;
                    }
                    else {
                        allowedChars += ch;
                    }
                }
                else {
                    switch (ch) {
                        case 'd': case 'm': case 'o': case 'w':
                            allowedChars += (hasNum ? '' : '0123456789'); hasNum = true; break;
                        case 'y': case '@': case '!':
                            allowedChars += (hasNum ? '' : '0123456789') + '-'; hasNum = true; break;
                        case 'J':
                            allowedChars += (hasNum ? '' : '0123456789') + '-.'; hasNum = true; break;
                        case 'D': case 'M': case 'Y':
                            return null; // Accept anything
                        case "'":
                            if (dateFormat.charAt(i + 1) == "'") {
                                allowedChars += "'";
                            }
                            else {
                                literal = true;
                            }
                            break;
                        default:
                            allowedChars += ch;
                    }
                }
            }
            return allowedChars;
        },


        _keyUp: function (event) {
            var target = event.target;
            var inst = $.data(target, $.calendars.picker.dataName);
            if (inst && !inst.ctrlKey && inst.lastVal != inst.target.val()) {
                try {
                    var dates = $.calendars.picker._extractDates(inst, inst.target.val());
                    if (dates.length > 0) {
                        $.calendars.picker.setDate(target, dates, null, true);
                    }
                }
                catch (event) {
                    // Ignore
                }
            }
            return true;
        },


        _doMouseWheel: function (event, delta) {
            var target = ($.calendars.picker.curInst && $.calendars.picker.curInst.target[0]) ||
			$(event.target).closest('.' + $.calendars.picker.markerClass)[0];
            if ($.calendars.picker.isDisabled(target)) {
                return;
            }
            var inst = $.data(target, $.calendars.picker.dataName);
            if (inst.get('useMouseWheel')) {
                delta = ($.browser.opera ? -delta : delta);
                delta = (delta < 0 ? -1 : +1);
                $.calendars.picker.changeMonth(target,
				-inst.get(event.ctrlKey ? 'monthsToJump' : 'monthsToStep') * delta);
            }
            event.preventDefault();
        },


        clear: function (target) {
            var inst = $.data(target, this.dataName);
            if (inst) {
                inst.selectedDates = [];
                this.hide(target);
                if (inst.get('selectDefaultDate') && inst.get('defaultDate')) {
                    var calendar = inst.get('calendar');
                    this.setDate(target, (inst.get('defaultDate') || calendar.today()).newDate());
                }
                else {
                    this._updateInput(target);
                }
            }
        },


        getDate: function (target) {
            var inst = $.data(target, this.dataName);
            return (inst ? inst.selectedDates : []);
        },


        setDate: function (target, dates, endDate, keyUp, setOpt) {
            var inst = $.data(target, this.dataName);
            if (inst) {
                if (!$.isArray(dates)) {
                    dates = [dates];
                    if (endDate) {
                        dates.push(endDate);
                    }
                }
                var calendar = inst.get('calendar');
                var dateFormat = inst.get('dateFormat');
                var minDate = inst.get('minDate');
                var maxDate = inst.get('maxDate');
                var curDate = inst.selectedDates[0];
                inst.selectedDates = [];
                for (var i = 0; i < dates.length; i++) {
                    var date = calendar.determineDate(
					dates[i], null, curDate, dateFormat, inst.getConfig());
                    if (date) {
                        if ((!minDate || date.compareTo(minDate) != -1) &&
							(!maxDate || date.compareTo(maxDate) != +1)) {
                            var found = false;
                            for (var j = 0; j < inst.selectedDates.length; j++) {
                                if (inst.selectedDates[j].compareTo(date) == 0) {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found) {
                                inst.selectedDates.push(date);
                            }
                        }
                    }
                }
                var rangeSelect = inst.get('rangeSelect');
                inst.selectedDates.splice(inst.get('multiSelect') ||
				(rangeSelect ? 2 : 1), inst.selectedDates.length);
                if (rangeSelect) {
                    switch (inst.selectedDates.length) {
                        case 1: inst.selectedDates[1] = inst.selectedDates[0]; break;
                        case 2: inst.selectedDates[1] =
						(inst.selectedDates[0].compareTo(inst.selectedDates[1]) == +1 ?
						inst.selectedDates[0] : inst.selectedDates[1]); break;
                    }
                    inst.pickingRange = false;
                }
                inst.prevDate = (inst.drawDate ? inst.drawDate.newDate() : null);
                inst.drawDate = this._checkMinMax((inst.selectedDates[0] ||
				inst.get('defaultDate') || calendar.today()).newDate(), inst);
                if (!setOpt) {
                    this._update(target);
                    this._updateInput(target, keyUp);
                }
            }
        },


        isSelectable: function (target, date) {
            var inst = $.data(target, this.dataName);
            if (!inst) {
                return false;
            }
            date = inst.get('calendar').determineDate(date,
			inst.selectedDates[0] || inst.get('calendar').today(), null,
			inst.get('dateFormat'), inst.getConfig());
            return this._isSelectable(target, date, inst.get('onDate'),
			inst.get('minDate'), inst.get('maxDate'));
        },


        _isSelectable: function (target, date, onDate, minDate, maxDate) {
            var dateInfo = (typeof onDate == 'boolean' ? { selectable: onDate} :
			(!onDate ? {} : onDate.apply(target, [date, true])));
            return (dateInfo.selectable != false) &&
			(!minDate || date.toJD() >= minDate.toJD()) &&
			(!maxDate || date.toJD() <= maxDate.toJD());
        },


        performAction: function (target, action) {
            var inst = $.data(target, this.dataName);
            if (inst && !this.isDisabled(target)) {
                var commands = inst.get('commands');
                if (commands[action] && commands[action].enabled.apply(target, [inst])) {
                    commands[action].action.apply(target, [inst]);
                }
            }
        },


        showMonth: function (target, year, month, day) {
            var inst = $.data(target, this.dataName);
            if (inst && (day != null ||
				(inst.drawDate.year() != year || inst.drawDate.month() != month))) {
                inst.prevDate = inst.drawDate.newDate();
                var calendar = inst.get('calendar');
                var show = this._checkMinMax((year != null ?
				calendar.newDate(year, month, 1) : calendar.today()), inst);
                inst.drawDate.date(show.year(), show.month(),
				(day != null ? day : Math.min(inst.drawDate.day(),
				calendar.daysInMonth(show.year(), show.month()))));
                this._update(target);
            }
        },


        changeMonth: function (target, offset) {
            var inst = $.data(target, this.dataName);
            if (inst) {
                var date = inst.drawDate.newDate().add(offset, 'm');
                this.showMonth(target, date.year(), date.month());
            }
        },


        changeDay: function (target, offset) {
            var inst = $.data(target, this.dataName);
            if (inst) {
                var date = inst.drawDate.newDate().add(offset, 'd');
                this.showMonth(target, date.year(), date.month(), date.day());
            }
        },


        _checkMinMax: function (date, inst) {
            var minDate = inst.get('minDate');
            var maxDate = inst.get('maxDate');
            date = (minDate && date.compareTo(minDate) == -1 ? minDate.newDate() : date);
            date = (maxDate && date.compareTo(maxDate) == +1 ? maxDate.newDate() : date);
            return date;
        },


        retrieveDate: function (target, elem) {
            var inst = $.data(target, this.dataName);
            return (!inst ? null : inst.get('calendar').fromJD(
			parseFloat(elem.className.replace(/^.*jd(\d+\.5).*$/, '$1'))));
        },


        selectDate: function (target, elem) {
            var inst = $.data(target, this.dataName);
            if (inst && !this.isDisabled(target)) {
                var date = this.retrieveDate(target, elem);
                var multiSelect = inst.get('multiSelect');
                var rangeSelect = inst.get('rangeSelect');
                if (multiSelect) {
                    var found = false;
                    for (var i = 0; i < inst.selectedDates.length; i++) {
                        if (date.compareTo(inst.selectedDates[i]) == 0) {
                            inst.selectedDates.splice(i, 1);
                            found = true;
                            break;
                        }
                    }
                    if (!found && inst.selectedDates.length < multiSelect) {
                        inst.selectedDates.push(date);
                    }
                }
                else if (rangeSelect) {
                    if (inst.pickingRange) {
                        inst.selectedDates[1] = date;
                    }
                    else {
                        inst.selectedDates = [date, date];
                    }
                    inst.pickingRange = !inst.pickingRange;
                }
                else {
                    inst.selectedDates = [date];
                }
                this._updateInput(target);
                if (inst.inline || inst.pickingRange || inst.selectedDates.length <
					(multiSelect || (rangeSelect ? 2 : 1))) {
                    this._update(target);
                }
                else {
                    this.hide(target);
                }
            }
        },


        _generateContent: function (target, inst) {
            var calendar = inst.get('calendar');
            var renderer = inst.get('renderer');
            var monthsToShow = inst.get('monthsToShow');
            monthsToShow = ($.isArray(monthsToShow) ? monthsToShow : [1, monthsToShow]);
            inst.drawDate = this._checkMinMax(
			inst.drawDate || inst.get('defaultDate') || calendar.today(), inst);
            var drawDate = inst.drawDate.newDate().add(-inst.get('monthsOffset'), 'm');
            // Generate months
            var monthRows = '';
            for (var row = 0; row < monthsToShow[0]; row++) {
                var months = '';
                for (var col = 0; col < monthsToShow[1]; col++) {
                    months += this._generateMonth(target, inst, drawDate.year(),
					drawDate.month(), calendar, renderer, (row == 0 && col == 0));
                    drawDate.add(1, 'm');
                }
                monthRows += this._prepare(renderer.monthRow, inst).replace(/\{months\}/, months);
            }
            var picker = this._prepare(renderer.picker, inst).replace(/\{months\}/, monthRows).
			replace(/\{weekHeader\}/g, this._generateDayHeaders(inst, calendar, renderer)) +
			($.browser.msie && parseInt($.browser.version, 10) < 7 && !inst.inline ?
			'<iframe src="javascript:void(0);" class="' + this._coverClass + '"></iframe>' : '');
            // Add commands
            var commands = inst.get('commands');
            var asDateFormat = inst.get('commandsAsDateFormat');
            var addCommand = function (type, open, close, name, classes) {
                if (picker.indexOf('{' + type + ':' + name + '}') == -1) {
                    return;
                }
                var command = commands[name];
                var date = (asDateFormat ? command.date.apply(target, [inst]) : null);
                picker = picker.replace(new RegExp('\\{' + type + ':' + name + '\\}', 'g'),
				'<' + open +
				(command.status ? ' title="' + inst.get(command.status) + '"' : '') +
				' class="' + renderer.commandClass + ' ' +
				renderer.commandClass + '-' + name + ' ' + classes +
				(command.enabled(inst) ? '' : ' ' + renderer.disabledClass) + '">' +
				(date ? date.formatDate(inst.get(command.text)) : inst.get(command.text)) +
				'</' + close + '>');
            };
            for (var name in commands) {
                addCommand('button', 'button type="button"', 'button', name,
				renderer.commandButtonClass);
                addCommand('link', 'a href="javascript:void(0)"', 'a', name,
				renderer.commandLinkClass);
            }
            picker = $(picker);
            if (monthsToShow[1] > 1) {
                var count = 0;
                $(renderer.monthSelector, picker).each(function () {
                    var nth = ++count % monthsToShow[1];
                    $(this).addClass(nth == 1 ? 'first' : (nth == 0 ? 'last' : ''));
                });
            }
            // Add calendar behaviour
            var self = this;
            picker.find(renderer.daySelector + ' a').hover(
				function () { $(this).addClass(renderer.highlightedClass); },
				function () {
				    (inst.inline ? $(this).parents('.' + self.markerClass) : inst.div).
						find(renderer.daySelector + ' a').
						removeClass(renderer.highlightedClass);
				}).
			click(function () {
			    self.selectDate(target, this);
			}).end().
			find('select.' + this._monthYearClass + ':not(.' + this._anyYearClass + ')').change(function () {
			    var monthYear = $(this).val().split('/');
			    self.showMonth(target, parseInt(monthYear[1], 10), parseInt(monthYear[0], 10));
			}).end().
			find('select.' + this._anyYearClass).click(function () {
			    $(this).next('input').css({ left: this.offsetLeft, top: this.offsetTop,
			        width: this.offsetWidth, height: this.offsetHeight
			    }).show().focus();
			}).end().
			find('input.' + self._monthYearClass).change(function () {
			    try {
			        var year = parseInt($(this).val(), 10);
			        year = (isNaN(year) ? inst.drawDate.year() : year);
			        self.showMonth(target, year, inst.drawDate.month(), inst.drawDate.day());
			    }
			    catch (e) {
			        alert(e);
			    }
			}).keydown(function (event) {
			    if (event.keyCode == 27) { // Escape
			        $(event.target).hide();
			        inst.target.focus();
			    }
			});
            // Add command behaviour
            picker.find('.' + renderer.commandClass).click(function () {
                if (!$(this).hasClass(renderer.disabledClass)) {
                    var action = this.className.replace(
						new RegExp('^.*' + renderer.commandClass + '-([^ ]+).*$'), '$1');
                    $.calendars.picker.performAction(target, action);
                }
            });
            // Add classes
            if (inst.get('isRTL')) {
                picker.addClass(renderer.rtlClass);
            }
            if (monthsToShow[0] * monthsToShow[1] > 1) {
                picker.addClass(renderer.multiClass);
            }
            var pickerClass = inst.get('pickerClass');
            if (pickerClass) {
                picker.addClass(pickerClass);
            }
            // Resize
            $('body').append(picker);
            var width = 0;
            picker.find(renderer.monthSelector).each(function () {
                width += $(this).outerWidth();
            });
            picker.width(width / monthsToShow[0]);
            // Pre-show customisation
            var onShow = inst.get('onShow');
            if (onShow) {
                onShow.apply(target, [picker, calendar, inst]);
            }
            return picker;
        },


        _generateMonth: function (target, inst, year, month, calendar, renderer, first) {
            var daysInMonth = calendar.daysInMonth(year, month);
            var monthsToShow = inst.get('monthsToShow');
            monthsToShow = ($.isArray(monthsToShow) ? monthsToShow : [1, monthsToShow]);
            var fixedWeeks = inst.get('fixedWeeks') || (monthsToShow[0] * monthsToShow[1] > 1);
            var firstDay = inst.get('firstDay');
            firstDay = (firstDay == null ? calendar.local.firstDay : firstDay);
            var leadDays = (calendar.dayOfWeek(year, month, calendar.minDay) -
			firstDay + calendar.daysInWeek()) % calendar.daysInWeek();
            var numWeeks = (fixedWeeks ? 6 : Math.ceil((leadDays + daysInMonth) / calendar.daysInWeek()));
            var showOtherMonths = inst.get('showOtherMonths');
            var selectOtherMonths = inst.get('selectOtherMonths') && showOtherMonths;
            var dayStatus = inst.get('dayStatus');
            var minDate = (inst.pickingRange ? inst.selectedDates[0] : inst.get('minDate'));
            var maxDate = inst.get('maxDate');
            var rangeSelect = inst.get('rangeSelect');
            var onDate = inst.get('onDate');
            var showWeeks = renderer.week.indexOf('{weekOfYear}') > -1;
            var calculateWeek = inst.get('calculateWeek');
            var today = calendar.today();
            var drawDate = calendar.newDate(year, month, calendar.minDay);
            drawDate.add(-leadDays - (fixedWeeks &&
			(drawDate.dayOfWeek() == firstDay || drawDate.daysInMonth() < calendar.daysInWeek()) ?
			calendar.daysInWeek() : 0), 'd');
            var jd = drawDate.toJD();
            // Generate weeks
            var weeks = '';
            for (var week = 0; week < numWeeks; week++) {
                var weekOfYear = (!showWeeks ? '' : '<span class="jd' + jd + '">' +
				(calculateWeek ? calculateWeek(drawDate) : drawDate.weekOfYear()) + '</span>');
                var days = '';
                for (var day = 0; day < calendar.daysInWeek(); day++) {
                    var selected = false;
                    if (rangeSelect && inst.selectedDates.length > 0) {
                        selected = (drawDate.compareTo(inst.selectedDates[0]) != -1 &&
						drawDate.compareTo(inst.selectedDates[1]) != +1)
                    }
                    else {
                        for (var i = 0; i < inst.selectedDates.length; i++) {
                            if (inst.selectedDates[i].compareTo(drawDate) == 0) {
                                selected = true;
                                break;
                            }
                        }
                    }
                    var dateInfo = (!onDate ? {} :
					onDate.apply(target, [drawDate, drawDate.month() == month]));
                    var selectable = (selectOtherMonths || drawDate.month() == month) &&
					this._isSelectable(target, drawDate, dateInfo.selectable, minDate, maxDate);
                    days += this._prepare(renderer.day, inst).replace(/\{day\}/g,
					(selectable ? '<a href="javascript:void(0)"' : '<span') +
					' class="jd' + jd + ' ' + (dateInfo.dateClass || '') +
					(selected && (selectOtherMonths || drawDate.month() == month) ?
					' ' + renderer.selectedClass : '') +
					(selectable ? ' ' + renderer.defaultClass : '') +
					(drawDate.weekDay() ? '' : ' ' + renderer.weekendClass) +
					(drawDate.month() == month ? '' : ' ' + renderer.otherMonthClass) +
					(drawDate.compareTo(today) == 0 && drawDate.month() == month ?
					' ' + renderer.todayClass : '') +
					(drawDate.compareTo(inst.drawDate) == 0 && drawDate.month() == month ?
					' ' + renderer.highlightedClass : '') + '"' +
					(dateInfo.title || (dayStatus && selectable) ? ' title="' +
					(dateInfo.title || drawDate.formatDate(dayStatus)) + '"' : '') + '>' +
					(showOtherMonths || drawDate.month() == month ?
					dateInfo.content || drawDate.day() : '&nbsp;') +
					(selectable ? '</a>' : '</span>'));
                    drawDate.add(1, 'd');
                    jd++;
                }
                weeks += this._prepare(renderer.week, inst).replace(/\{days\}/g, days).
				replace(/\{weekOfYear\}/g, weekOfYear);
            }
            var monthHeader = this._prepare(renderer.month, inst).match(/\{monthHeader(:[^\}]+)?\}/);
            monthHeader = (monthHeader[0].length <= 13 ? 'MM yyyy' :
			monthHeader[0].substring(13, monthHeader[0].length - 1));
            monthHeader = (first ? this._generateMonthSelection(
			inst, year, month, minDate, maxDate, monthHeader, calendar, renderer) :
			calendar.formatDate(monthHeader, calendar.newDate(year, month, calendar.minDay)));
            var weekHeader = this._prepare(renderer.weekHeader, inst).
			replace(/\{days\}/g, this._generateDayHeaders(inst, calendar, renderer));
            return this._prepare(renderer.month, inst).replace(/\{monthHeader(:[^\}]+)?\}/g, monthHeader).
			replace(/\{weekHeader\}/g, weekHeader).replace(/\{weeks\}/g, weeks);
        },


        _generateDayHeaders: function (inst, calendar, renderer) {
            var firstDay = inst.get('firstDay');
            firstDay = (firstDay == null ? calendar.local.firstDay : firstDay);
            var header = '';
            for (var day = 0; day < calendar.daysInWeek(); day++) {
                var dow = (day + firstDay) % calendar.daysInWeek();
                header += this._prepare(renderer.dayHeader, inst).replace(/\{day\}/g,
				'<span class="' + this._curDoWClass + dow + '" title="' +
				calendar.local.dayNames[dow] + '">' +
				calendar.local.dayNamesMin[dow] + '</span>');
            }
            return header;
        },


        _generateMonthSelection: function (inst, year, month, minDate, maxDate, monthHeader, calendar) {
            if (!inst.get('changeMonth')) {
                return calendar.formatDate(monthHeader, calendar.newDate(year, month, 1));
            }
            // Months
            var monthNames = calendar.local[
			'monthNames' + (monthHeader.match(/mm/i) ? '' : 'Short')];
            var html = monthHeader.replace(/m+/i, '\\x2E').replace(/y+/i, '\\x2F');
            var selector = '<select class="' + this._monthYearClass +
			'" title="' + inst.get('monthStatus') + '">';
            var maxMonth = calendar.monthsInYear(year) + calendar.minMonth;
            for (var m = calendar.minMonth; m < maxMonth; m++) {
                if ((!minDate || calendar.newDate(year, m,
					calendar.daysInMonth(year, m) - 1 + calendar.minDay).
					compareTo(minDate) != -1) &&
					(!maxDate || calendar.newDate(year, m, calendar.minDay).
					compareTo(maxDate) != +1)) {
                    selector += '<option value="' + m + '/' + year + '"' +
					(month == m ? ' selected="selected"' : '') + '>' +
					monthNames[m - calendar.minMonth] + '</option>';
                }
            }
            selector += '</select>';
            html = html.replace(/\\x2E/, selector);
            // Years
            var yearRange = inst.get('yearRange');
            if (yearRange == 'any') {
                selector = '<select class="' + this._monthYearClass + ' ' + this._anyYearClass +
				'" title="' + inst.get('yearStatus') + '">' +
				'<option>' + year + '</option></select>' +
				'<input class="' + this._monthYearClass + ' ' + this._curMonthClass +
				month + '" value="' + year + '">';
            }
            else {
                yearRange = yearRange.split(':');
                var todayYear = calendar.today().year();
                var start = (yearRange[0].match('c[+-].*') ? year + parseInt(yearRange[0].substring(1), 10) :
				((yearRange[0].match('[+-].*') ? todayYear : 0) + parseInt(yearRange[0], 10)));
                var end = (yearRange[1].match('c[+-].*') ? year + parseInt(yearRange[1].substring(1), 10) :
				((yearRange[1].match('[+-].*') ? todayYear : 0) + parseInt(yearRange[1], 10)));
                selector = '<select class="' + this._monthYearClass +
				'" title="' + inst.get('yearStatus') + '">';
                start = calendar.newDate(start + 1, calendar.firstMonth, calendar.minDay).add(-1, 'd');
                end = calendar.newDate(end, calendar.firstMonth, calendar.minDay);
                var addYear = function (y) {
                    if (y != 0 || calendar.hasYearZero) {
                        selector += '<option value="' +
						Math.min(month, calendar.monthsInYear(y) - 1 + calendar.minMonth) +
						'/' + y + '"' + (year == y ? ' selected="selected"' : '') + '>' +
						y + '</option>';
                    }
                };
                if (start.toJD() < end.toJD()) {
                    start = (minDate && minDate.compareTo(start) == +1 ? minDate : start).year();
                    end = (maxDate && maxDate.compareTo(end) == -1 ? maxDate : end).year();
                    for (var y = start; y <= end; y++) {
                        addYear(y);
                    }
                }
                else {
                    start = (maxDate && maxDate.compareTo(start) == -1 ? maxDate : start).year();
                    end = (minDate && minDate.compareTo(end) == +1 ? minDate : end).year();
                    for (var y = start; y >= end; y--) {
                        addYear(y);
                    }
                }
                selector += '</select>';
            }
            html = html.replace(/\\x2F/, selector);
            return html;
        },


        _prepare: function (text, inst) {
            var replaceSection = function (type, retain) {
                while (true) {
                    var start = text.indexOf('{' + type + ':start}');
                    if (start == -1) {
                        return;
                    }
                    var end = text.substring(start).indexOf('{' + type + ':end}');
                    if (end > -1) {
                        text = text.substring(0, start) +
						(retain ? text.substr(start + type.length + 8, end - type.length - 8) : '') +
						text.substring(start + end + type.length + 6);
                    }
                }
            };
            replaceSection('inline', inst.inline);
            replaceSection('popup', !inst.inline);
            var pattern = /\{l10n:([^\}]+)\}/;
            var matches = null;
            while (matches = pattern.exec(text)) {
                text = text.replace(matches[0], inst.get(matches[1]));
            }
            return text;
        }
    });


    function extendRemove(target, props) {
        $.extend(target, props);
        for (var name in props)
            if (props[name] == null || props[name] == undefined)
                target[name] = props[name];
        return target;
    };


    $.fn.calendarsPicker = function (options) {
        var otherArgs = Array.prototype.slice.call(arguments, 1);
        if ($.inArray(options, ['getDate', 'isDisabled', 'isSelectable', 'options', 'retrieveDate']) > -1) {
            return $.calendars.picker[options].apply($.calendars.picker, [this[0]].concat(otherArgs));
        }
        return this.each(function () {
            if (typeof options == 'string') {
                $.calendars.picker[options].apply($.calendars.picker, [this].concat(otherArgs));
            }
            else {
                $.calendars.picker._attachPicker(this, options || {});
            }
        });
    };


    $.calendars.picker = new CalendarsPicker(); // singleton instance

    $(function () {
        $(document).mousedown($.calendars.picker._checkExternalClick).
		resize(function () { $.calendars.picker.hide($.calendars.picker.curInst); });
    });

})(jQuery);




(function ($) { // Hide scope, no $ conflict


    function PersianCalendar(language) {
        this.local = this.regional[language || ''] || this.regional[''];
    }

    PersianCalendar.prototype = new $.calendars.baseCalendar;

    $.extend(PersianCalendar.prototype, {
        name: 'Persian', // The calendar name
        jdEpoch: 1948320.5, // Julian date of start of Persian epoch: 19 March 622 CE
        daysPerMonth: [31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29], // Days per month in a common year
        hasYearZero: false, // True if has a year zero, false if not
        minMonth: 1, // The minimum month number
        firstMonth: 1, // The first month in the year
        minDay: 1, // The minimum day number

        regional: { // Localisations
            '': {
                name: 'Persian', // The calendar name
                epochs: ['BP', 'AP'],
                monthNames: ['Farvardin', 'Ordibehesht', 'Khordad', 'Tir', 'Mordad', 'Shahrivar',
			'Mehr', 'Aban', 'Azar', 'Day', 'Bahman', 'Esfand'],
                monthNamesShort: ['Far', 'Ord', 'Kho', 'Tir', 'Mor', 'Sha', 'Meh', 'Aba', 'Aza', 'Day', 'Bah', 'Esf'],
                dayNames: ['Yekshambe', 'Doshambe', 'Seshambe', 'Chæharshambe', 'Panjshambe', 'Jom\'e', 'Shambe'],
                dayNamesShort: ['Yek', 'Do', 'Se', 'Chæ', 'Panj', 'Jom', 'Sha'],
                dayNamesMin: ['Ye', 'Do', 'Se', 'Ch', 'Pa', 'Jo', 'Sh'],
                dateFormat: 'yyyy/mm/dd', // See format options on BaseCalendar.formatDate
                firstDay: 6, // The first day of the week, Sun = 0, Mon = 1, ...
                isRTL: false // True if right-to-left language, false if left-to-right
            }
        },


        leapYear: function (year) {
            var date = this._validate(year, this.minMonth, this.minDay, $.calendars.local.invalidYear);
            return (((((date.year() - (date.year() > 0 ? 474 : 473)) % 2820) +
			474 + 38) * 682) % 2816) < 682;
        },


        weekOfYear: function (year, month, day) {

            var checkDate = this.newDate(year, month, day);
            checkDate.add(-((checkDate.dayOfWeek() + 1) % 7), 'd');
            return Math.floor((checkDate.dayOfYear() - 1) / 7) + 1;
        },


        daysInMonth: function (year, month) {
            var date = this._validate(year, month, this.minDay, $.calendars.local.invalidMonth);
            return this.daysPerMonth[date.month() - 1] +
			(date.month() == 12 && this.leapYear(date.year()) ? 1 : 0);
        },


        weekDay: function (year, month, day) {
            return this.dayOfWeek(year, month, day) != 5;
        },


        toJD: function (year, month, day) {
            var date = this._validate(year, month, day, $.calendars.local.invalidDate);
            year = date.year();
            month = date.month();
            day = date.day();
            var epBase = year - (year >= 0 ? 474 : 473);
            var epYear = 474 + mod(epBase, 2820);
            return day + (month <= 7 ? (month - 1) * 31 : (month - 1) * 30 + 6) +
			Math.floor((epYear * 682 - 110) / 2816) + (epYear - 1) * 365 +
			Math.floor(epBase / 2820) * 1029983 + this.jdEpoch - 1;
        },


        fromJD: function (jd) {
            jd = Math.floor(jd) + 0.5;
            var depoch = jd - this.toJD(475, 1, 1);
            var cycle = Math.floor(depoch / 1029983);
            var cyear = mod(depoch, 1029983);
            var ycycle = 2820;
            if (cyear != 1029982) {
                var aux1 = Math.floor(cyear / 366);
                var aux2 = mod(cyear, 366);
                ycycle = Math.floor(((2134 * aux1) + (2816 * aux2) + 2815) / 1028522) + aux1 + 1;
            }
            var year = ycycle + (2820 * cycle) + 474;
            year = (year <= 0 ? year - 1 : year);
            var yday = jd - this.toJD(year, 1, 1) + 1;
            var month = (yday <= 186 ? Math.ceil(yday / 31) : Math.ceil((yday - 6) / 30));
            var day = jd - this.toJD(year, month, 1) + 1;
            return this.newDate(year, month, day);
        }
    });


    function mod(a, b) {
        return a - (b * Math.floor(a / b));
    }


    $.calendars.calendars.persian = PersianCalendar;
    $.calendars.calendars.jalali = PersianCalendar;

})(jQuery);


(function ($) {
    $.calendars.calendars.persian.prototype.regional['fa'] = {
        name: 'Persian',
        epochs: ['BP', 'AP'],
        monthNames: ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور',
		'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'],
        monthNamesShort: ['فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور',
		'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند'],
        dayNames: ['يک شنبه', 'دوشنبه', 'سه شنبه', 'چهار شنبه', 'پنج شنبه', 'جمعه', 'شنبه'],
        dayNamesShort: ['يک', 'دو', 'سه', 'چهار', 'پنج', 'جمعه', 'شنبه'],
        dayNamesMin: ['ي', 'د', 'س', 'چ', 'پ', 'ج', 'ش'],
        dateFormat: 'yyyy/mm/dd',
        firstDay: 6,
        isRTL: true
    };
})(jQuery);