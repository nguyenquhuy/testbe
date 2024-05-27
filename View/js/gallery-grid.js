// layer
$(document).ready(function(){
	$('.gallery-grid .item').mouseenter(function(){
		$(this).find('.info-layer').css('animation-name','infoShow');
	});
	$('.gallery-grid .item').mouseleave(function(){
		$(this).find('.info-layer').css('animation-name','infoHide');
	});
});


// button
$(document).ready(function(){
	$('.gallery-grid .item').mouseenter(function(){
		$(this).find('.layer-item').css('animation-name','buttonShow');
	});
	$('.gallery-grid .item').mouseleave(function(){
		$(this).find('.layer-item').css('animation-name','buttonHide');
	});
});

//set background for each image
$(document).ready(function(){
	$('.gallery-grid .image').each(function(){
		$bgImg = $(this).children('img').attr('src');
		$(this).css('background-image','url('+ $bgImg +')');
	});
});


//image popup - show and hide
$(document).ready(function(){
	$('.info-layer .zoom').click(function(){
		$bgImg = $(this).parent().parent().find('.image .bg-replace').attr('src');
		$('.image-item img').attr('src',$bgImg);
		$('.image-show').css('display','block');
	});
	$('.img-popup .close-popup').click(function(){
		$('.member-img .img-popup').css('display','none');
	});
});

// close image popup when not click on it
$(document).mouseup(function(e){
    if (!$('.image-item img').is(e.target) && $('.image-item img').has(e.target).length === 0){
        $('.image-show').hide();
    }
});

