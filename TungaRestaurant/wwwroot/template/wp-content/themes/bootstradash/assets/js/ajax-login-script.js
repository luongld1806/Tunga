jQuery(document).ready(function () {


    // //Display form from link inside a popup
    // jQuery('#pop_login').on('click', function (e) {
    //     formToFadeOut = jQuery('form#register');
    //     formtoFadeIn = jQuery('form#custom_login');
    //     if (jQuery(this).attr('id') == 'pop_signup') {
    //         formToFadeOut = jQuery('form#custom_login');
    //         formtoFadeIn = jQuery('form#register');
    //     }
    //     formToFadeOut.fadeOut(500, function () {
    //         formtoFadeIn.fadeIn();
    //     })
    //     return false;
    // });
    jQuery("document").ready(function(){
    jQuery("form#register").hide();
});
    jQuery("#pop_login").click(function(){
        jQuery("form#register").hide().attr("formnovalidate");
        jQuery("form#custom_login").toggle();
    });

    jQuery("#pop_signup").click(function(){
        jQuery("form#custom_login").hide().attr("formnovalidate");
        jQuery("form#register").toggle();
    });


 
    jQuery(document).on('click', '.login_overlay, .close', function () {
        jQuery('form#custom_login, form#register').fadeOut(500, function () {
            jQuery('.login_overlay').remove();
        });
        return false;
    });
 
    // Show the login/signup popup on click
    jQuery('#show_login, #show_signup').on('click', function (e) {
        jQuery('body').prepend('<div class="login_overlay"></div>');
        if (jQuery(this).attr('id') == 'show_login') 
            jQuery('form#custom_login').fadeIn(500);
        else 
            jQuery('form#register').fadeIn(500);
        e.preventDefault();
    });
 
    // Perform AJAX login/register on form submit
    jQuery('form#custom_login, form#register').on('submit', function (e) {
        jQuery('p.status', this).show().text(ajax_auth_object.loadingmessage);
        action = 'ajaxlogin';
        username =  jQuery('form#custom_login #username').val();
        if (username ==''){
            jQuery('p.status', this).show().text('required username');
            jQuery('form#custom_login #username').focus();
            return false;
        }
        password = jQuery('form#custom_login #password').val();
        external = jQuery('form#custom_login #external_url').val();
        email = '';
        security = jQuery('form#custom_login #security').val();
        if (jQuery(this).attr('id') == 'register') {
            action = 'ajaxregister';
            username = jQuery('#signonname').val();
            password = jQuery('#signonpassword').val();
            email = jQuery('#email').val();
            security = jQuery('#signonsecurity').val();  
        }  
        ctrl = jQuery(this);
        jQuery.ajax({
            type: 'POST',
            dataType: 'json',
            url: ajax_auth_object.ajaxurl,
            data: {
                'action': action,
                'username': username,
                'password': password,
                'email': email,
                'security': security
            },
            success: function (data) {
                jQuery('p.status', ctrl).text(data.message);
                if (data.loggedin == true) {
                    externalurl = ajax_auth_object.redirecturl+'?external-url='+external;
                    document.location.href = externalurl;
                }
            }
        });
        e.preventDefault();
    });
    
    // Client side form validation
   if (jQuery("#register").length) 
        jQuery("#register").validate(
        { 
            rules:{
            password2:{ equalTo:'#signonpassword' 
            }   
        }}
        );
    else if (jQuery("#login").length) 
        jQuery("#login").validate();
});