'use strict';

angular.module('Home')

.factory('HomeService', 
    ['$http',
    function ($http) {
        var service = {};

        service.GetSecureData = function (callback) {
            $http.get('http://localhost:2661/securedata')
                .success(function (response) {
                    callback(response);
                });
        };

        return service;
    }]);