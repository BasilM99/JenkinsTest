/*!
 * Hero Carousel - jQuery Plugin
 *
 * Copyright (c) 2011 Paul Welsh
 *
 * Version: 1.3 (26/05/2011)
 *
 * Dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
 *
 *   Responsive Version (17/12/2012) by Evgeny Kostritsa and TVDA
 *   Copyright (c) 2012 TVDA.Themes
 *   http://themes.tvda.eu
 */

jQuery.fn.heroCarousel = function(options){

	options = jQuery.extend({
		animationSpeed: 600,
		navigation: true,
		easing: '',
		timeout: 5000,
		pause: true,
		pauseOnNavHover: true,
		prevText: '<i class="icon-chevron-left"></i>',
		nextText: '<i class="icon-chevron-right"></i>',
		css3pieFix: false,
		currentClass: 'current',
		onLoad: function(){},
		onStart: function(){},
		onComplete: function(){}
	}, options);

	if(jQuery.browser.msie && parseFloat(jQuery.browser.version) < 7){
		options.animationSpeed = 0;
	}

	return this.each(function() {
		var carousel = jQuery(this),
		elements = carousel.children();
		currentItem = 1;
		childWidth = elements.width();
		childHeight = elements.height();

		if(elements.length > 2){

			elements.each(function(i){
				if(options.itemClass){
					jQuery(this).addClass(options.itemClass);
				}
			});

			elements.filter(':first').addClass(options.currentClass).before(elements.filter(':last'));

			var carouselWidth = Math.round(childWidth * carousel.children().length),
			carouselMarginLeft = '-'+ Math.round(childWidth + Math.round(childWidth / 2) ) +'px'

			carousel.addClass('hero-carousel-container').css({
				'position': 'relative',
				'overflow': 'hidden',
				'left': '50%',
				'top': 0,
				'margin-left': carouselMarginLeft,
				'height': childHeight,
				'width': carouselWidth
			});
			
			carousel.before('<ul class="hero-carousel-nav"><li class="prev"><a href="#">'+options.prevText+'</a></li><li class="next"><a href="#">'+options.nextText+'</a></li></ul>');
			
			var carouselNav = carousel.prev('.hero-carousel-nav'),
			timeoutInterval;
			
			if(options.timeout > 0){
				var paused = false;
				if(options.pause){
					carousel.hover(function(){
						paused = true;
					},function(){
						paused = false;
					});
				}
				if(options.pauseOnNavHover){
					carouselNav.hover(function(){
						paused = true;
					},function(){
						paused = false;
					});
				}
				function autoSlide(){
					if(!paused && !$('.hero-carousel').hasClass('resizing'))
					{
				  		carouselNav.find('.next a').trigger('click');
				  	}
				}
				timeoutInterval = window.setInterval(autoSlide, options.timeout);
			}

			carouselNav.find('a').data('disabled', false).click(function(e){
				e.preventDefault();
				if (carouselNav.attr('data-disabled') !== 'true')
				{
    				var navItem = jQuery(this),
    				isPrevious = navItem.parent().hasClass('prev'),
    				elements = carousel.children();
    				if(navItem.data('disabled') === false){
    					options.onStart(carousel, carouselNav, elements.eq(currentItem), options);
    					if(isPrevious){
    						animateItem(elements.filter(':last'), 'previous');
    					}else{
    						animateItem(elements.filter(':first'), 'next');
    					}
    					navItem.data('disabled', true);
    					setTimeout(function(){
    						navItem.data('disabled', false);
    					}, options.animationSpeed+200);
    					if(options.timeout > 0){
    			   			window.clearInterval(timeoutInterval);
    			   			timeoutInterval = window.setInterval(autoSlide, options.timeout);
    			  		}
    			  }
				}

			});
			
			function animateItem(object,direction){
			    // custom
			    var carousel = $('.hero-carousel'),
        		    elements = carousel.children(),
        		    childWidth = elements.width(),
        		    childHeight = elements.height(),
        		    carouselWidth = Math.round(childWidth * carousel.children().length),
        		    carouselMarginLeft = '-'+ Math.round(childWidth + Math.round(childWidth / 2) ) +'px';
        		    
			    $('.hero-carousel-nav').attr('data-disabled', true);
				var carouselPosLeft = parseFloat(carousel.position().left),
				carouselPrevMarginLeft = parseFloat(carousel.css('margin-left'));
				
				if(direction === 'previous'){
					object.before( object.clone().addClass('carousel-clone') );
					carousel.prepend( object );
					var marginLeft = Math.round(carouselPrevMarginLeft - childWidth);
					var plusOrMinus = '+=';
					//custom
					var nextSlide = currentItem;
				}else{
					object.after( object.clone().addClass('carousel-clone') );
					carousel.append( object );
					var marginLeft = carouselMarginLeft;
					var plusOrMinus = '-=';
					//custom
					var nextSlide = currentItem + 1;
				}
				if(options.css3pieFix){
					fixPieClones(jQuery('.carousel-clone'));
				}
				
				
				carousel.css({
					'left': carouselPosLeft,
					'width': Math.round(carouselWidth + childWidth),
					'margin-left': marginLeft
				}).animate({'left':plusOrMinus+childWidth}, options.animationSpeed, options.easing, function(){
					carousel.css({
						'left': '50%',
						'width': carouselWidth,
						'margin-left': carouselPrevMarginLeft
					});
					jQuery('.carousel-clone').remove();
					$('.hero-carousel-nav').attr('data-disabled', false);//.removeClass('play');
					finishCarousel();
				});
				
				//custom code 
				carousel.children().removeClass(options.currentClass).eq(nextSlide).addClass(options.currentClass);
			}
			
			function fixPieClones(clonedObject){
				var itemPieId = clonedObject.attr('_pieId');
				if(itemPieId){
					clonedObject.attr('_pieId', itemPieId+'_cloned');
				}
				clonedObject.find('*[_pieId]').each(function(i, item){
					var descendantPieId = $(item).attr('_pieId');
					$(item).attr('_pieId', descendantPieId+'_cloned');
				});
			}

			function finishCarousel(){
				var elements = carousel.children();
				//elements.removeClass(options.currentClass).eq(currentItem).addClass(options.currentClass);
				options.onComplete(carousel, carousel.prev('.hero-carousel-nav'), elements.eq(currentItem), options);
			}

			if(jQuery.browser.msie){
				carouselNav.find('a').attr("hideFocus", "true");
			}

			options.onLoad(carousel, carouselNav, carousel.children().eq(currentItem), options);

		}

	});

};