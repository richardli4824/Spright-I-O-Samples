(function (moduleOptions) {
    "use strict";

    //  https://github.com/johnpapa/angular-styleguide#moduleOptionss
    var defaultDependencies = [
        'ui.bootstrap',
        'ngRoute',
        'ngAnimate',
        'toastr'
    ];

    var arrOfDep = getModuleDependencies(moduleOptions, defaultDependencies);
    //console.log('array of dependencies', sabio);

    var app = angular.module(moduleOptions.APPNAME, arrOfDep);

    app.value('$sabio', moduleOptions.page);

    if (moduleOptions)
    {
        processModuleOptions(moduleOptions, app);
    }


    function getModuleDependencies(opts, defaults) {
        if (opts && opts.extraModuleDependencies) {
            var newItems = defaults.concat(opts.extraModuleDependencies);
            return newItems;
        }
        return defaults;
    }

    function processModuleOptions(opts, clientApp) {
        if (!opts) {
            return;
        }

        if (opts.runners) {
            for (var i = 0; i < opts.runners.length; i++) {
                var runner = opts.runners[i];
                clientApp.run(runner);
            }
        }


    }

})(sabio.moduleOptions);