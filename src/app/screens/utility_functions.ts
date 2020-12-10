export function secondsToDuration(seconds : number, includeHour : boolean = true) {
    let hours = Math.floor(seconds / 60 / 60);
    seconds = seconds - hours * 60 * 60;
    let minutes = Math.floor(seconds / 60);
    seconds = seconds - minutes * 60;
    return (hours == 0 ? '' : hours + ':') + minutes + ':' + seconds;
}

export function metersToString(meters : number, numberOfDecimals : number = 0) {
    let factor = Math.pow(10, numberOfDecimals);
    return Math.round((meters / 1000.0 + Number.EPSILON) * factor) / factor + ' km'
}

export function dateToString(date : Date) {
    return convertDayToString(date.getDay()) + 
        ', ' + date.getDate() + '. ' + convertMonthToString(date.getMonth()) + ' ' + date.getFullYear() + 
        ' kl. ' + date.getHours() + ':' + date.getMinutes();
}

export function dateToStringMinimal(date : Date) {
    return date.getDate() + '. ' + convertMonthToString(date.getMonth()) + (date.getFullYear() === new Date().getFullYear() ? '' : ' ' + date.getFullYear()) + 
    ' kl. ' + date.getHours() + ':' + date.getMinutes();
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