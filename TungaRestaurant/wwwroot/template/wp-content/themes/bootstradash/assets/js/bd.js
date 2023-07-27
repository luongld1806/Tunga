jQuery(window).scroll(function() {    
	var scroll = jQuery(window).scrollTop();
	if(jQuery(window).width() > 991){
		if (scroll >= 500) {
			//jQuery(".float-panel").removeClass("moved-up");
			jQuery(".float-panel").addClass("nav-shadow");
			jQuery(".offer-banner").addClass("fixed-top");
		}
		else{
			
			jQuery(".float-panel").removeClass("nav-shadow");
			jQuery(".offer-banner").removeClass("fixed-top");
		}
	}
});
function closebanner(){
	 //jQuery(".contest-banner").removeClass("d-flex");
	 jQuery(".navtop-banner").remove();
	 Cookies.set("topnav_bar_closed", 1, { expires : 1 });
	}
	jQuery('.showloginpopup').click(function(){
		jQuery('#loginpopup').modal('show')	;	

	});
	jQuery('.mb-buy-now,.purchase-now').on('click', function(event) {
		var target = jQuery(this.getAttribute('href'));
		if( target.length ) {
			event.preventDefault();
			jQuery('html, body').stop().animate({
				scrollTop: target.offset().top - 100
			}, 0);
		}
	});
	jQuery(document).ready(function(){
		if(jQuery('.new-template-wrapper').length>0){ 
			
				jQuery(document).ready(checkreview);
			}

	});

	jQuery(document).ready(function(){
		if(jQuery('#tab-title-reviews').length!=0){ 
			if(jQuery('#tab-title-reviews').hasClass('active')){

				jQuery(document).ready(checkContainer);
			}

		}
	
		/*if(jQuery('.new-template-wrapper').length>0){ 
			
				jQuery(document).ready(checkContainer);
		}*/

		jQuery("li[role='tab']").click(function(){
			jQuery(this).tab('show');
		});

		jQuery('li[role="tab"]').on('shown.bs.tab', function (e) {
	  	var target = jQuery(e.target).attr("id") ;// activated tab
	  	if(target=='tab-title-reviews'){
	  		jQuery(document).ready(checkContainer);
	  	}

	  }); 
		if(jQuery(window).width() > 991){
			var $sticky = jQuery('.sticky-sidebar-ad');
			var $stickyrStopper = jQuery('.mastfoot');
			if (!!$sticky.offset()) { 
				var parentw=jQuery('.sticky-sidebar-ad').parent().width();
				var generalSidebarHeight = $sticky.innerHeight();
				var stickyTop = $sticky.offset().top;
				var stickOffset = 120;
				var stickyStopperPosition = $stickyrStopper.offset().top;
				var stopPoint = stickyStopperPosition - generalSidebarHeight - stickOffset - 50;
				var diff = stopPoint + stickOffset;
				var output='';
				output=output+'generalSidebarHeight:'+generalSidebarHeight+'<br>';
				output=output+'stickyTop:'+stickyTop+'<br>';
				output=output+'stickyStopperPosition:'+stickyStopperPosition+'<br>';
				output=output+'stopPoint:'+stopPoint+'<br>';
				output=output+'diff:'+diff+'<br>';
 			//jQuery('.sticky-sidebar-ad .meta').html(output);
 			jQuery(window).scroll(function(){ 
 				var windowTop = jQuery(window).scrollTop(); 
 				if (stopPoint < windowTop) {
 					$sticky.css({ position: 'absolute',bottom:'0',top:'auto',width:parentw});
 				} else if (stickyTop < windowTop+stickOffset) {
 					topinpx=stickOffset+'px';
 					$sticky.css({ position: 'fixed', top: topinpx ,width:parentw});
 				} else {
 					$sticky.css({position: 'absolute', top: 'initial',bottom:'auto',width:parentw});
 				}
 				//jQuery('.sticky-sidebar-ad p.scroll').html(windowTop);
 			});

 		}
 	}

 });
	function checkContainer() {
		if(jQuery('.bd-recent-reviews').is(':visible')){  
			var h1 = jQuery('#comments ol').height();
			var h2=jQuery('.bd-recent-reviews .reviews').height();

			if(h1>500){
				jQuery('#comments ol').css('height','500px');

				jQuery('#comments ol').addClass('lesss');
				jQuery('#comments').append('<div class="more_less_toggle"><a href="#" id="morelink_comments" class="mb-0 less">Show more</a></div>');

			}
			if(h2>600){
				jQuery('.bd-recent-reviews .reviews').css('height','600px');

				jQuery('.bd-recent-reviews .reviews').addClass('lesss');
				jQuery('.bd-recent-reviews').append('<div class="more_less_toggle"><a href="#" id="morelink1_comments" class="mb-0 less">Show more</a></div>');
			}
		} 
		else {
			setTimeout(checkContainer, 50); 
		}
		
		/*if(jQuery('.new-template-wrapper').is(':visible')){  
			
			var h1 = jQuery('.review-content-wrap').height();
			var h2=jQuery('.changelog-wrapper').height();

			if(h1>500){
				jQuery('.review-content-wrap').css('height','500px');

				jQuery('.review-content-wrap').addClass('lesss');
				jQuery('.review-content-wrap').append('<div class="more_less_toggle"><a href="#" id="review_pro" class="mb-0 less">Show more</a></div>');

			}
			if(h2>600){
				jQuery('.changelog-wrapper').css('height','600px');

				jQuery('.changelog-wrapper').addClass('lesss');
				jQuery('.changelog-wrapper').append('<div class="more_less_toggle"><a href="#" id="change_log" class="mb-0 less">Show more</a></div>');
			}


		}
		else {
			setTimeout(checkContainer, 50); 
		}*/
	}
	function checkreview() {
		if(jQuery('.new-template-wrapper').is(':visible')){  
			
			var h1 = jQuery('.changelog-wrapper').height();
			var h2=jQuery('.review-content-wrap').height();

			if(h1>500){
				jQuery('.changelog-wrapper').css('height','907px');

				jQuery('.changelog-wrapper').addClass('lesss');
				jQuery('.changelog-wrapper').append('<div class="more_less_toggle"><a href="#" id="change_log" class="mb-0 less">Show more</a></div>');

			}
			if(h2>600){
				jQuery('.review-content-wrap').css('height','445px');

				jQuery('.review-content-wrap').addClass('lesss');
				jQuery('.review-content-wrap').append('<div class="more_less_toggle"><a href="#" id="review_pro" class="mb-0 less">Show more</a></div>');
			}


		}
		else {
			setTimeout(checkreview, 50); 
		}

	}
	jQuery('.bd-recent-reviews').on("click","#morelink1_comments",function(e){
		e.preventDefault();
		if(jQuery(this).hasClass('less')){
			jQuery(this).removeClass('less');
			jQuery('.bd-recent-reviews .reviews').removeClass('lesss');
			jQuery('.bd-recent-reviews .reviews').css('height','100%'); 
			jQuery(this).text('Show less'); 

		}
		else{
			jQuery(this).addClass('less');
			jQuery('.bd-recent-reviews .reviews').addClass('lesss');
			jQuery('.bd-recent-reviews .reviews').css('height','600px');
			jQuery(this).text('Show more'); 
			jQuery('html, body').animate({
				scrollTop: jQuery(this).offset().top - 200
			}	, 0);
		}
	});

	jQuery("#comments").on("click", "a#morelink_comments", function(e){
		e.preventDefault();
		if(jQuery('#morelink_comments').hasClass('less')){
			jQuery('#morelink_comments').removeClass('less');
			jQuery('#comments ol').removeClass('lesss');
			jQuery('#comments ol').css('height','100%'); 
			jQuery('a#morelink_comments').text('Show less'); 


		}
		else{
			jQuery('#morelink_comments').addClass('less');
			jQuery('#comments ol').addClass('lesss');
			jQuery('#comments ol').css('height','500px');
			jQuery('a#morelink_comments').text('Show more');
			jQuery('html, body').animate({
				scrollTop: jQuery("a#morelink_comments").offset().top-100
			}, 0);
		}
	});

	jQuery('.changelog-wrapper').on("click","#change_log",function(e){
		e.preventDefault();
		if(jQuery(this).hasClass('less')){
			jQuery(this).removeClass('less');
			jQuery('.changelog-wrapper').removeClass('lesss');
			jQuery('.changelog-wrapper').css('height','100%'); 
			jQuery(this).text('Show less'); 

		}
		else{
			jQuery(this).addClass('less');
			jQuery('.changelog-wrapper').addClass('lesss');
			jQuery('.changelog-wrapper').css('height','600px');
			jQuery(this).text('Show more'); 
			jQuery('html, body').animate({
				scrollTop: jQuery(this).offset().top - 200
			}	, 0);
		}
	});

	jQuery(".review-content-wrap").on("click", "a#review_pro", function(e){
		e.preventDefault();
		if(jQuery('#review_pro').hasClass('less')){
			jQuery('#review_pro').removeClass('less');
			jQuery('.review-content-wrap').removeClass('lesss');
			jQuery('.review-content-wrap').css('height','100%'); 
			jQuery('a#review_pro').text('Show less'); 


		}
		else{
			jQuery('#review_pro').addClass('less');
			jQuery('.review-content-wrap').addClass('lesss');
			jQuery('.review-content-wrap').css('height','500px');
			jQuery('a#review_pro').text('Show more');
			jQuery('html, body').animate({
				scrollTop: jQuery("a#review_pro").offset().top-100
			}, 0);
		}
	});


	jQuery(function() {
		jQuery('#show-ma-register').click(function(e) {
			e.preventDefault();
			jQuery('#bd-ma-login').hide();
			jQuery('#bd-ma-register').show();

		});
		jQuery('#show-ma-login').click(function(e) {
			e.preventDefault();
			jQuery('#bd-ma-register').hide();
			jQuery('#bd-ma-login').show();
		});
	});
	jQuery( function( $ ) {

	// wc_single_product_params is required to continue.
	if ( typeof wc_single_product_params === 'undefined' ) {
		///return false;
	}

	$( 'body' )
	.on( 'init', '#rating', function() {
		$( '#rating' )
		.hide()
		.before(
			'<p class="stars">\
			<span>\
			<a class="star-1" href="#">1</a>\
			<a class="star-2" href="#">2</a>\
			<a class="star-3" href="#">3</a>\
			<a class="star-4" href="#">4</a>\
			<a class="star-5" href="#">5</a>\
			</span>\
			</p>'
			);
	} )
	.on( 'click', '#respond p.stars a', function() {
		var $star   	= $( this ),
		$rating 	= $( this ).closest( '#respond' ).find( '#rating' ),
		$container 	= $( this ).closest( '.stars' );

		$rating.val( $star.text() );
		$star.siblings( 'a' ).removeClass( 'active' );
		$star.addClass( 'active' );
		$container.addClass( 'selected' );

		return false;
	} )
	.on( 'click', '#respond #submit', function() {
		var $rating = $( this ).closest( '#respond' ).find( '#rating' ),
		rating  = $rating.val();
		if($('#review_form p.stars').hasClass('selected')){
			$('#review_form p.stars').removeClass('focus');
		}
		else{
			$('#review_form p.stars').addClass('focus');
			$('#review_form #error-msg').text('Rating field cann not be empty!');
			return false;
		}

		if ( $rating.length > 0 && ! rating && wc_single_product_params.review_rating_required === 'yes' ) {
			window.alert( wc_single_product_params.i18n_required_rating_text );

			return false;
		}
	} );
} );

	function carouselNormalization() {
  var items = jQuery('.p-testimonials-section .carousel-item'), //grab all slides
    heights = [], //create empty array to store height values
    tallest; //create variable to make note of the tallest slide

    if (items.length) {
    	function normalizeHeights() {
      items.each(function() { //add heights to array
      	heights.push(jQuery(this).height());
      });
      tallest = Math.max.apply(null, heights); //cache largest value
      items.each(function() {
      	jQuery(this).css('min-height', tallest + 'px');
      });
  };
  normalizeHeights();

  jQuery(window).on('resize orientationchange', function() {
      tallest = 0, heights.length = 0; //reset vars
      items.each(function() {
        jQuery(this).css('min-height', '0'); //reset min-height
    });
      normalizeHeights(); //run it again 
  });
}
}

