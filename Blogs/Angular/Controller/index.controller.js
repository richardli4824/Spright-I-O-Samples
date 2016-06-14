(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('indexController', IndexController);

    IndexController.$inject = ['$scope', '$baseController', "$blogService"];

    function IndexController(
        $scope
        , $baseController
        , $blogService) {

        var vm = this;
        vm.adminBlogs = null;
        vm.selectedBlog = null;

        vm.$blogService = $blogService;
        vm.$scope = $scope;

        vm.listBlogs = _listBlogs;
        vm.deleteBlogsById = _deleteBlogsById;
        vm.blogDeleteSuccess = _blogDeleteSuccess;

        $baseController.merge(vm, $baseController);

        vm.notify = vm.$blogService.getNotifier($scope);

        render();

        function render() {
            vm.$blogService.blogslistJson(vm.listBlogs, _onBlogError);
        }

        function _listBlogs(data) {
            vm.notify(function () {
                vm.blogs = data.items;
            });
        }

        function _onBlogError(jqXhr, error) {
            console.error(error);
            vm.$alertService.error();
        };

        function _deleteBlogsById(id) {
            if (confirm("Are you sure you want to delete this Blog?")) {
                vm.$blogService.deleteBlogsById(id, _blogDeleteSuccess, _blogDeleteError);
            }
        };

        function _blogDeleteSuccess() {
            vm.$alertService.success();
            render();
        };

        function _blogDeleteError() {
            vm.$alertService.error();
        };
    }
})();