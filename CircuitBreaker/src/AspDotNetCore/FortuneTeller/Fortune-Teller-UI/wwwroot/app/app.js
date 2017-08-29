angular.module('fortunes', ['ngRoute']).config(function ($routeProvider) {

    $routeProvider.when('/', {
        templateUrl: 'fortune.html',
        controller: 'fortune'
    })
    .when('/multiple', {
        templateUrl: 'multifortune.html',
        controller: 'multifortune'
    })

}).controller('fortune', function ($scope, $http) {

    $http.get('random').success(function (data) {
        $scope.fortune = data;
    })

})
.controller('multifortune', function ($scope, $http) {

    $http.get('multirandom').success(function (data) {
        $scope.fortunes = data;
    })
});