(function () {
    "use strict";

    angular.module(APPNAME)
        .config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {

            $routeProvider.when('/edit/:adminBlogId', {
                templateUrl: '/Scripts/sabio/application/adminblogs/templates/content.html',
                controller: 'adminBlogsContentController',
                controllerAs: 'aBContent'
            }).when('/media/:adminBlogId', {
                templateUrl: '/Scripts/sabio/application/adminblogs/templates/media.html',
                controller: 'adminBlogsMediaController',
                controllerAs: 'aBMedia'
            }).when('/', {
                templateUrl: '/Scripts/sabio/application/adminblogs/templates/index.html',
                controller: 'indexController',
                controllerAs: 'index'
            }).when('/create', {
                templateUrl: '/Scripts/sabio/application/adminblogs/templates/create.html',
                controller: 'adminBlogsContentController',
                controllerAs: 'aBContent'
            })

            $locationProvider.html5Mode(false);

        }]);

})();