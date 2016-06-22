(function () {
    'use strict';

    app.controller('MainController', MainController);

    MainController.$inject = ['$scope'];

    function MainController($scope) {
        $scope.title = 'MainController';

        activate();

        function activate() {
            $scope.message = 'Hello from Main';
        }
    }
})();
