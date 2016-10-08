<?php
$userid = "";
if (isset($_GET['via'])){
    $userid = $_GET['via'];
    $ziArr = array('A'=>0,'B'=>1,'C'=>2,'D'=>3,'E'=>4,'F'=>5,'G'=>6,'H'=>7,'I'=>8,'J'=>9);
    $temArr = str_split(strtoupper(trim($userid)),1);
    $ziStr='';
    for ($i = 0; $i < count($temArr); $i++) {
        if(array_key_exists($temArr[$i],$ziArr))
        $ziStr = $ziStr . $ziArr[$temArr[$i]];
    }
    $mysqli = new \mysqli('10.66.159.153:3306', 'jx3role', 'eN8Hbc6mPvTn3)Z.', '515game');

    // Output any connection error
    if ($mysqli->connect_error) {
        die('Error : (' . $mysqli->connect_errno . ') ' . $mysqli->connect_error);
    }

    // MySqli Select Query
    $results = $mysqli->query('replace INTO Unrelated_QQWeb(UserID,IP,State) VALUES(' . $ziStr . ',"' . get_client_ip(0,true) . '",1) ;');
    if ($results) {
        // http://rc.qzone.qq.com/1104953359?via=
    } else {}

    // close connection
    $mysqli->close();
}
$appid = "1054312856";
$html=<<<HTML
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta charset="utf-8">
<meta content="IE=edge,chrome=1" http-equiv="X-UA-Compatible">
<meta content="" name="description">
<meta content="" name="keywords">
<meta name="viewport" content="width=device-width"/>  
<script type="text/javascript">
if(window.location.toString().indexOf('pref=padindex') != -1){
	window.location.href="http://www.515.com/download/poker515.apk";
}else{
	if(/AppleWebKit.*Mobile/i.test(navigator.userAgent) || (/MIDP|SymbianOS|NOKIA|SAMSUNG|LG|NEC|TCL|Alcatel|BIRD|DBTEL|Dopod|PHILIPS|HAIER|LENOVO|MOT-|Nokia|SonyEricsson|SIE-|Amoi|ZTE/.test(navigator.userAgent))){  
		try{
			if(/iPhone|iPod|iPad/i.test(navigator.userAgent)){
				if(/MicroMessenger/i.test(navigator.userAgent)==false){
					window.location.href="https://itunes.apple.com/cn/app/id{$appid}";
				}
			}else{
				if(/MicroMessenger/i.test(navigator.userAgent)==false){
					window.location.href="http://www.515.com/download/poker515.apk";
				}
            }
		}catch(e){}
	}else{
		window.location.href="http://rc.qzone.qq.com/1104953359/?via={$userid}";
	}
}
</script>
<title>一起来打德州扑克吧，海量奖品等你拿，全球亿万网民都在热战！</title>
<style type="text/css" media="all">
            html {
                -ms-touch-action: none; /* Direct all pointer events to JavaScript code. */
            }
                html, body, div, span, applet, object, iframe,
                h1, h2, h3, h4, h5, h6, p, blockquote, pre,
                a, abbr, acronym, address, big, cite, code,
                del, dfn, em, font, img, ins, kbd, q, s, samp,
                small, strike, strong, sub, sup, tt, var,
                b, u, i, center,
                dl, dt, dd, ol, ul, li,
                fieldset, form, label, legend,
                table, caption, tbody, tfoot, thead, tr, th, td {
                    margin: 0;
                    padding: 0;
                    border: 0;
                    outline: 0;
                    font-size: 100%;
                    vertical-align: baseline;
                    background: transparent;
                }
                body {
                    line-height: 1;
                }
                ol, ul {
                    list-style: none;
                }
                blockquote, q {
                    quotes: none;
                }
                blockquote:before, blockquote:after,
                q:before, q:after {
                    content: '';
                    content: none;
                }
                /* remember to define focus styles! */
                :focus {
                    outline: 0;
                }
                /* remember to highlight inserts somehow! */
                ins {
                    text-decoration: none;
                }
                del {
                    text-decoration: line-through;
                }
                /* tables still need 'cellspacing="0"' in the markup */
                table {
                    border-collapse: collapse;
                    border-spacing: 0;
                }
                    
                    
                    body {color:#444;font-family:"微软雅黑",Arial,Helvetica,sans-serif;font-size: 14px;}
                    #main{width:720px;margin:0 auto;}
                    #content{background:url(image/img_515.jpg) no-repeat 0 0;}
                    
                    
                    #popweixin {
                        overflow:hidden;
                        z-index:1000;
                        background:rgba(0,0,0,.5);
                        top:0;
                        left:0;
                        display:none;
                    }
                    #popweixin .tip {
                        width:100%;
                        background:#fff;
                        z-index:1001;
                    }
					
					#popweixin2 {
                        overflow:hidden;
                        z-index:1000;
                        background:rgba(0,0,0,.5);
                        top:0;
                        left:0;
                        display:none;
                    }
                    #popweixin2 .tip {
                        width:100%;
                        background:#fff;
                        z-index:1001;
                    }
                
                
                        
                    </style>
