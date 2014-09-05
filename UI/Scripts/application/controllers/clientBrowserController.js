'use strict';

//controller for the main view
uiAngularControllers.controller('clientBrowserController', ['$scope', '$rootScope', '$http', '$location', function ($scope, $rootScope, $http, $location) {
 
    $scope.gridOptions = { data: 'myData' };
    $scope.name="";
    $scope.possiblyStolen;

    $scope.newClient = { id: '', name: '', initialDeposit: 0 };
    $scope.updateClient = { id: '', quantity: 0, inATM: null };
    
    $scope.readFilteredClients = function () {
       
        $http.get('/api/Clients/', {
                params: {
                    clientName: $scope.name,
                    possiblyStolen: $scope.possiblyStolen
                }
            })
            .success(function (data, status, headers, config) {
                 $scope.myData=data;
             
            }).
            error(function (data, status, headers, config) {
                // log error
            });
    };

    $scope.submitNewClient = function () {

        $http.post('/api/Clients/', 
            $scope.newClient
        )
         .success(function (data, status, headers, config) {
           
         }).
         error(function (data, status, headers, config) {
             // log error
         });

    };

    $scope.submitUpdateClient = function () {

        $http.put('/api/Clients/',
            $scope.updateClient
        )
         .success(function (data, status, headers, config) {

         }).
         error(function (data, status, headers, config) {
             // log error
         });

    };

    $scope.readFilteredClients();
}]);