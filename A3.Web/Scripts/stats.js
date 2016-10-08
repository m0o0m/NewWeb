(function (){
    var require, on;
    require = function(src){
        var js = document.createElement('script'),
            head = document.getElementsByTagName('head')[0];
        js.type = 'text/javascript'; 
        js.async = true;
        head.insertBefore(js, head.firstChild);
        js.src = src;
    };
    on = function(el, type, fn) {
        el.attachEvent ? el.attachEvent('on' + type,function() {
            fn.call(el, event);
        }) : el.addEventListener(type, fn, false);
    };

    /* 1、百度
    -------------------------------------------------------------------*/
    var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
    var baiduID = 'FA316f9338e0f77fa6d9a5068b40c088c';

	document.write('<div style="display:none" id="baidu_tj">');
	document.write(
		unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F" + baiduID + "' type='text/javascript'%3E%3C/script%3E")
	);
	document.write('</div>');       
    
    /* 2、cnzz
    -------------------------------------------------------------------*/
	var cnzz_protocol = (("https:" == document.location.protocol) ? " https://" : " http://");
	var cnzzID = '1253281074';

	document.write('<div style="display:none" id="cnzz_tj">');
	document.write(
		unescape("%3Cspan id='cnzz_stat_icon_1253281074'%3E%3C/span%3E%3Cscript src='" + cnzz_protocol + "s5.cnzz.com/z_stat.php%3Fid%3D" + cnzzID + "%26show%3Dpic1' type='text/javascript'%3E%3C/script%3E")
	);
	document.write('</div>');        
    //隐藏百度图标
    //on(window, 'load', function (){
    //    var d = document.getElementById('baidu_tj');
    //    if (d) {
    //        var links = d.parentNode.getElementsByTagName('A');
    //        for (var i = links.length; i--;) {
    //            if (/tongji\.baidu/i.test(links[i].href)) {
    //                links[i].style.display = 'none';
    //            }
    //        }
    //    }
    //});

})();


