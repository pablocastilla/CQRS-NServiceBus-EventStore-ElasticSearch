'use strict';

//controller for the main view
uiAngularControllers.controller('clientBrowserController', ['$scope', '$rootScope', '$http', '$location', function ($scope, $rootScope, $http, $location) {
 
    $scope.gridOptions = { data: 'myData' };
    $scope.name;
    
    $scope.readFilteredClients = function () {
       
        $http.get('/api/Clients/', {
                params: {
                    clientName: $scope.name
                   
                }
            })
            .success(function (data, status, headers, config) {
                 $scope.myData=data;
             
            }).
            error(function (data, status, headers, config) {
                // log error
            });
    };

    $scope.readFilteredClients();
}]);