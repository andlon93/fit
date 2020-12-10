export function secondsToDuration(seconds : number, includeHour : boolean = true) {
    let hours = Math.floor(seconds / 60 / 60);
    seconds = seconds - hours * 60 * 60;
    let minutes = Math.floor((seconds - hours * 60 * 60) / 60);
    seconds = seconds - minutes * 60;
    return (hours == 0 ? '' : hours + ':') + minutes + ':' + seconds;
}

export function metersToString(meters : number, numberOfDecimals : number = 0) {
    let factor = Math.pow(10, numberOfDecimals);
    return Math.round((meters / 1000.0 + Number.EPSILON) * factor) / factor + ' km'
}

export function dateToString(date : Date) {
    return date.getDay() + 
        ', ' + date.getDate() + '. ' + convertMonthToString(date.getMonth()) + ' ' + date.getFullYear() + 
        ' kl. ' + date.getHours() + ':' + date.getMinutes();
}

export function dateToStringMinimal(date : Date) {
    return date.getDate() + '. ' + convertMonthToString(date.getMonth()) + (date.getFullYear() === new Date().getFullYear() ? '' : ' ' + date.getFullYear()) + 
    ' kl. ' + date.getHours() + ':' + date.getMinutes();
  }

export function convertMonthToString (month : number) {
    switch(month) { 
        case 0: { 
            return 'januar'; 
        }
        case 1: {
            return 'februar';
        }
        case 2: {
            return 'mars';
        }
        case 3: {
            return 'april';
        }
        case 4: {
            return 'mai';
        }
        case 5: {
            return 'juni';
        }
        case 6: {
            return 'juli';
        }
        case 7: {
            return 'august';
        }
        case 8: {
            return 'september';
        }
        case 9: {
            return 'oktober';
        }
        case 10: {
            return 'november';
        }
        case 11: {
            return 'desember';
        }
        default: {
            return '';
        } 
    }
}