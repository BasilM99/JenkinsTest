$(document).ready(function($) {

	$('body').addClass('bg-cover');

	// Color Selector
	$(".color-green" ).click(function()			{ $("#colors" ).attr("href", "css/colors/color-green.css" ); });
	$(".color-orange" ).click(function()		{ $("#colors" ).attr("href", "css/colors/color-orange.css" ); });
	$(".color-blue" ).click(function()			{ $("#colors" ).attr("href", "css/colors/color-blue.css" );	});
	$(".color-red" ).click(function()			{ $("#colors" ).attr("href", "css/colors/color-red.css" ); });
	$(".color-cream" ).click(function()			{ $("#colors" ).attr("href", "css/colors/color-cream.css" ); });
	$(".color-lightgreen" ).click(function()	{ $("#colors" ).attr("href", "css/colors/color-lightgreen.css" ); });
	$(".color-yellow" ).click(function()		{ $("#colors" ).attr("href", "css/colors/color-yellow.css" ); });
	$(".color-cyan" ).click(function()	    	{ $("#colors" ).attr("href", "css/colors/color-cyan.css" ); });	
	$(".color-pink" ).click(function()			{ $("#colors" ).attr("href", "css/colors/color-pink.css" ); });	
	$(".color-tan" ).click(function()			{ $("#colors" ).attr("href", "css/colors/color-tan.css" ); });
	

	// Layout Selector
	$(".boxed" ).click(function(){
		$("#layout" ).attr("href", "css/wide.css" );
		$(window).resize();
	});

	$("#layout-selector").on('change', function() {
		$('#layout').attr('href', $(this).val() + '.css');
		var x, divArray = ["bg-image", "bg-pattern"];
		$(window).resize();
	    /*for (x in divArray) {
	        if (x) {
	            var display = document.geTelementById(divArray[x]).style.display;
	            if (display == "none")
	                document.geTelementById(divArray[x]).style.display="block";
	            else
	                document.geTelementById(divArray[x]).style.display="none";
	        }
	    }*/
        google.maps.event.trigger(map, 'resize');
 		map.setCenter(new google.maps.LatLng(34.021453,-118.785027));
	});;


	// Style Selector	
	$('#style-selector').animate({
		left: '-212px'
	});

	$('#style-selector a.close').click(function(e){
		e.preventDefault();
		var div = $('#style-selector');
		if (div.css('left') === '-212px') {
			$('#style-selector').animate({
				left: '0'
			});
			$(this).removeClass('icon-chevron-right');
			$(this).addClass('icon-chevron-left');
		} else {
			$('#style-selector').animate({
				left: '-212px'
			});
			$(this).removeClass('icon-chevron-left');
			$(this).addClass('icon-chevron-right');
		}
	})

	$('#color-styles a').click(function(e){
		e.preventDefault();
		
		$(this).parent().parent().find('a').removeClass('active');
		$(this).addClass('active');
	})

	$('#bg-image a').click(function(e){
		e.preventDefault();
		var selectedLayout;
		selectedLayout = $("#layout-selector").val();
		if(selectedLayout == 'css/wide') {
			alert("Select boxed layout");
			return false;
		}
		
		$(this).parent().parent().find('a').removeClass('active');
		$('#bg-pattern a').parent().parent().find('a').removeClass('active');
		$(this).addClass('active');
		var bg = $(this).css('backgroundImage');
		$('body').addClass('bg-cover');
		$('body').css('backgroundImage',bg);
	})

	$('#bg-pattern a').click(function(e){
		e.preventDefault();
		
		var selectedLayout;
		selectedLayout = $("#layout-selector").val();
		if(selectedLayout == 'css/wide') {
			alert("Select boxed layout");
			return false;
		}
		$(this).parent().parent().find('a').removeClass('active');
		$('#bg-image a').parent().parent().find('a').removeClass('active');
		$(this).addClass('active');
		var bg = $(this).css('backgroundImage');
		$('body').removeClass('bg-cover');
		$('body').css('backgroundImage',bg);
	})

});
