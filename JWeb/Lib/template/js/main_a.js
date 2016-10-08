var hUrl='';
$(document).ready(function(){
	hUrl=document.URL.split( "?" )[0];
	$('#actcenter').click(function(e){
		layer.closeAll();
		if(uid!=''){
			$('#content').css('top','-20000px');
			$("#article li").attr("class","");
			$(this).attr("class","li_one");
			main_actcenter();
		}
		e.preventDefault();
	});
	$('#mobilebtn').click(function(){
		onGotoGG();
	});
	$('#article_logo,#gamecontent').click(function(e){
		e.preventDefault();
		onLogoClick();
	});
	$('#article_yq').click(function(e){
		doInvite();
		e.preventDefault();
	});
	$('#article_sc').click(function(e){
		AddFavorite();
		e.preventDefault();
	});
});

function onLogoClick()
{
	layer.closeAll();
	$('#content').css('top','147px');
	$("#article li").attr("class","");
	$("#article li").eq(0).attr("class","li_one");
	e.preventDefault();
}

var mainindex=0;
function main_actcenter(){
	
	mainindex =	layer.open({
		    type: 2,
		    closeBtn: false, //不显示关闭按钮
		    shift: 9,
		    fix: false,
		    title: false,
		    offset:["147px","0px"],
		    area: ["950px","900px"],
		    shadeClose: false, //开启遮罩关闭
		    content: "./index.php/home/Actcenter?uid="+uid+"&hip="+hip+"&hpost="+hpost,
		    shade:false,
		    zIndex: layer.zIndex, //重点1
		    success: function(layero){
		        layer.setTop(layero); //重点2
		    }
		});
		layer.style(mainindex, {
		    top: '147px',
		    left: '0px'
		});
}
function asCallDoInvite()  {
	doInvite();
}

