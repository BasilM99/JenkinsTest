/* Isotope Portflio Showcase
================================================== */
$(document).ready(function () {
    var $container = $('.portfolio-wrapper .items');
    $container.imagesLoaded(function () {
        $container.isotope({
            itemSelector: '.item',
            layoutMode: 'fitRows'
        });
    });

    $('.filter li a').click(function () {

        $('.filter li a').removeClass('active');
        $(this).addClass('active');

        var selector = $(this).attr('data-filter');
        $container.isotope({
            filter: selector
        });

        return false;
    });
});
/* Fancybox
================================================== */
$(document).ready(function() {
	$(".fancybox-media").fancybox({
		arrows: true,
		padding: 0,
		closeBtn: true,
		openEffect: 'fade',
		closeEffect: 'fade',
		prevEffect: 'fade',
		nextEffect: 'fade',
		helpers: {
			media: {},
			 overlay : {
                css : {
                    'background' : 'rgba(51, 51, 51, 0.8)'
                }
		    },
			buttons: false,
			thumbs: {
				width: 50,
				height: 50
			},
			title: {
				type: 'inside'
			}
		},
		beforeLoad: function() {
			var el, id = $(this.element).data('title-id');
			if (id) {
				el = $('#' + id);
				if (el.length) {
					this.title = el.html();
				}
			}
		}
	});
}); 
/* Portfolio Item Overlay
================================================== */			
$(document).ready(function() {
	$('.overlay a').prepend('<span class="more"></span>');
});
$(document).ready(function() {
	$('.overlay').mouseenter(function(e) {
		$(this).children('a').children('span').fadeIn(300);
	}).mouseleave(function(e) {
		$(this).children('a').children('span').fadeOut(200);
	});
}); 
$(document).ready(function() {
        $('.items li').mouseenter(function(e) {

            $(this).children('a').children('div').fadeIn(300);
        }).mouseleave(function(e) {

            if (!$(this).hasClass("active")) $(this).children('a').children('div').fadeOut(200);
        });
    });	

/* Portfolio Showcase
================================================== */

