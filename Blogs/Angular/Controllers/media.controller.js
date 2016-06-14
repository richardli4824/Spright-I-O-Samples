(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('adminBlogsMediaController', adminBlogsMediaController);

    adminBlogsMediaController.$inject = ['$scope', '$baseController', '$blogService', '$mediaUploaderService'];

    function adminBlogsMediaController(
        $scope
        , $baseController
        , $blogService
        , $mediaUploaderService) {

        var vm = this;
        vm.blog = null;
        vm.dropzone = null;
        vm.mediaId = null;
        vm.adminBlogId = null;
        vm.onUpdateBlogImageSubmit = null;
        vm.payload = null;
        vm.dzConfig = {
            autoProcessQueue: true,
            uploadMultiple: false,
            parallelUploads: 1,
            maxFiles: 1,
            maxFileSize: 5,
            url: "/api/MediaUploader/UploadWithData"
        };

        vm.$blogService = $blogService;
        vm.$mediaUploaderService = $mediaUploaderService;
        vm.$scope = $scope;

        vm.blogMediaPayload;
        vm.onPostBlogMediaSuccess = _onPostBlogMediaSuccess;
        vm.onGetMediabyBlogIdSuccess = _onGetMediabyBlogIdSuccess;
        vm.setMain = _setMain;
        vm.setMainOnSuccess = _setMainOnSuccess;

        vm.onUpdateImage = _onUpdateImage;
        vm.OnAnySuccess = _OnAnySuccess;
        vm.OnAnyError = _OnAnyError;

        vm.dzAddedFile = _dzAddedFile;
        vm.dzError = _dzError;
        vm.dzOnSending = _dzOnSending;
        vm.dzOnSuccess = _dzOnSuccess;

        $baseController.merge(vm, $baseController);

        vm.notify = vm.$blogService.getNotifier($scope);
        vm.notify = vm.$mediaUploaderService.getNotifier($scope);

        render();

        function render() {
            vm.adminBlogId = vm.$routeParams.adminBlogId;
            _loadBlogMedia();
        };

        function _dzAddedFile(file, response) {
        };

        function _dzError(file, errorMessage) {
        };

        function _dzOnSending(file, xhr, formData) {
            formData.append("Title", $('#Title').val());
            formData.append("Description", $('#Description').val());
        };

        function _dzOnSuccess(file, response) {
            vm.mediaId = response.item;
            vm.adminBlogId = vm.$routeParams.adminBlogId;
            vm.onUpdateImage(vm.mediaId);
            vm.dropzone.removeFile(file);
            vm.blogMediaPayload = {
                blogId: vm.adminBlogId,
                MediaId: vm.mediaId,
                IsCoverPhoto: 0
            }
            vm.$mediaUploaderService.postBlogMedia(vm.blogMediaPayload, vm.onPostBlogMediaSuccess, vm.OnAnyError)
        };
        function _onPostBlogMediaSuccess(object) {
            _loadBlogMedia();
        }
        function _loadBlogMedia() {
            vm.$mediaUploaderService.getBlogMedia(vm.adminBlogId, vm.onGetMediabyBlogIdSuccess)
        }

        function _onGetMediabyBlogIdSuccess(data) {
            vm.notify(function () {

                vm.blogMedia = data.items;

            });

        }

        function _OnAnySuccess(response) {
            vm.$alertService.success();

        };

        function _OnAnyError(response) {
            vm.$alertService.error();
        };

        function _onUpdateImage(mediaId) {
            vm.payload = {
                MediaId: mediaId
            }
        };

        function _onBlogImageSubmitError() {
            vm.$alertService.error();
        };

        function _setMain(mediaId) {
            vm.setMainPayload = {
                blogId: vm.adminBlogId,
                MediaId: mediaId
            }

            vm.$mediaUploaderService.CoverPhoto(vm.setMainPayload, vm.setMainOnSuccess, vm.OnAnyError);

        }

        function _setMainOnSuccess() {
            _loadBlogMedia()
        }
    }
})();