'use strict';

//controller for the main view
uiAngularControllers.controller('mainViewController', ['$scope', '$rootScope', '$http', '$location', 'ngDialog', function ($scope, $rootScope, $http, $location, ngDialog) {

    //what has to be loaded when the tab changes
    $scope.tabSelected = function (tabId) {

        if (tabId == 0)
            $location.path("/Home/DashBoard");
        else if (tabId == 1)
            $location.path("/Home/ClientBrowser");
    };





    $scope.isViewLoading = false;
    $scope.$on('$routeChangeStart', function () {
        $scope.isViewLoading = true;
    });
    $scope.$on('$routeChangeSuccess', function () {
        $scope.isViewLoading = false;
    });

  
}]);