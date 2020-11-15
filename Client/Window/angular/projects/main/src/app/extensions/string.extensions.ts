export {}

declare global {
    interface String {
        escapeJson(): string;
    }
}

String.prototype.escapeJson = function() {
    const str = this as String;
    return str.replace(/\\"/g, '"')
        .replace(/\n/g, "\\n")
        .replace(/\r/g, "\\r")
        .replace(/\t/g, "\\t")
        .replace(/\f/g, "\\f");
}