
jQuery(document).ready(function ($) {
   

    

	// Perform AJAX login/register on form submit
	jQuery('form#login, form#register').on('submit', function (e) {
       /* 
		if($('form#login #bd-log-username').val()==''){
			 $('form#login p.bd-login-notice').text('Username required.');
			 $('form#login #bd-log-username').focus();
			return false;
		}
		 else
			 $('form#login p.bd-login-notice').text('');
		 if($('form#login #bd-log-password').val()==''){
			 $('form#login p.bd-login-notice').text('Password required.');
			 $('form#login #bd-log-password').focus();
			 return false;
		 }
		 else
			 $('form#login p.bd-login-notice').text('');
		 */
		
		 $('p.bd-notice', this).text(ajax_auth_object.loadingmessage);
		$('.bd-btn', this).css("opacity","0.5");
		
		 
		action = 'ajaxlogin';
		username = 	$('form#login #bd-log-username').val();
		password = $('form#login #bd-log-password').val();
		email = '';
		security = jQuery('form#login #security').val();
		if (jQuery(this).attr('id') == 'register') {
			if ($('#bd-checkbox').is(":checked"))
			{
	  			
			}
	 		else {
	       		 $('form#register p.bd-notice').text('Please accept our terms and conditions');
	       		 $('form#register .bd-btn').css("opacity","1");
	       		 return false;
	    	}
			
				action = 'ajaxregister';
				if($('#reg-username'))
					username = $('#reg-username').val();
				else
					username='';
				password = $('#reg-password').val();
				email = $('#reg-email').val();
				security = $('#signonsecurity').val();
	
 

		}  
		ctrl = $(this);
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
				$('.bd-btn', ctrl).css("opacity","1");
				$('p.bd-notice', ctrl).text(data.message);
				if (data.loggedin == true) {
                    document.location.href = ajax_auth_object.redirecturl;
                }
            }
        });
		
        e.preventDefault();
    });
	
	$('form#forgot_password').on('submit', function(e){
		ctrl = $(this);
		$('.bd-btn', ctrl).css("opacity","0.5");
		 $('p.bd-notice', ctrl).show().text(ajax_auth_object.loadingmessage);
		 		
		 $.ajax({
		 type: 'POST',
					dataType: 'json',
					url: ajax_auth_object.ajaxurl,
		 data: { 
		 'action': 'ajaxforgotpassword', 
		 'user_login': $('#user_login').val(), 
		 'security': $('#forgotsecurity').val(), 
		 },
		 success: function(data){ 
			$('p.bd-notice',ctrl).text(data.message);
			$('.bd-btn', ctrl).css("opacity","1");
			
			if (data.loggedin == true) {
                $('#reset-close-btn',ctrl).text('Close');
				$('#reset-close-btn',ctrl).attr("data-dismiss","modal");
            }
		 }
		 });
		 
		 e.preventDefault();
		 return false;
 });
	
});
