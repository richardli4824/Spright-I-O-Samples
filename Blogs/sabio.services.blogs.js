if (!sabio.services.blogs)
    sabio.services.blogs = {}

sabio.services.blogs.insertBlog = function (payLoad, onSuccess, onError) {
    $.ajax({
        type: 'POST',
        url: '/api/Admin/Blogs',
        data: payLoad,
        dataType: 'json',
        success: onSuccess,
        error: onError
    });
};

sabio.services.blogs.updateBlog = function (payLoad, blogsId, onSuccess, onError) {
    $.ajax({
        type: 'PUT',
        url: '/api/Admin/Blogs/' + blogsId,
        data: payLoad,
        dataType: 'json',
        success: onSuccess,
        error: onError
    });
};

sabio.services.blogs.blogslistJson = function (onSuccess, onError) {
    $.ajax({
        type: 'GET',
        url: '/api/Admin/Blogs',
        dataType: 'json',
        success: onSuccess,
        error: onError
    });
};

sabio.services.blogs.getBlogById = function (blogsId, onSuccess, onError) {
    $.ajax({
        type: 'GET',
        url: '/api/Admin/Blogs/' + blogsId,
        dataType: 'json',
        success: onSuccess,
        error: onError
    });
};

sabio.services.blogs.deleteBlogsById = function (blogsId, onSuccess, onError) {
    $.ajax({
        type: 'DELETE',
        url: '/api/Admin/Blogs/' + blogsId,
        success: onSuccess,
        error: onError
    });
};

sabio.services.blogs.listBlogsByWebsiteId = function (websiteId, payload, onSuccess, onError) {
    $.ajax({
        type: 'GET',
        url: '/api/Admin/Blogs/website/' + websiteId,
        data: payload,
        dataType: 'json',
        success: onSuccess,
        error: onError
    });
};

