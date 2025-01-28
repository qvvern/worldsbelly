/* eslint-disable */
import moment from "moment";
import i18n from "@/lang";

const momentPlugin = {
    install(Vue) {
        Vue.prototype.$moment = {};

        Vue.prototype.$moment.toDate = function(date) {
            return moment(date).format("DD-MM-YYYY");
        };

        Vue.prototype.$moment.toDateTime = function(date) {
            if (!date) {
                return "";
            }

            return moment(date).format("DD-MM-YYYY HH:mm");
        };

        Vue.prototype.$moment.toDateTimeReversed = function(date) {
            if (!date) {
                return "";
            }

            return moment(date).format("HH:mm DD-MM-YYYY");
        };

        Vue.prototype.$moment.toDateTimeFullSeconds = function(date) {
            if (!date) {
                return "";
            }

            return moment(date).format("DD-MM-YYYY HH:mm:ss");
        };

        Vue.prototype.$moment.toDateTimeSeconds = function(date) {
            if (!date) {
                return "";
            }

            return moment(date).format("DD-MM HH:mm:ss");
        };

        Vue.prototype.$moment.getTimeSinceText = function(date) {
            if (!date) {
                return `${i18n.t('not')} ${i18n.t('published')}`;
            }


            const momentDateTime = moment(date);

            const diffSeconds = moment().diff(momentDateTime, "seconds");

            if (diffSeconds === 1) {
                return `${diffSeconds} ${i18n.t('second')} ${i18n.t('ago')}`;
            }
            if (diffSeconds <= 60) {
                return `${diffSeconds} ${i18n.t('seconds')} ${i18n.t('ago')}`;
            }

            const diffMinutes = moment().diff(momentDateTime, "minutes");

            if (diffMinutes === 1) {
                return `${diffMinutes} ${i18n.t('minute')} ${i18n.t('ago')}`;
            }
            if (diffMinutes <= 60) {
                return `${diffMinutes} ${i18n.t('minutes')} ${i18n.t('ago')}`;
            }

            const diffHours = moment().diff(momentDateTime, "hours");

            if (diffHours === 1) {
                return `${diffHours} ${i18n.t('hour')} ${i18n.t('ago')}`;
            }
            if (diffHours <= 24) {
                return `${diffHours} ${i18n.t('hours')} ${i18n.t('ago')}`;
            }

            const diffDays = moment().diff(momentDateTime, "days");

            if (diffDays === 1) {
                return `${diffDays} ${i18n.t('day')} ${i18n.t('ago')}`;
            }
            if (diffDays <= 7) {
                return `${diffDays} ${i18n.t('days')} ${i18n.t('ago')}`;
            }

            const diffWeeks = moment().diff(momentDateTime, "weeks");

            if (diffWeeks === 1) {
                return `${diffWeeks} ${i18n.t('week')} ${i18n.t('ago')}`;
            }
            if (diffWeeks <= 4) {
                return `${diffWeeks} ${i18n.t('weeks')} ${i18n.t('ago')}`;
            }

            const diffMonths = moment().diff(momentDateTime, "months");
            if (diffMonths === 1) {
                return `${diffMonths} ${i18n.t('month')} ${i18n.t('ago')}`;
            }
            if (diffMonths <= 12) {
                return `${diffMonths} ${i18n.t('months')} ${i18n.t('ago')}`;
            }

            const diffYears = moment().diff(momentDateTime, "years");

            if (diffYears === 1) {
                return `${diffYears} ${i18n.t('year')} ${i18n.t('ago')}`;
            }
            if (diffYears < 24) {
                return `${diffYears} ${i18n.t('years')} ${i18n.t('ago')}`;
            }

            return this.toDateTime(momentDateTime);
        };

        Vue.prototype.$moment.isToday = function(date) {
            return !moment(date).isBefore(moment(), "day");
        };

        Vue.prototype.$moment.daysSince = function(date) {
            var given = moment(date, "YYYY-MM-DD");
            var current = moment().startOf("day");
            return moment.duration(given.diff(current)).asDays();
        };

        Vue.prototype.$moment.toTime = function(date) {
            return moment(date).format("HH:mm");
        };

        Vue.prototype.$moment.toDayName = function(date) {
            return moment(date).format("ddd");
        };

        Vue.prototype.$moment.newDate = function(daysToSubtract) {
            return moment()
                .subtract(daysToSubtract, "days")
                .format("DD-MM");
        };

        Vue.prototype.$moment.getNow = function() {
            return moment().format("DD-MM-YYYY HH:mm:ss");
        };

        Vue.prototype.$moment.timeBetweenDates = function(then, now) {
            if (!now || !then) {
                return;
            }
            var then = moment(then);
            var now = moment(now);
            var ms = moment(now, "DD/MM/YYYY HH:mm:ss").diff(moment(then, "DD/MM/YYYY HH:mm:ss"));
            var d = moment.duration(ms);
            var timeString = "";
            if (d.days()) {
                timeString += d.days() + ` ${i18n.t('days')} `;
            }
            if (d.hours()) {
                timeString += d.hours() + ` ${i18n.t('hours')} `;
            }
            if (d.minutes()) {
                timeString += d.minutes() + ` ${i18n.t('minutes')} `;
            }
            if (d.seconds()) {
                timeString += d.seconds() + ` ${i18n.t('seconds')} `;
            }
            if (!timeString) {
                timeString = `0 ${i18n.t('seconds')}`;
            }
            return timeString;
        };
    }
};

export default momentPlugin;
