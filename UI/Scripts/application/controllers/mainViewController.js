'use strict';

//controller for the main view
uiAngularControllers.controller('mainViewController', ['$scope', '$rootScope', '$http', '$location', function ($scope, $rootScope, $http,  $location) {

    //what has to be loaded when the tab changes
    $scope.tabSelected = function (tabId) {

        if (tabId == 0)
            $location.path("/Home/DashBoard");
        else if (tabId == 1)
            $location.path("/Home/MakeDeposit");
    };


    $scope.isViewLoading = false;
    $scope.$on('$routeChangeStart', function () {
        $scope.isViewLoading = true;
    });
    $scope.$on('$routeChangeSuccess', function () {
        $scope.isViewLoading = false;
    });
}]);