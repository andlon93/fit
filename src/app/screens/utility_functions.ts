import { ChallengeType } from '../types';

export function secondsToDuration(seconds : number, includeHour : boolean = true) {
    let hours = Math.floor(seconds / 60 / 60);
    seconds = seconds - hours * 60 * 60;
    let minutes = Math.floor(seconds / 60);
    seconds = seconds - minutes * 60;
    return (!includeHour && hours == 0 ? '' : timeWithLeadingZero(hours) + ':') + timeWithLeadingZero(minutes) + ':' + timeWithLeadingZero(seconds);
}

function minutesToDuration(minutes : number) {
    let hours = Math.floor(minutes / 60);
    minutes = minutes - hours * 60;
    return hours + 't:' + minutes + 'm';
}

function timeWithLeadingZero(hourMinuteSecond : number) {
    if (hourMinuteSecond < 10) {
        return '0' + hourMinuteSecond;
    } else {
        return hourMinuteSecond;
    }
}

export function secondsToWaterNeed(seconds : number, includeHour : boolean = true) {
    return round(seconds / 60.0 / 20.0 * 0.15, 2) + ' L';
}

export function metersToString(meters : number, numberOfDecimals : number = 0) {
    return round(meters / 1000.0, numberOfDecimals) + ' km'
}

function round(number : number, numberOfDecimals : number = 0) {
    let factor = Math.pow(10, numberOfDecimals);
    return Math.round((number + Number.EPSILON) * factor) / factor;
}

export function dateToString(date : Date, mode : 'date' | 'time' | 'datetime' = 'datetime') {
    let result = '';
    if (mode === 'date' || mode === 'datetime') {
        result += convertDayToString(date.getDay()) + 
        ', ' + date.getDate() + '. ' + convertMonthToString(date.getMonth()) + ' ' + date.getFullYear();
    }
    if (mode === 'time' || mode === 'datetime') {
        if (result !== '') {
            result += ' ';
        }
        result += 'kl. ' + date.getHours() + ':' + timeWithLeadingZero(date.getMinutes());
    }
    return result;
}

export function dateToStringMinimal(date : Date) {
    return date.getDate() + '. ' + convertMonthToString(date.getMonth()) + (date.getFullYear() === new Date().getFullYear() ? '' : ' ' + date.getFullYear()) + 
    ' kl. ' + timeWithLeadingZero(date.getHours()) + ':' + timeWithLeadingZero(date.getMinutes());
}

export function daysLeftUntil(date : Date) {
    let today = new Date();
    return (Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()) -
    Date.UTC(today.getFullYear(), today.getMonth(), today.getDate())) / 86400000 + 1;
}

export function daysLeftUntilAsString(date : Date) {
    let daysLeft = daysLeftUntil(date);
    return daysLeft === 1 ? daysLeft + ' dag igjen' : daysLeft + ' dager igjen';
}

export function challengeScoreToString(type : string, score : number) {
    switch(type) { 
        case 'MOST_ACTIVE_MINUTES': { 
           return minutesToDuration(score);
        } 
        case 'MOST_WORKOUTS': { 
           return score === 1 ? score + ' treningsøkt' : score + ' treningsøkter';
        } 
        default: { 
           return '-';
        } 
     } 
     
}

const months = [
    'januar',
    'februar',
    'mars',
    'april',
    'mai',
    'juni',
    'juli',
    'august',
    'september',
    'oktober',
    'november',
    'desember'
]

const days = [
    'søndag',
    'mandag',
    'tirsdag',
    'onsdag',
    'torsdag',
    'fredag',
    'lørdag'
]

export function convertMonthToString (month : number) {
    return months[month];
}

export function convertDayToString (day : number) {
    return days[day];
}