var show_navigation_close_tips = getCookie('show_navigation_close_tips');
if(!show_navigation_close_tips){
	setCookie('show_navigation_close_tips','1',3600*24*365);
	$('.shrink div.shrink_tips').hide();
}
$('.shrink').toggle(
	function(){
		$(".shrink").css("top","0");
		$("body").css("background-position","0 -32px");
		$("#hd").css("display","none");
		setCookie('show_navigation_close_tips','0',3600*24*365);
		$('.shrink_tips').html("点此显示顶部栏");
	},
	function(){
		$(".shrink").css("top","32px");
		$("body").css("background-position","0 0");
		$("#hd").css("display","block");
		setCookie('show_navigation_close_tips','1',3600*24*365);
		$('.shrink_tips').html("点此隐藏顶部栏");
	}
)
if(show_navigation_close_tips == 0){
	$(".shrink").click();
}
$('.ashrink').mouseover(function(){
	$(".shrink_tips").show();
})
$('.ashrink').mouseout(function(){
	$(".shrink_tips").hide();
})

function stop_ad2(){
	if(window.ad2Timer){
		clearInterval(window.ad2Timer);
	}
}
function start_ad2(current){
	var current_pic = current;
	var all_pic = $('#pic_window a').length;
	var times = all_pic*2-1;
	var direction = 1;	//向上滑动
	$('#pic_window').css('height',all_pic*90);
	$('.pic_dot a.active').removeClass('active');
	$('.pic_dot a:eq('+(current_pic-1)+')').addClass('active');
	if(('#ad2').length>1){
		window.ad2Timer = setInterval(function(){
			var top_v = -(current_pic-1)*90;
			$('#pic_window').animate({top:top_v+'px'});
			$('.pic_dot a.active').removeClass('active');
			$('.pic_dot a:eq('+(current_pic-1)+')').addClass('active');
			current_pic += direction;
			if(current_pic == 1 || current_pic == all_pic){
				direction = -direction;
			}
		},3000);
	}
}
//游戏的顶部图片轮播
$(document).ready(function(){
	if($('#ad2').length > 0){
		start_ad2(1);
	}
	var cha;
	$('.pic_dot a').mouseover(function(){
		stop_ad2();
		$('#pic_window').animate({top:(-$(this).index()*90)+'px'});
		cha=$(this).index();
		$('.pic_dot a.active').removeClass('active');
		$(this).addClass('active');
	});
	$('.pic_dot a').mouseout(function(){       
	   if(cha==$('#pic_window a').length-1)
	   {start_ad2(1)}
	   else
	   start_ad2(cha+1);
	})
});