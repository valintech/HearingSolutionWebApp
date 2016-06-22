(function () {
    'use strict';

    app.factory('UserConnection', ['$rootScope', '$q', function ($rootScope, $q) {

        //this object will be exposed to controller and view
        var UserConnection = this;

        UserConnection.users = [];

        //User ViewModel
        var User = function (user) {
            if (!user) user = {};

            var User = {
                Id: user.Id || null,
                UserName: user.UserName || null,
                CompanyName: user.CompanyName || null,
                //todo fill rest of user properies
                IsUpdating: false,
                UpdateFailed: user.UpdateFailed || false,
                IsNew: user.IsNew || false
            };
            return User;
        }

        var connection;
        var userHub;
        UserConnection.loading = true;
        UserConnection.connected = false;

        //Connection start fired by controller on activation; creates new connection if not already connected
        UserConnection.start = function () {
            if (!UserConnection.connected) {
                connection = $.hubConnection();
                userHub = connection.createHubProxy('userHub');
                userHub.on('allUsersData' ,allUsersData);
                userHub.on('userUpdateFailed' ,userUpdateFailed);
                userHub.on('userUpdated' ,userUpdated);
                userHub.on('userAdded' , userAdded);
                userHub.on('userRemoved ' ,userRemoved);
                connection.start().done(function () { userHub.invoke('getAllUsers'); UserConnection.connected = true; });
            }
        }




        //Client side functions fired by server

        function allUsersData(data) {
            UserConnection.users.length = 0;
            angular.forEach(data, function (user) {
                UserConnection.users.push(new User(user));
            });
            UserConnection.loading = false;
            $rootScope.$apply();
        }

        function userUpdateFailed(user) {
            var existingUser = find(user.Id);
            angular.extend(existingUser, new User(user));
            $rootScope.$apply();
        }

        function userUpdated(user) {
            var existingUser = find(user.Id);
            angular.extend(existingUser, new User(user));
            $rootScope.$apply();
        }

        function userAdded(user) {
            UserConnection.users.push(new User(user));
            $rootScope.$apply();
        }

        function userRemoved(userId) {
            var item = find(userId);
            var index = $scope.bdays.indexOf(item);
            $scope.bdays.splice(index, 1);
            $rootScope.$apply();
        }


        //functions fired by view to be passed on server

        UserConnection.addUser = function (user) {
            userHub.invoke('addUser',user)
        }

        UserConnection.deleteUser = function (userId) {
            userHub.invoke('deleteUser',serId);
        }

        UserConnection.updateUser = function (user) {
            find(user.Id).IsUpdating = true;
            userHub.invoke('updateUser',user);
        }

        //finding helper
        var find = function (id) {
            for (var i = 0; i < UserConnection.users.length; i++) {
                if (UserConnection.users[i].Id == id) return UserConnection.users[i];
            }
            return null;
        };

        return UserConnection;
    }
    ])
})();