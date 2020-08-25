interface Math {
    randomBetween(minIncluded: number, maxExcluded: number): number;
    randomBetweenIncluded(minIncluded: number, maxIncluded: number): number;
    randomIntBetween(minIncluded: number, maxExcluded: number): number;
    randomIntBetweenIncluded(minIncluded: number, maxIncluded: number): number;
}

Math.randomBetween = function (minIncluded, maxExcluded) {
    return Math.random() * (maxExcluded - minIncluded) + minIncluded;
}

Math.randomBetweenIncluded = function (minIncluded, maxIncluded) {
    return Math.random() * (maxIncluded - minIncluded + 1) + minIncluded;
}

Math.randomIntBetween = function (minIncluded, maxExcluded) {
    minIncluded = Math.ceil(minIncluded);
    maxExcluded = Math.floor(maxExcluded);
    return Math.floor(Math.random() * (maxExcluded - minIncluded)) + minIncluded;
}

Math.randomIntBetweenIncluded = function (minIncluded, maxIncluded) {
    minIncluded = Math.ceil(minIncluded);
    maxIncluded = Math.floor(maxIncluded);
    return Math.floor(Math.random() * (maxIncluded - minIncluded + 1)) + minIncluded;
}

