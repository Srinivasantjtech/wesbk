$(document).ready(function(){
			
	/* ---- */
	/* FORM */
	/* ---- */
		
	$('.dropdown_filter')
		.click( 
			function() {
				$(this).children('ul').slideToggle(150);
				
				if ($(this).hasClass('open')) {
					$(this).removeClass('open');
					return false;
				} else {
					$(this).addClass('open');
					return false;
				}
				
				return false;
			})
			
		.hover( 
			function() {
			
			},
			function() {
				$(this).children('ul').slideUp(150);
				$(this).removeClass('open');
			});
		
	$('.dropdown_filter ul li')
		.click( 
			function() {
				var sitem = $(this).html();
				var sid = $(this).attr('id');
				
				$(this).siblings('li').removeClass('selected');
				$(this).addClass('selected');
				$(this).parent('ul').siblings('span.selected').html(sitem);
				$(this).parent('ul').siblings('input').val(sid);
			});
			
			/* End Dropdown-Filter ---*/
			
			
			$('.brand_modal')
		.click( 
			function() {
				$(this).children('ul').slideToggle(150);
				
				if ($(this).hasClass('open')) {
					$(this).removeClass('open');
					return false;
				} else {
					$(this).addClass('open');
					return false;
				}
				
				return false;
			})
			
		.hover( 
			function() {
			
			},
			function() {
				$(this).children('ul').slideUp(150);
				$(this).removeClass('open');
			});
		
	$('.brand_modal ul li')
		.click( 
			function() {
				var sitem = $(this).html();
				var sid = $(this).attr('id');
				
				$(this).siblings('li').removeClass('selected');
				$(this).addClass('selected');
				$(this).parent('ul').siblings('span.selected').html(sitem);
				$(this).parent('ul').siblings('input').val(sid);
			});

});
/* end document ready */
	


