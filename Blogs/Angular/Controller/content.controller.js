(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('adminBlogsContentController', adminBlogsContentController);

    adminBlogsContentController.$inject = ['$scope', '$baseController', '$blogService', '$textAngularService'];

    function adminBlogsContentController(
        $scope
        , $baseController
        , $blogService
        , $textAngularService) {

        var vm = this;
        vm.blog = null;
        vm.dt = null;

        vm.addBlog = _addBlog;
        vm.slugCat = _slugCat;
        vm.today = _today;
        vm.open1 = _open1;
        vm.setDate = _setDate;

        $baseController.merge(vm, $baseController);

        vm.$blogService = $blogService;
        vm.$scope = $scope;
        vm.$textAngularService = $textAngularService;

        vm.notify = vm.$blogService.getNotifier($scope);
        vm.notify = vm.$textAngularService.getNotifier($scope);

        render();

        function render() {
            vm.adminBlogId = vm.$routeParams.adminBlogId;
            if (vm.adminBlogId && vm.adminBlogId.length > 0) {
                vm.$blogService.getBlogById(vm.adminBlogId, _GetBlogSuccess, _onGetBlogError);
            };
        };

        function _addBlog() {
            vm.showNewBlogErrors = true;
            vm.websiteId = vm.$routeParams.websiteId;

            if (vm.blogForm.$valid) {

                var payload = vm.blog;
                payload.publishDate = moment(payload.publishDate).format('YYYY-MM-DD, hh:mm:ss a');
                payload.websiteId = vm.websiteId;

                if (vm.adminBlogId && vm.adminBlogId.length > 0) {
                    vm.$blogService.updateBlog(payload, vm.adminBlogId, _updateBlogSuccess, _updateBlogError);
                }
                else {

                    vm.$blogService.insertBlog(payload, _insertBlogSuccess, _insertBlogError);
                }
            }

            else {
            };
        };

        function _setDate(year, month, day) {
            vm.dt = new Date(year, month, day);
        };

        function _open1() {
            vm.popup1.opened = true;
        };

        vm.popup1 = {
            opened: false
        };

        var tomorrow = new Date();
        tomorrow.setDate(tomorrow.getDate() + 1);
        var afterTomorrow = new Date();
        afterTomorrow.setDate(tomorrow.getDate() + 1);
        vm.events = [
          {
              date: tomorrow,
              status: 'full'
          },
          {
              date: afterTomorrow,
              status: 'partially'
          }
        ];

        function _today() {
            vm.dt = new Date();
        };
        _today();

        function _GetBlogSuccess(data) {
            vm.notify(function () {
                vm.blog = data.item;
            });
        };

        function _onGetBlogError(jqXhr, error) {
            console.error(error);
            vm.$alertService.error();
        };

        function _updateBlogSuccess() {
            vm.$alertService.success();
        };

        function _updateBlogError() {
            vm.$alertService.error();
        };

        function _insertBlogSuccess() {
            vm.$alertService.success();
        };

        function _insertBlogError() {
            vm.$alertService.error();
        };

        function _slugCat(text) {

            vm.blog.slug = text.toLowerCase().replace(/[^\w ]+/g, '').replace(/ +/g, '-');
        };

        function _checkboxModel() {
            value1: true
            value2: true
        };
    }
})();