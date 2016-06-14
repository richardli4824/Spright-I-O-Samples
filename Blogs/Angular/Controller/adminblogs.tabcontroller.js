(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('adminBlogsTabController', adminBlogsTabController);

    adminBlogsTabController.$inject = ['$scope', '$baseController', '$routeParams'];

    function adminBlogsTabController(
        $scope
        , $baseController
        , $routeParams) {

        var vm = this;

        $baseController.merge(vm, $baseController);

        vm.$scope = $scope;
        vm.$routeParams = $routeParams;

        vm.currentRequestLabel = "Current Request:";
        vm.adminBlogId = vm.$routeParams.adminBlogId;
        vm.websiteId = vm.$routeParams.websiteId;

        vm.tabs = [
          { link: '#/blog/' + vm.websiteId + '/edit/' + vm.adminBlogId, label: 'Blog', controller: 'adminBlogsContentController' },
          { link: '#/blog/' + vm.websiteId + '/media/' + vm.adminBlogId, label: 'Media', controller: 'adminBlogsMediaController' },
        ];

        vm.setUpCurrentRequest(vm);

        vm.selectedTab = vm.tabs[0];

        for (var x = 0; x <= vm.tabs.length; x++) {
            if (vm.currentRequest.$$route.controller == vm.tabs[x].controller) {
                vm.selectedTab = vm.tabs[x];
                break;
            }
        };

        vm.tabClass = _tabClass;
        vm.setSelectedTab = _setSelectedTab;

        render();

        function render() {

        };

        function _tabClass(tab) {
            if (vm.selectedTab == tab) {
                return "active";
            } else {
                return "";
            }
        };

        function _setSelectedTab(tab) {
            vm.selectedTab = tab;
        };
    }
})();