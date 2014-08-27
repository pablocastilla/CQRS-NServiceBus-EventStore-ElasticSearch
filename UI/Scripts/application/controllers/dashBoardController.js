'use strict';

//controller for the main view
uiAngularControllers.controller('dashBoardController', ['$scope', '$rootScope', '$http', '$location', function ($scope, $rootScope, $http, $location) {

    $scope.TotalMoneyInBank = 0;


    $scope.readDashBoardData = function () {

        $http.get("/api/DashBoard/").
            success(function (data, status, headers, config) {
                $scope.TotalMoneyInBank = data.TotalMoneyInBank;
            }).
            error(function (data, status, headers, config) {
                // log error
            });
    };

    $scope.readDashBoardData();
}]);