<body>
<img style="position: absolute;opacity: 0;filter: alpha(opacity=0);" src="image/ShareIcon.jpg"/>
<div id="main">
        
        <div id="popweixin">
            <div class="tip">
                <img src="image/wechat_appstore_popup.png"/>
            </div>
        </div>
        
        <div id="popweixin2">
            <div class="tip">
                <img src="image/wechat_appstore_popup2.png"/>
            </div>
        </div>
        
        <div id="content">

            <div style="width:720px;height:1130px">
                <div style="padding-top:870px;" align="center">

                    <div style="width: 250px; float:left;padding-top:20px;">
                        <img src="image/2wm.jpg" width="215">
                    </div>
                    <div style="width: 435px; float:left; margin-left: 20px">
                        <a class="btn" href="http://itunes.apple.com/cn/app/id1054312856">
                        <img style="margin:20px 0 20px 0;" src="image/btn_ios.png"></a>
                        <a class="btn" href="http://www.515.com/download/poker515.apk" ><img src="image/btn_android.png"></a> 
                    </div>
                    <div style="clear:both"></div>
                    
                    <!-- <img src="image/2wm.jpg" width="215"> -->
                    
                    <!-- <div style="position:absolute;margin-left:200px;">
                    <a class="btn" href="http://itunes.apple.com/cn/app/id1054312856">
                        <img style="margin:20px 0 20px 0;" src="image/btn_ios.png"></a>
                    <a class="btn" href="http://www.515.com/" ><img src="image/btn_android.png"></a> 
                    </div> -->
                </div>
            </div>


            <div><img src="image/line.png" width="720" height="4"  alt=""/>
            </div>

            <div align="center" style="width:720px;height:81px;padding-top:15px;"><a class="btn" href="http://www.515.com/"><img src="image/btn_toweb.png"></a>
            </div>

        </div>

        <div style="text-align:center"><a href="http://www.515.com/" >五一五游戏@2015</a>
        </div>

    </div>
    
    
    <script type="text/javascript" >
        
      /**  function a(){
            var ua = navigator.userAgent.toLowerCase();
            if (/iphone|itouch|ipad|ipod/.test(ua)) {
                if(/micromessenger/.test(ua)){
                      document.getElementById("popweixin").style.display = "block";
                }
            }
        }
        a(); 
	*/
                        
        /**
         浏览器版本信息
         * @type {Object}
         * @return {Boolean}  返回布尔值
         */
	function browser() {
	    var u = navigator.userAgent.toLowerCase();
	    var app = navigator.appVersion.toLowerCase();
	    return {
	        txt: u, // 浏览器版本信息
	        version: (u.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [])[1], // 版本号
	        msie: /msie/.test(u) && !/opera/.test(u), // IE内核
	        mozilla: /mozilla/.test(u) && !/(compatible|webkit)/.test(u), // 火狐浏览器
	        safari: /safari/.test(u) && !/chrome/.test(u), //是否为safair
	        chrome: /chrome/.test(u), //是否为chrome
	        opera: /opera/.test(u), //是否为oprea
	        presto: u.indexOf('presto/') > -1, //opera内核
	        webKit: u.indexOf('applewebkit/') > -1, //苹果、谷歌内核
	        gecko: u.indexOf('gecko/') > -1 && u.indexOf('khtml') == -1, //火狐内核
	        mobile: !!u.match(/applewebkit.*mobile.*/), //是否为移动终端
	        ios: !!u.match(/\(i[^;]+;( u;)? cpu.+mac os x/), //ios终端
            android: u.indexOf('android') > -1, //android终端
            iPhone: u.indexOf('iphone') > -1, //是否为iPhone
            iPad: u.indexOf('ipad') > -1, //是否iPad
			iPad: u.indexOf('ipod') > -1, //是否iPad
            webApp: !!u.match(/applewebkit.*mobile.*/) && u.indexOf('safari/') == -1 //是否web应该程序，没有头部与底部
        };
    }
                           
//    function open_appstore() {
//        var b=browser();
//        if(b.ios||b.iPhone||b.iPad){
//            window.location="itms-apps://http://itunes.apple.com/cn/app/id903301935";
//        }else if(b.android){
//
//        }
//    }
//                           
//    function try_to_open_app() {
//        var b=browser();
//        if(b.ios||b.iPhone||b.iPad){
//            window.location="mqq:open";
//        }else if(b.android){
//    
//        }
//        timeout = setTimeout('open_appstore()', 30);
//    }
    
//    try_to_open_app();
                           
	function a(){
		var ua = navigator.userAgent.toLowerCase();
        var b=browser();
        if(b.ios||b.iphone||b.ipad||b.ipod){
			if(/micromessenger/.test(ua)){
                document.getElementById("popweixin").style.display = "block";
             }else{				 
			 	 window.location="itms-apps://itunes.apple.com/cn/app/id1054312856";
		     }
		}else if(b.android){
			if(/micromessenger/.test(ua)){
                document.getElementById("popweixin2").style.display = "block";
             }else{				 
			 	 //window.location="http://www.515.com/download/poker515.apk";
		     }
        }
	}

    a();
        
    </script>
</body>
</html>
HTML;

echo $html;

/**
 * 获取客户端IP地址
 * @param integer $type 返回类型 0 返回IP地址 1 返回IPV4地址数字
 * @param boolean $adv 是否进行高级模式获取（有可能被伪装）
 * @return mixed
 */
function get_client_ip($type = 0,$adv=false) {
    $type       =  $type ? 1 : 0;
    static $ip  =   NULL;
    if ($ip !== NULL) return $ip[$type];
    if($adv){
        if (isset($_SERVER['HTTP_X_FORWARDED_FOR'])) {
            $arr    =   explode(',', $_SERVER['HTTP_X_FORWARDED_FOR']);
            $pos    =   array_search('unknown',$arr);
            if(false !== $pos) unset($arr[$pos]);
            $ip     =   trim($arr[0]);
        }elseif (isset($_SERVER['HTTP_CLIENT_IP'])) {
            $ip     =   $_SERVER['HTTP_CLIENT_IP'];
        }elseif (isset($_SERVER['REMOTE_ADDR'])) {
            $ip     =   $_SERVER['REMOTE_ADDR'];
        }
    }elseif (isset($_SERVER['REMOTE_ADDR'])) {
        $ip     =   $_SERVER['REMOTE_ADDR'];
    }
    // IP地址合法验证
    $long = sprintf("%u",ip2long($ip));
    $ip   = $long ? array($ip, $long) : array('0.0.0.0', 0);
    return $ip[$type];
}