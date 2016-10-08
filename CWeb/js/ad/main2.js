//获取页面加载前时间
var beforeLoad = (new Date()).getTime();

function resize(){
    var fontSize = (window.innerWidth/320)*16;
    document.body.style.fontSize = fontSize+'px';
}

// 汇报点击
try{
	clickApi = clickApi.replace(/__[A-Z_]+__/g, function(macro){
		macroName = macro.toLowerCase().replace(/__/g, '');
		return DomobLanding.core.track.get(macroName) || '';
	});

    if(clickApi){
        var tmpImg = document.createElement('IMG');
        tmpImg.src = clickApi; 
        tmpImg.width = "0";
        tmpImg.height = "0";
        document.head.appendChild(tmpImg);
    }
}catch(e){
    console.log('clk report send failure...');
}

$(document).ready(function(){
    resize();
    window.onresize = resize;

    var mySwiper =$('.swiper-container').swiper({
        speed: 200,
        mode: "horizontal",
        loop: true,
        grabCursor: true,
        centeredSlides: true,
        slidesPerView: 1,
    });
    setInterval(function(){
        mySwiper.swipeNext();
      }, 3000);
});

//获取页面加载完成时间
function getPageLoadTime() {
    var afterLoad = (new Date()).getTime();
    pageLoadSeconds = afterLoad - beforeLoad;
    DomobLanding.monitor.reg({"pageLoadSeconds":pageLoadSeconds});
    console.log(pageLoadSeconds);
} 

function downloadUCTT( ){
    //点击监测 ispost
    $.ajax({
        url: '/AD/UCTT',
        type: "POST",
        data: { "ispost": "ispost" },
        dataType: "html",
        success: function (data) {

        }
    });

    var downLoad = (new Date()).getTime();
    downloadSeconds = downLoad - beforeLoad;
    DomobLanding.monitor.clk('xiazai');
    DomobLanding.monitor.reg({"downloadSeconds":downloadSeconds}, function(){}, 'xiazai');
}



function downloadJRTT() {
    //点击监测
    $.ajax({
        url: '/AD/JRTT',
        type: "POST",
        data: { "ispost": "ispost" },
        dataType: "html",
        success: function (data) {

        }
    });

    var downLoad = (new Date()).getTime();
    downloadSeconds = downLoad - beforeLoad;
    DomobLanding.monitor.clk('xiazai');
    DomobLanding.monitor.reg({ "downloadSeconds": downloadSeconds }, function () { }, 'xiazai');
}



window.onload=getPageLoadTime;
