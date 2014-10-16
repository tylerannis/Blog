$(document).ready(function () {
    //show and hide the comments
    $('.showComments').on('click', function () {
        //selecting the parent
        var parent = $(this).parent();
        //select comment to show or hide
        var commentToShow = parent.find('.commentsDiv');
        commentToShow.slideToggle();

    });

    //click on the like area
    $('.like').on('click', function () {
        //setting what post does the user want
        var postId = $(this).data('postid');
        //store the (this) element into a variable
        var likeButton = $(this);
        //doin an ajax get the (data) is the html that the function will get
        $.get('/Home/LikePost/' + postId, function (data) {
            //replacing the html with the data
            likeButton.parent().html(data);
        });

    });
    //wiring up the ajax post for the comments form
    $('.commentsDiv form').on('submit', function (event) {
        event.preventDefault();
       
        var theForm = $(this);
        //to get the html you use the action attribute, then to convert form data to a string you seralize it
        $.post(theForm.attr('action'), theForm.serialize(), function (data) {
            //update the html
            theForm.before(data);
            //clear out the values of the comment form
            theForm.find('.commentAuthor, .commentBody').val('');
        });

    });

    
});