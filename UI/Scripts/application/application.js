var app = angular.module("main", ['ngRoute', 'ngAnimate', 'ui.bootstrap', 'ngGrid', 'uiAngularControllers', 'uiAngularServices', 'ngDialog']);
app.value('$', $);

var uiAngularControllers = angular.module("uiAngularControllers", []);
var uiAngularServices = angular.module('uiAngularServices', []);


app.config(['ngDialogProvider', function (ngDialogProvider) {
    ngDialogProvider.setDefaults({
        className: 'ngdialog-theme-default',
        plain: false,
        showClose: true,
        closeByDocument: true,
        closeByEscape: true,
        appendTo: false
    });
}]);

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
app.run(['SignalRHubService', function (SignalRHubService) {

    SignalRHubService.initialize();



}]);

