// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


jQuery('#submitBtn').click(function () {

    var favorite = [];
    $.each(jQuery("[name='Candidate']:checked"), function () {
        favorite.push($(this).data('name'));
    });

    if (favorite.length > 0) {
        jQuery('#lname').html(favorite.join("<br>"));
        jQuery('#candidato').show();
        jQuery('#blanco').hide();
    } else {
        jQuery('#candidato').hide();
        jQuery('#blanco').show();
    }
});


jQuery('#submitBtn1').click(function () {
    jQuery('#nulo').show();
});


jQuery('#submit').click(function () {
    /* when the submit button in the modal is clicked, submit the form */
    jQuery('#submit').attr("disabled", true);
    jQuery('#vote').submit();
});

jQuery('#submit1').click(function () {
    /* when the submit button in the modal is clicked, submit the form */
    jQuery('#submit1').attr("disabled", true);
    jQuery('#VoteNull').val(true);
    jQuery('#vote').submit();
});

//jQuery("[name='Candidate']").on('click', function () {
//    if (jQuery(this).val() === 'true') {
//        jQuery('#submitBtn').attr("disabled", "disabled");
//    }
//    else {
//        jQuery('#submitBtn').removeAttr("disabled");
//    }
//});

