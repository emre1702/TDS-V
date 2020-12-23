import { FormControl } from '@angular/forms';
import { notOnlyNumbers, onlyAscii } from '../../shared/regex';

export function validateName(c: FormControl) {
    if (!c.value?.length) {
        return null;
    }
    if (!notOnlyNumbers.test(c.value)) {
        return { notOnlyNumbers: true };
    }
    if (!onlyAscii.test(c.value)) {
        return { notAllowedCharacter: getNotAsciiChars(c.value) };
    }
    return null;
}

function getNotAsciiChars(str: string) {
    let ret = '';
    for (const char of str) {
        if (!onlyAscii.test(char)) {
            if (!ret.length) ret += char;
            else ret += ', ' + char;
        }
    }
    return ret;
}