jQuery(document).ready(function() {
	 var $container = $('.portfolio-wrapper.showcase .items');

	 var speed = 600;
	 var header_offset = 0;
	 var scrollspeed = 600;
	 var force_scrolltotop = false;


	 ///////////////////////////
	// GET THE URL PARAMETER //
	///////////////////////////
	function getUrlVars(hashdivider)
			{
				
				try { var vars = [], hash;
					var hashes = window.location.href.slice(window.location.href.indexOf(hashdivider) + 1).split('_');
					for(var i = 0; i < hashes.length; i++)
					{
						hashes[i] = hashes[i].replace('%3D',"=");
						hash = hashes[i].split('=');
						vars.push(hash[0]);
						vars[hash[0]] = hash[1];
					}
					return vars;
				} catch(e) { }
				
			}


	////////////////////////
	// GET THE BASIC URL  //
	///////////////////////
    function getAbsolutePath() {
		    var loc = window.location;
		    var pathName = loc.pathname.substring(0, loc.pathname.lastIndexOf('/') + 1);
		    return loc.href.substring(0, loc.href.length - ((loc.pathname + loc.search + loc.hash).length - pathName.length));
    }

    //////////////////////////
	// SET THE URL PARAMETER //
	///////////////////////////
    function updateURLParameter(paramVal){
    		var yScroll=document.body.scrollTop;


	    	var baseurl = window.location.pathname.split("#")[0];
	    	var url = baseurl.split("#")[0];
	    	if (paramVal==undefined) paramVal="";

	    	if (paramVal.length==0)
		    	par="#"
	   		else
 		  		par='#'+paramVal;

		    //window.location.replace(url+par);

		    if(history.pushState) {
			    history.pushState(null, null, par);
			}
			else {
			    location.hash = par;
			}

	}


	 var deeplink = getUrlVars("#");
	 var ie = !$.support.opacity;
	 var ie9 = (document.documentMode == 9);

	 $container.find('.item').click(function() {

		 	// The CLicked Thumb
		 	var thumb = jQuery(this);



		 	// IF THE CLICKED THUMB IS ALREADY SELECTED, WE NEED TO CLOSE THE WINDOWS SIMPLE
		 	if (thumb.hasClass("active")) {
			 	thumb.removeClass("active");

		 		closeDetailView($container);

			// OTHER WAY WE CLOSE THE WINDOW (IF NECESsARY, OPEN AGAIN, AND DESELET / SELECT THE RIGHT THUMBS
		 	}  else {
		 		updateURLParameter("pro-"+thumb.index());
			 	thumb.addClass("latest-active");
			 	removeActiveThumbs($container);

			 	thumb.removeClass("latest-active");
			 	thumb.addClass("active");

			 	// CHECK IF WE ALREADY HAVE THE DETAIL WINDOW OPEN
			 	 var pdv = jQuery('body').find('.portfolio-detail-view');
				 
			 	if (pdv.length) {
			 		var fade=false;
			 		clearInterval(pdv.data('interval'));
			 		pdv.addClass("portfolio-detail-view-remove").removeClass('portfolio-detail-view');
				 	pdv.animate({'height':'0px','opacity':'0'},{duration:speed, complete:function() { jQuery(this).remove();}});

				 	var delay=speed+50;
				 	//if (thumb.position().top<pdv.position().top) {
			 		// 	delay=0;
				 	//} else {
				 	   moveThumbs($container,pdv.data('itemstomove'),0);
			 		   setTimeout(function() {$container.isotope( 'reLayout');},speed+50);
			 		// }

				 	setTimeout(function() {
				 			jQuery('body,html').animate({
                                scrollTop: thumb.offset().top-(header_offset+10)
                            }, {
                                duration: scrollspeed,
                                queue: false
                            });
                             if (force_scrolltotop) {

	                             openDetailView($container,thumb,fade);
			                    } else {
				                    setTimeout(function () {
									 	openDetailView($container,thumb,fade);
				                    },scrollspeed)
			                    }


				 	},delay)

			 	} else {

				 	jQuery('body,html').animate({
                                scrollTop: thumb.offset().top-(header_offset+10)
                            }, {
                                duration: scrollspeed,
                                queue: false
                            });
                    if (force_scrolltotop) {
						 	openDetailView($container,thumb);
                    } else {
	                    setTimeout(function () {
						 	openDetailView($container,thumb);
	                    },scrollspeed)
                    }




			 	}
			}
			return false;
	 }) // END OF CLICK ON PORTFOLIO ITEM

	 // DEEPLINK START IF NECESSARY
		 if (deeplink[0].split('pro-').length>1) {
		 	var thmb = parseInt(deeplink[0].split('pro-')[1],0)+1;
			 $container.find('.item:nth-child('+thmb+')').click();
			 $container.find('.item:nth-child('+thmb+')').addClass("active").children('a').children('div').fadeIn(300);;
			 
		}



	 // CLICK ON FILTER SHOULD CLOSE THE DETAIL PAGE ALSO
	 jQuery('.portfolio-wrapper.showcase .filter').find('li a').each(function() {
		 jQuery(this).click(function() {
			
			closeDetailView($container)
		 })
	 })

	 // ON RESIZE REMOVE THE DETAIL VIEW CONTAINER
	 
	 if (!ie) {
		 jQuery(window).bind('resize',function()  {		
			if (!isiPhone()) {
				closeDetailView($container);
				centerpdv($container);
				
			}
		 });
	} else {
		
		$container.isotope( 'option', {resizable:false});
	}

	 //  CHECK IPHONE
	// Return boolean TRUE/FALSE
		function isiPhone(){
			return (
				(navigator.platform.indexOf("iPhone") != -1) ||
				(navigator.platform.indexOf("iPod") != -1) ||
    			(navigator.platform.indexOf("iPad") != -1)
			);
		}
		
		
	 // REMOVE ACTIVE THUMB EFFECTS
	 function removeActiveThumbs($container) {
		 	$container.find('.item').each(function() {
					jQuery(this).removeClass('active');
					if (!jQuery(this).hasClass('latest-active')) jQuery(this).children('a').children('div').fadeOut(200);

			 	});
	 }

	 // CLOSE DETAILVIEW
	 function closeDetailView($container) {
		
		 var pdv = jQuery('body').find('.portfolio-detail-view');
	 	 setTimeout(function() {
			 if (pdv.length) {
			 		removeActiveThumbs($container);
			 		clearInterval(pdv.data('interval'));
				 	pdv.animate({'height':'0px','opacity':'0'},{duration:speed, complete:function() { jQuery(this).remove();}});
				 	moveThumbs($container,pdv.data('itemstomove'),0);
			}
			setTimeout(function() {
					$container.isotope( 'reLayout',function() { 
						$container.data('height',$container.height());								
						setTimeout(function() {
							
							$container.data('height',$container.height());
						},speed);  //500 old value
					});
			},speed+50);
			if (!ie && !ie9) updateURLParameter("");
			 	
		},150)
	 }

	 function centerpdv($container) {
		try {
			var pdv = jQuery('body').find('.portfolio-detail-view');
			var pleft=jQuery('body').width()/2 - pdv.width()/2;
			
			pdv.css({'left':pleft+"px"});
			
		} catch(e) { }
	 }
	 
	 
	 // OPEN THE DETAILVEW AND CATCH THE THUMBS BEHIND THE CURRENT THUMB
	 function openDetailView($container,thumb,fadeit) {

		 	// The Top Position of the Current Item.
		 	currentTop= thumb.position().top;
		 	thumbOffsetTop= thumb.offset().top;

			 // ALL ITEM WE NEED TO MOVE SOMEWHERE
		 	var itemstomove =[];

		 	$container.find('.item').each(function() {
			 	var curitem = jQuery(this);
			 	if (curitem.position().top>currentTop) itemstomove.push(curitem);

		 	})

		 	// Reset CurrentPositions

		 	jQuery.each(itemstomove,function() {
			 	var thumb = jQuery(this);
			 	thumb.data('oldPos',thumb.position().top);

		 	})

		 	// We Save the Height Of the current Container here.
		 	if ($container.data('height')!=undefined) {
			 	if ($container.height()<$container.data('height')) 	$container.data('height',$container.height());
		 	} else {
			 	$container.data('height',$container.height());
			 }

		 	// ADD THE NEW CONTENT IN THE DETAIL VIEW WINDOW.
		 	jQuery('body').append(
				'<div class="portfolio-detail-view">'+
		 		'<div class="inner">'+
		 		'<div class="portfolio-detail-content-container">'+
		 		thumb.data('detailcontent')+
		 		'</div>'+
		 		'</div>'+
		 		'<div class="closebutton"></div>'+
		 		'</div>');
		
		 	// CATCH THE DETAIL VIEW AND CONTENT CONTAINER
		 	var pdv = jQuery('body').find('.portfolio-detail-view');
		 	var closeb = pdv.find('.closebutton');
		 	var pdcc = pdv.find('.portfolio-detail-content-container');
		 	var pdvpad = parseInt(pdcc.css('paddingBottom'),0) + parseInt(pdcc.css('paddingTop'),0);
			
		 	var offset = pdcc.height()+pdvpad + parseInt(pdv.css('marginBottom'),0);

	
		 	closeb.click(function() {
				
		 			closeDetailView($container);
		 	})

		 	// ANIMATE THE OPENING OF THE CONTENT CONTAINER

			pdv.animate({'height':(pdcc.outerHeight(true)+pdvpad)+"px"},{duration:speed,queue:false});


		 	// SAVE THE ITEMS TO MOVE IN THE PDV
		 	pdv.data('itemstomove',itemstomove);

			
			
		 	//PUT THE CONTAINER IN THE RIGHT POSITION
		 	pdv.css({'top':(thumbOffsetTop+thumb.height()+parseInt(thumb.css('marginBottom'),0))+"px"});
			
			centerpdv($container);
			
			// FIRE THE CALLBACK HERE
			try{
				var callback = new Function(thumb.data('callback'));
				
				callback();
			} catch(e) {}
							
			//INITIALISE THE CAROUSEL HERE			
			pdv.find('.carousel').each(function() {

				jQuery(this).carousel({
					interval: 2000
				})
			});

			

			jQuery.each(itemstomove,function() {
				var thumb = jQuery(this);
				if (ie ||ie9) 
					thumb.data('top',parseInt(thumb.position().top,0));
				else
					thumb.data('top',0);
			});
		 	// MOVE THE REST OF THE THUMBNAILS
		 	moveThumbs($container,itemstomove,offset);

		 	pdv.data('interval',setInterval(function() {
			 	var pdv = jQuery('body').find('.portfolio-detail-view');
			 	var closeb = pdv.find('.closebutton');
			 	var pdcc = pdv.find('.portfolio-detail-content-container');
			 	var pdvpad =  parseInt(pdcc.css('paddingBottom'),0) + parseInt(pdcc.css('paddingTop'),0);
			 	var offset = pdcc.height()+pdvpad + parseInt(pdv.css('marginBottom'),0);
			 	moveThumbs($container,itemstomove,offset);
			 	pdv.animate({'height':Math.round(pdcc.height()+pdvpad)+"px"},{duration:speed,queue:false});
				
		 	},100));

	 }

	 // MOVE THE THUMBS
	 function moveThumbs($container,itemstomove,offset) {
		 
			jQuery.each(itemstomove,function() {
			 	var thumb = jQuery(this);
			 	thumb.stop(true);				
			 	thumb.animate({'top':(thumb.data('top')+offset)+"px"},{duration:speed,queue:false});				
		 	})
			
			
			if (ie || ie9) {
				$container.stop(true);
				$container.animate({'height':($container.data('height')+offset)+"px"}, {duration:speed,queue:false});
			} else {				
				$container.css({'height':Math.round($container.data('height')+offset)+"px"});
			}
	 }
});
