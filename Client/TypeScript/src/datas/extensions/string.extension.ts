interface String {
    format(...args: any[]): string;
}

String.prototype.format = function () {
    var args = arguments;
    return (this as String).replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
            ? args[number]
            : match
            ;
    });
}