/**
 * Wait until all the assets have been loaded so a maximum height 
 * can be calculated correctly.
 */
 window.onload = function() {
 	carouselNormalization();
 }
 jQuery(document).click(function(e) 
 {
 	var container = jQuery("body.postid-11108 header nav .show");

    // if the target of the click isn't the container nor a descendant of the container
    if (!container.is(e.target) && container.has(e.target).length === 0) 
    {
    	container.removeClass('show');
    }
});
 jQuery(document).ready(function(){
 	jQuery("li.menu-item-has-children").click(function(){
 		if(jQuery(window).width() < 992){
 			jQuery(".sub-menu",this).toggle();
 		}
 	});
 });
 jQuery(document).ready( function () {

 	jQuery('#mc-embedded-subscribe-form button[type="submit"]').bind('click', function ( event ) {
 		event.preventDefault();
 		var form = jQuery('#mc-embedded-subscribe-form');
 		var emaili=jQuery('#mc-embedded-subscribe-form #mce-EMAIL').val();
 		if(emaili==''){
 			jQuery('#subscribeMsg').html("The field can not be empty!");
 			return false;
 		}
 		datah = { EMAIL: emaili };

 		jQuery.ajax({
 			type: form.attr('method'),
 			url: form.attr('action'),
 			data: datah,
 			dataType: 'jsonp', 
 			crossDomain: true,
 			beforeSend: function() { 
 				jQuery('#subscribeMsg').html("Please wait...");
 			},
 			error       : function(err) { 
 				jQuery('#subscribeMsg').html("Could not connect to the registration server. Please try again later."); 
 				console.log(err);
 			},
 			success     : function(data) {
 				jQuery('#subscribeMsg').html(data.msg);
 				console.log(data);

 			}
 		});
 	});

 });
  var acc = document.getElementsByClassName("accordion-1");
var i;

for (i = 0; i < acc.length; i++) {
  acc[i].addEventListener("click", function() {
    this.classList.toggle("active-1");
    var panel1 = this.nextElementSibling;
    if (panel1.style.maxHeight) {
      panel1.style.maxHeight = null;
    } else {
      panel1.style.maxHeight = panel1.scrollHeight + "px";
    } 
  });
}