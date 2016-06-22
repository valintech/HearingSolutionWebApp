(function () {
    'use strict';

    app.controller('UsersController', UsersController);

    UsersController.$inject = ['$scope'];

    function UsersController($scope) {
        $scope.title = 'UsersController';

        activate();

        function activate() {
            $scope.message = 'Hello from Users';
        }
    }
})();
