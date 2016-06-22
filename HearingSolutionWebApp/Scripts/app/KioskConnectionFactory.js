(function () {
    'use strict';

    app.factory('KioskConnection', ['$rootScope', '$q', function ($rootScope, $q) {

        //this object will be exposed to controller and view
        var KioskConnection = this;

        KioskConnection.kiosks = [];


        //Kiosk ViewModel
        function Kiosk(kiosk) {
            if (!kiosk) kiosk = {};

            var Kiosk = {
                Id: kiosk.Id || null,
                KioskDisplayName: kiosk.KioskDisplayName || null,
                StoreContact: kiosk.StoreContact || null,
                Email: kiosk.Email || null,
                Phone: kiosk.Phone || null,
                LocationName: kiosk.LocationName || null,
                CompanyName: kiosk.CompanyName || null,
                DateDeleted: kiosk.DateDeleted || null,
                KioskType: kiosk.KioskType || null,
                OwnerId : kiosk.OwnerId || null,
                IsUpdating: false,
                UpdateFailed: kiosk.UpdateFailed || false,
                IsNew: kiosk.IsNew || false
            };
            return Kiosk;
        }

        var kioskHub;
        var connection;
        KioskConnection.loading = true;
        KioskConnection.connected = false;
        KioskConnection.newKiosk = Kiosk;//easy way to expose the function outside

        //Connection start fired by controller on activation; creates new connection if not already connected
        KioskConnection.start = function () {
            if (!KioskConnection.connected) {       
                connection = $.hubConnection();
                kioskHub = connection.createHubProxy('kioskHub');
                kioskHub.on('allKiosksData', allKiosksData);
                kioskHub.on('allKiosksData',allKiosksData);
                kioskHub.on('kioskUpdateFailed',kioskUpdateFailed);
                kioskHub.on('kioskUpdated',kioskUpdated);
                kioskHub.on('kioskAdded',kioskAdded);
                kioskHub.on('kioskRemoved', kioskRemoved);
                connection.start().done(function () { kioskHub.invoke('getAllKiosks'); KioskConnection.connected = true; });
            }
        }


        //Client side functions fired by server

        function allKiosksData(data) {
            KioskConnection.kiosks.length = 0;
            angular.forEach(data, function (kiosk) {
                KioskConnection.kiosks.push(new Kiosk(kiosk));
            });
            KioskConnection.loading = false;
            $rootScope.$apply();
        }

        function kioskUpdateFailed(kiosk) {
            var existingKiosk = find(kiosk.Id);
            angular.extend(existingKiosk, new Kiosk(kiosk));
            $rootScope.$apply();
        }

        function kioskUpdated(kiosk) {
            var existingKiosk = find(kiosk.Id);
            angular.extend(existingKiosk, new Kiosk(kiosk));
            $rootScope.$apply();
        }

        function kioskAdded(kiosk) {
            KioskConnection.kiosks.push(new Kiosk(kiosk));
            $rootScope.$apply();
        }

        function kioskRemoved(kioskId) {
            var item = find(kioskId);
            var index = $scope.kiosks.indexOf(item);
            $scope.kiosks.splice(index, 1);
            $rootScope.$apply();
        }


        //functions fired by view to be passed on server

        KioskConnection.addKiosk = function (kiosk) {
            kioskHub.invoke('addKiosk', kiosk);
        }

        KioskConnection.deleteKiosk = function (kioskId) {
            kioskHub.invoke('deleteKiosk',kioskId);
        }

        KioskConnection.updateKiosk = function (kiosk) {
            find(kiosk.Id).IsUpdating = true;
            kioskHub.invoke('updateKiosk',kiosk);
        }

        //finding helper
        var find = function (id) {
            for (var i = 0; i < KioskConnection.kiosks.length; i++) {
                if (KioskConnection.kiosks[i].Id == id) return KioskConnection.kiosks[i];
            }
            return null;
        };

        return KioskConnection;
    }
    ])
})();