var app = angular.module("main", ['ngRoute', 'ngAnimate', 'ui.bootstrap','ngGrid', 'uiAngularControllers']);
app.value('$', $);

var uiAngularControllers = angular.module("uiAngularControllers", []);


//URL MAPPINGS
app.config(function ($routeProvider) {
    $routeProvider.
      when('/Home/DashBoard', {
          templateUrl: '/Home/DashBoard',
          controller: 'dashBoardController'
      }).
      when('/Home/ClientBrowser', {
          templateUrl: '/Home/ClientBrowser',
          controller: 'clientBrowserController'
      }).   
      otherwise({
          redirectTo: '/404'
      });


});

//Initialization
/*app.run(['SignalRHubService', function (SignalRHubService) {

    SignalRHubService.initialize();



}]);*/

