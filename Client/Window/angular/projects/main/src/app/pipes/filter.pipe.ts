import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'filter'
})
export class FilterPipe implements PipeTransform {

    transform(arr: any[], property: string, value: any): any {
        console.log(arr);
        console.log(arr[property]);
        return arr.filter(a => a[property] == value);
    }
}
