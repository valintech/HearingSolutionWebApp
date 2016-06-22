(function () {
    'use strict';

    app.controller('TransactionsController', TransactionsController);

    TransactionsController.$inject = ['$scope'];

    function TransactionsController($scope) {
        $scope.title = 'TransactionsController';

        activate();

        function activate() {
            $scope.message = 'Hello from Transactions';
        }
    }
})();