function doInvite() {
	fusion2.dialog.invite({// receiver : "00000000000000000000000000009FED",
		msg  : "邀请你来玩~",
		source : "app",
		context : "invite",
		onSuccess : doInviteSuccess,
		onCancel : function (opt) { doTrace("邀请取消"); },
		onClose : function (opt) {  doTrace("邀请关闭"); }
	});
}
	
	function asCallDoBuy(url_param) {
	var _sandbox = false;
	if(sandbox==1){
		_sandbox = true;
	}
		fusion2.dialog.buy
		({
		disturb : true,
		param : url_param,
		context : url_param,
		sandbox : _sandbox,
		onSuccess : function (opt) {doBuyCallBack(opt, 0);},
		onCancel : function (opt) {doBuyCallBack(opt, 1);},
		onSend : function (opt) {},
		onClose : function (opt) {doBuyCallBack(opt, 3);}
		});
	}
	
	function doBuyCallBack(opt, flag) {
		var swfobj = thisMovie("ClientLoader");
		if(swfobj && (swfobj.jsCallOnBuyCallBack)) {
			var obj = new Object();
			obj.opt = opt;
			obj.flag = flag;
			swfobj.jsCallOnBuyCallBack(obj);
		}
	}
	
	function asCallshowCouponActivity(context1){
		

fusion2.dialog.showCouponActivity 
({
    context : context1, 

    onSuccess : function (opt) 
     {  
         doshowCouponActivityCallBack(opt,0);
     },

    onClose : function (opt) 
     {  
        doshowCouponActivityCallBack(opt,1);  
     }
  })


	}
	
	function doshowCouponActivityCallBack(opt, flag) {
		var swfobj = thisMovie("ClientLoader");
		if(swfobj && (swfobj.jsCallshowCouponActivityBack)) {
			var obj = new Object();
			obj.opt = opt;
			obj.flag = flag;
			swfobj.jsCallshowCouponActivityBack(obj);
		}
	}
	
	function doInviteSuccess(opt) {
		doTrace('doInviteSuccess');
		var swfobj = thisMovie("ClientLoader");
		if(swfobj && (swfobj.jsCallOnInvitedSucceed)) {
			swfobj.jsCallOnInvitedSucceed(opt);
		}
	}
	
	function asCallDoStory(ssobj) {
		doTrace('调用分享接口 fusion2.dialog.sendStory：' + ssobj.title);
		doTrace(ssobj.summary);
		var pf = ssobj.pf;
		var opt = new Object();
		if(pf == 'tapp') {	// 微博平台分享
			
		}
		else if(pf == '3366') {	// 3366分享
			
		}
		else {
			fusion2.dialog.sendStory ({ 
				title :ssobj.title,
				img:ssobj.img,
				//receiver : [""],
				summary :ssobj.summary,
				msg:ssobj.msg,
			//	button :"进入应用",
				source :ssobj.source,
				context:ssobj.context,
				onShown : function (opt) {doTrace("Shown");},
				onSuccess : function (opt) {
					// opt.context：可选。opt.context为调用该接口时的context透传参数，以识别请求
					doTrace("succeeded: " + opt.context);
					shareStoryCb(opt, 0);
				},
				onCancel : function (opt) {
					// opt.context：可选。opt.context为调用该接口时的context透传参数，以识别请求
					doTrace("cancelled: " + opt.context);
					shareStoryCb(opt, 1);
				},
				onClose : function (opt) {
					doTrace("closed: " + opt.context);
					shareStoryCb(opt, 2);
				}
			});
		}
	}
	
	function shareStory2(ssobj){
		
		fusion2.dialog.sendStory ({ 
			title :ssobj.title,
			img:ssobj.img,
			//receiver : [""],
			summary :ssobj.summary,
			msg:ssobj.msg,
		//	button :"进入应用",
			source :ssobj.source,
			context:"send-story"
			
		});
	}
	
	function shareStoryCb(opt, flag) {
		var viFlash = thisMovie("ClientLoader");
		 if(viFlash && (viFlash.jsCallOnSendStoryCb))
		{
			opt.YQB_flag = flag;
			viFlash.jsCallOnSendStoryCb(opt);
		}
	}
	
	function asCallSetCompanyNumber(cn) {
		var obj = N$.GT('ClientLoader');
		if(obj) {
			N$.INH(obj, cn);
		}
		else {
			N$.ATCE(document, 'load', function(){N$.INH('CompanyNumber', cn);});
		}
	}
	
	function asCallRelogin() {
		fusion2.dialog.relogin();
	}
	
	function asCallRefreshPage() {
		location.reload();
	}
	
	function asCallTrace(info) {
		doTrace(info);
	}
	
	function doTrace(info) {
	/*	var infoAry = info.split('\n');
		for(var i= 0; i&lt;infoAry.length; i++) {
			if(infoAry[i].length &gt; 0) {
				var para=document.createElement('p');
				var node=document.createTextNode(infoAry[i]);
				para.appendChild(node);
				N$.GT('debug_container').appendChild(para);
			}
		}*/
	}
	
	//'freegift','request'
	function doSendRequest(fgObj, srType, cbFunc) {
	/*	var opt = new Object();
		opt.context = fgObj.ct;
		if(fgObj.testRecvId) {
			opt.receiver = [fgObj.testRecvId]; 
			if(window.confirm('功能测试，确定赠送么？确定（是），取消（否）')) {
				cbFunc(opt,1);
			}
			else {
				cbFunc(opt,0);
			}
		}
		else {
			win_alert('没有好友OpenID，不能赠送!');
			cbFunc(opt,0);
		}*/
		doTrace('调用免费礼物接口 fusion2.dialog.sendRequest：' + fgObj.sfgb.giftName);
		fusion2.dialog.sendRequest({type : srType,
			receiver : [""],
			stat:'action_id',
			title : fgObj.sfgb.giftName,
			msg : '1234567',
			img:_ICON_PATH + '/feed_' + fgObj.sfgb.giftId + '.png',
			source :fgObj.source,
			desc : fgObj.sfgb.giftDesc,
			context : fgObj.ct,
			callback : '',
			onSuccess : function (opt) {cbFunc(opt,1);},
			onCancel : function (opt) {cbFunc(opt,0);},
			onClose : function (opt) {cbFunc(opt,0);}});
	}
	function asCallSendFreeGift(fgObj) {
		doSendRequest(fgObj, 'freegift', sendFreeGiftCb);
	}
	function asCallSendFreeGiftReq(fgObj) {
		doSendRequest(fgObj, 'request', sendFreeGiftReqCb);
	}
	function sendFreeGiftCb(opt, flag) {
		var swfobj = thisMovie("ClientLoader");
		if(swfobj && (swfobj.jsCallOnSendFreeGiftCb)) {
			opt.YQB_flag = flag;
			swfobj.jsCallOnSendFreeGiftCb(opt);
		}
	}
	function sendFreeGiftReqCb(opt, flag) {
		var swfobj = thisMovie("ClientLoader");
		if(swfobj && (swfobj.jsCallOnSendFreeGiftReqCb)) {
			opt.YQB_flag = flag;
			swfobj.jsCallOnSendFreeGiftReqCb(opt);
		}
	}
	function asCallRecharge() {
		click_recharge();
	}
	function asCallAdd2QQClient() {
		fusion2.dialog.addClientPanel({
			context : "add_Client_Panel_1",
			onSuccess : function(opt) {add2QQClientCb(opt, 1);},
			onCancel : function(opt) {add2QQClientCb(opt, 2);},
			onClose : function(opt) {add2QQClientCb(opt, 3);}
		});
	}
	function add2QQClientCb(opt, flag) {
		var swfobj = thisMovie("ClientLoader");
		if(swfobj && (swfobj.jsCallOnAdd2QQClientPanelCb)) {
			opt.YQB_flag = flag;
			swfobj.jsCallOnAdd2QQClientPanelCb(opt);
		}
	}
	
	function asCallAdd2QQGroup() {
		var str = '';
		var count = 0;
		for (var i = 0; i<_qqGroupJso.length; i++) {
			if(_qqGroupJso[i].qgstatus == 1) {
				if(str.length > 0) {
					str += ',';
				}
				str += _qqGroupJso[i].groupopenid;
				count++;
			}
			if(count >= 5) {
				break;
			}
		}
		fusion2.dialog.inviteToGroup({
			context : "add_Client_Panel_1",
			officialGroup : str,
			onSuccess : function(opt) {add2QQGroupCb(opt, 1);},
			onCancel : function(opt) {add2QQGroupCb(opt, 2);},
			onClose : function(opt) {add2QQGroupCb(opt, 3);},
			onError : function(opt) {add2QQGroupCb(opt, 3);}
		});
	}
	
	function asCallReactive(){
		fusion2.dialog.reactive({
		     title : "召回老友奖励", 
		     receiveImg: _ICON_PATH + '/feed_4.png',
		     sendImg: _ICON_PATH + '/co_107.png',
		     msg: "我玩515时想起了你，没有你我很孤单，送你个礼物，回来我和一起玩吧。", 
		    
			onSuccess : function(opt) {reactiveCallBack(opt, 1);},
			onCancel : function(opt) {reactiveCallBack(opt, 2);},
			onClose : function(opt) {reactiveCallBack(opt, 3);}
		});	
	}

	function add2QQGroupCb(opt, flag) {
		var swfobj = thisMovie("ClientLoader");
		if(swfobj && (swfobj.jsCallOnAdd2QQGroupCb)) {
			opt.YQB_flag = flag;
			swfobj.jsCallOnAdd2QQGroupCb(opt);
		}
	}
	
	function reactiveCallBack( opt, flag)
	{
		var swfobj = thisMovie("ClientLoader");
		if(swfobj && (swfobj.jsCallReactive)) {
			opt.YQB_flag = flag;
			swfobj.jsCallReactive(opt);
		}		
	}
	
	
	var uid='';
	var hip='';
	var hpost=0;
	function lockAccountHandler(gid)
	{
		uid=gid;
		document.getElementById("spError").innerText = "您的账号ID("+gid+")已经封号冻结，如有疑问，请联系在线客服QQ:4000825515";
		var divObj=document.getElementById("divError");
		if( divObj )
			divObj.style.display='block'; 
		swfobject.removeSWF("ClientLoader"); 
	}
	function lockIpHandler(msg)
	{
		document.getElementById("spError").innerText = msg;
		var divObj=document.getElementById("divError");
		if( divObj )
			divObj.style.display='block'; 
		swfobject.removeSWF("ClientLoader"); 
	}
	
	function setCopy(value){
		var viFlash = thisMovie("ClientLoader");
		 if(viFlash && (viFlash.jsCallcopytext))
		{
			viFlash.jsCallcopytext(value);
		}
	}

function AddFavorite() {
	var url = window.location;
	var title = document.title;
	var ua = navigator.userAgent.toLowerCase();
	if (ua.indexOf("360se") > -1) {
		alert("由于360浏览器功能限制，请按 Ctrl+D 手动收藏！");
	}
	else if (ua.indexOf("msie 8") > -1) { //备注：360浏览器 8.0兼容模式也会进来
		try{
			window.external.AddToFavoritesBar(url, title); //IE8
		}catch(e){
			try{
				window.sidebar.addPanel(title, url, "");
			}catch(e){
				alert("您的浏览器不支持,请按 Ctrl+D 手动收藏!");
			}
		}
	}
	else if (document.all) {
		try{
			window.external.addFavorite(url, title);
		}catch(e){
			alert('您的浏览器不支持,请按 Ctrl+D 手动收藏!');
		}
	}
	else if (window.sidebar) {
		window.sidebar.addPanel(title, url, "");
	}
	else {
		alert('您的浏览器不支持,请按 Ctrl+D 手动收藏!');
	}
}

function onReload(){
	var viFlash = thisMovie("ClientLoader");
	 if(viFlash && (viFlash.jsCallReload))
	{
		viFlash.jsCallReload();
	}
	location.reload();
}