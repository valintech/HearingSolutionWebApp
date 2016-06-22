(function () {
    'use strict';

    app.controller('OverviewController', OverviewController);

    OverviewController.$inject = ['$scope']; 

    function OverviewController($scope) {
        $scope.title = 'OverviewController';

        activate();

        function activate() {
            $scope.message = 'Hello from Overview';
        }
    }
})();
