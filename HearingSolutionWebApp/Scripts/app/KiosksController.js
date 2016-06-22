(function () {
    'use strict';

    app.controller('kiosksController', kiosksController);

    kiosksController.$inject = ['$scope', 'KioskConnection', 'UserConnection', '$rootScope'];

    function kiosksController($scope, KioskConnection, UserConnection, $rootScope) {
        $scope.title = 'KiosksController';
        $scope.kioskConnection = KioskConnection;
        $scope.userConnection = UserConnection;

        function activate() {
            $scope.userConnection.start(); 
            $scope.kioskConnection.start();
        }
 
        //Pagination
        $scope.currentPage = 1; // keeps track of the current page
        $scope.pageSize = 10; // holds the number of items per page

        //Sorting
        $scope.sortType = "KioskDisplayName"; //current column table is sorted with
        $scope.sortReverse = false; //sorting direction

        //changin kiosk
        $scope.kioskChanged = function (kiosk) {
            $scope.kioskConnection.updateKiosk(kiosk);
            return false;
        }

        //deleting kiosk
        $scope.deleteKiosk = function (kiosk) {
            //todo confirm delete popup/dialog
            $scope.kioskConnection.deleteKiosk(kiosk.Id);
            return false;
        }




        //adding new kiosk

        var newKiosk;//stores new kiosk model

        $scope.addKiosk = function (data) {
            $scope.kioskConnection.addKiosk(data);
        }

        $scope.showAddKioskForm = function(localScope)
        {
            if (!newKiosk)//creates initial new kiosk or clears previous form values
                newKiosk = KioskConnection.newKiosk();
            else
                angular.extend(newKiosk,KioskConnection.newKiosk()); 
            localScope.addKioskForm.$show();
        }

        $scope.validateNewKiosk = function(kiosk)
        {
            //todo validation of new kiosk
            return true;
        }

        activate();
    }
})();
