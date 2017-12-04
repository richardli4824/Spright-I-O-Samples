if (!spright.services.blogs)
    spright.services.blogs = {}

// Ajax Call to Create and Insert a Blog
spright.services.blogs.insertBlog = function (payLoad, onSuccess, onError) {
    $.ajax({
        type: 'POST',
        url: '/api/Admin/Blogs',
        data: payLoad,
        dataType: 'json',
        success: onSuccess,
        error: onError
    });
};

// Ajax Call to Update a Blog By Id
spright.services.blogs.updateBlog = function (payLoad, blogsId, onSuccess, onError) {
    $.ajax({
        type: 'PUT',
        url: '/api/Admin/Blogs/' + blogsId,
        data: payLoad,
        dataType: 'json',
        success: onSuccess,
        error: onError
    });
};

// Ajax Call to Retrive All Blogs
spright.services.blogs.blogslistJson = function (onSuccess, onError) {
    $.ajax({
        type: 'GET',
        url: '/api/Admin/Blogs',
        dataType: 'json',
        success: onSuccess,
        error: onError
    });
};

// Ajax Call to Retrieve A Blog By Id
spright.services.blogs.getBlogById = function (blogsId, onSuccess, onError) {
    $.ajax({
        type: 'GET',
        url: '/api/Admin/Blogs/' + blogsId,
        dataType: 'json',
        success: onSuccess,
        error: onError
    });
};

// Ajax Call to Delete a Blog
spright.services.blogs.deleteBlogsById = function (blogsId, onSuccess, onError) {
    $.ajax({
        type: 'DELETE',
        url: '/api/Admin/Blogs/' + blogsId,
        success: onSuccess,
        error: onError
    });
};

// Ajax Call to Retrieve all the Blogs Specific to the Website
spright.services.blogs.listBlogsByWebsiteId = function (websiteId, payload, onSuccess, onError) {
    $.ajax({
        type: 'GET',
        url: '/api/Admin/Blogs/website/' + websiteId,
        data: payload,
        dataType: 'json',
        success: onSuccess,
        error: onError
    });
};

