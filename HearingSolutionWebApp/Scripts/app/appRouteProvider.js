(function () {
    'use strict';

    app.config(function ($routeProvider,$locationProvider) {
        $routeProvider

        .when('/', {
            templateUrl: 'Pages/Overview.html',
            controller: 'MainController'
        })

        .when('/overview', {
            templateUrl: 'Pages/Overview.html',
            controller: 'MainController'
        })

        .when('/kiosks', {
            templateUrl: 'Pages/KiosksView.html',
            controller: 'MainController'
        })

        .when('/transactions', {
            templateUrl: 'Pages/Transactions.html',
            controller: 'MainController'
        })

        .when('/users', {
            templateUrl: 'Pages/Users.html',
            controller: 'MainController'
        })
        .otherwise({ redirectTo: '/' });
    });

})();