angular
    .module('fortunes', ['ngRoute'])
    .config(function ($routeProvider) {
        $routeProvider.when('/', {
            templateUrl: 'fortune.html',
            controller: 'fortune'
        }).when('/random2', {
            templateUrl: 'fortune.html',
            controller: 'fortune'
        })
        .when('/multiple', {
            templateUrl: 'multifortune.html',
            controller: 'fortune'
        })
    })
    .controller('fortune', function ($scope, $location, $http) {
        console.log($location.$$path);
        switch ($location.$$path) {
            case "/random2":
                $http.get('random2').success(function (data) {
                    $scope.fortune = data;
                });
                break;
            case "/multiple":
                $http.get('multirandom').success(function (data) {
                    $scope.fortunes = data;
                });
                break;
            default:
                $http.get('random').success(function (data) {
                    $scope.fortune = data;
                });
                break;
        }
    })
