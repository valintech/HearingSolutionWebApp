(function () {
    'use strict';

    app.filter('pageFilter', function () {
        return function (input, start) {
            if (!input || !input.length) { return; }
            start = +start;
            return input.slice(start);
        };
    });

    app.filter('sortFilter', function () {
        return function (items, field, reverse) {
            var filtered = [];
            angular.forEach(items, function (item) {
                filtered.push(item);
            });
            filtered.sort(function (a, b) {
                return (a[field] > b[field] ? 1 : -1);
            });
            if (reverse) filtered.reverse();
            return filtered;
        };
    });
})();