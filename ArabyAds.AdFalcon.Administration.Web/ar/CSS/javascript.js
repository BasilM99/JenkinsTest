// JavaScript Document

var animationLength = 500;
var betweenAnimationLength = 8000;
var sumWait = 0;

var currentSelection;

	
function startAnimation()
{
	
	var banner1 = jQuery("#banner1");
	var banner2 = jQuery("#banner2");
	var banner3 = jQuery("#banner3");
	var banner4 = jQuery("#banner4");
	
	currentSelection = banner1;
	currentLinkSelection = '#lnkBanner1';
	
	banner1.delay(betweenAnimationLength).fadeOut(animationLength, function(){
		banner2.fadeIn(animationLength, function(){
			jQuery('#lnkBanner1').removeClass('selected');
			jQuery('#lnkBanner2').addClass('selected');
			currentLinkSelection = '#lnkBanner2';
			currentSelection = banner2;
			
			banner2.delay(betweenAnimationLength).fadeOut(animationLength, function(){
				banner3.fadeIn(animationLength, function(){
					jQuery('#lnkBanner2').toggleClass('selected');
					jQuery('#lnkBanner3').toggleClass('selected');
					currentSelection = banner3;
					currentLinkSelection = '#lnkBanner3';
					
					banner3.delay(betweenAnimationLength).fadeOut(animationLength, function(){
						banner4.fadeIn(animationLength, function(){
							jQuery('#lnkBanner3').toggleClass('selected');
							jQuery('#lnkBanner4').toggleClass('selected');
							currentSelection = banner4;
							currentLinkSelection = '#lnkBanner4';
							
							banner4.delay(betweenAnimationLength).fadeOut(animationLength, function(){
						banner1.fadeIn(animationLength, function(){
							jQuery('#lnkBanner4').toggleClass('selected');
							jQuery('#lnkBanner1').toggleClass('selected');
							currentSelection = banner4;
							currentLinkSelection = '#lnkBanner1';							
							
							startAnimation();
						}
						
					);
					
							});
						});
					
				});
				
			});
			
		});
		
		
	});	});
	
}

function switchSlides(switchedToBanner , switchedToLink)
{
	
		currentSelection.fadeOut(animationLength, function(){
					switchedToBanner.fadeIn(animationLength, function(){
						jQuery(switchedToLink).toggleClass('selected');
						jQuery(currentLinkSelection).toggleClass('selected');
						currentSelection = switchedToBanner;
						currentLinkSelection = switchedToLink;
					});
		});
	
}


function stopActions()
{
	jQuery('#banner1').stop(true, true);
	jQuery('#banner2').stop(true, true);
	jQuery('#banner3').stop(true, true);
	jQuery('#banner4').stop(true, true);
}