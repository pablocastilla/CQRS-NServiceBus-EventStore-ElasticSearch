var app = angular.module("main", ['ngRoute', 'uiAngularControllers', 'ui.bootstrap']);
app.value('$', $);

var uiAngularControllers = angular.module("uiAngularControllers", []);


//URL MAPPINGS
app.config(function ($routeProvider) {
    $routeProvider.
      when('/Home/DashBoard', {
          templateUrl: '/Home/DashBoard',
          controller: 'dashBoardController'
      }).
      when('/Home/MakeDeposit', {
          templateUrl: '/Home/MakeDeposit',
          controller: 'makeDepositController'
      }).   
      otherwise({
          redirectTo: '/404'
      });


});

//Initialization
/*app.run(['SignalRHubService', function (SignalRHubService) {

    SignalRHubService.initialize();



}]);*/

