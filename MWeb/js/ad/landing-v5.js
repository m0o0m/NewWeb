(function(that){
    /* landingname、pagename、url中的请求参数、landing加载的引导配置、domobtrack */
    that.landingName = that.pageName = that.queryArgv = that.bootArgv = that.domobtrack = that.dmsource = that.dmrtbtrack = that.dmrid = that.osType = that.domobid = that.dmrtbsr = null;
    that.version = 'v5.4.0';
    that.debug = false;
    that.reScale = true; // 是否调整当前的缩放比例，默认调整

    /* 提供的一些对内或对外的方法 */
    (function(lib){
    	// 生成一个随机的字符串
		lib.getRandString = function(len){          
			len = len || 32;
			var chars = 'ABC1DEFGH2IJK3LMNOQP4RSTU5VWXYZab6cdef8ghij7kmlnopq9rest0uvwxyz';                    
			var maxPos = chars.length;
			var pwd = '';
			for (i = 0; i < len; i++) { 
				pwd += chars.charAt(Math.floor(Math.random() * maxPos));
			} 
			return pwd;
		};

		// 获取当前域名
		lib.getDomain = function(){
			var host = window.location.host;
			if(/^[\d.]+$/.test(host)){
				return host;
			}else{
				return host.match(/\.?[\w-]+\.\w+(:\d+)?$/)[0];
			}
		};

		// 判断是否支持localStorage
		lib.checkStorage = function(){
			if(window.localStorage){
				return true;
			}
			return false;
		}

		// 获取当前页面url上的search字符
		lib.getQueryStr = function(){
			return location.search.substring(1);
		};
		// 获取js外链加载链接上的参数
		lib.getBootStr = function(){
			var scripts = document.getElementById('domob_landing');
			var r = scripts.src.match(/landing-(v[\d.]+).js/);
			if(r) return r.input.split('?').pop();
			return null
		};
		// query格式参数转换为json格式
		lib.queryStrToJson = function(querystr, decode){
			var params = {}; 
			querys = querystr.split('&');
			for(var i in querys){
				var _s = querys[i].indexOf('=');
				if(_s == -1) continue;
				params[querys[i].substring(0, _s)] = !decode ? querys[i].substring(_s+1) : decodeURIComponent(querys[i].substring(_s+1));
			}
			return params;
		};
		// json格式转换为query格式参数，当参数值不是一个对象的时候，原样放回
		lib.jsonToQueryStr = function(json, encode){
			var param = new Array();
			if(typeof(json) != 'object') return json;
			for(var i in json){
				json[i] = !encode ? json[i] : encodeURIComponent(json[i]);
				param.push(i + '=' + json[i])
			}
			return param.join('&');
		};
		// 判断josn中是否存在一个key
		lib.inJosn = function(str, json){
			for(var k in json){
				if(k == str) return true;
			}    
			return false;
		};
		// 将两个json合并在一起
		lib.jsonMerge = function(p, sp){
			for(var k in sp){
				// 如果key之前存在，这里给出提示，让开发者修改字段名
				if(lib.inJosn(k, p)) lib.log(k + ', the key already exists!');
				p[k] = sp[k];
			}
			return p;
		}
		// 获取浏览器类型
		lib.getBrowserType = function(){
			var ua = navigator.userAgent, app = navigator.appVersion; 
			return { 
				android: ua.indexOf('Android') > -1 || ua.indexOf('Linux') > -1,//android系统
				iPhone : ua.indexOf('iPhone') > -1,						//是否iPhone终端
				iPad   : ua.indexOf('iPad') > -1,   	    			//是否iPad终端
				iOS    : !!ua.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/) || ua.indexOf('Mac') > -1,        //iOS系统
				wp     : !!ua.match(/windows (ce|phone)/i),   	   		//是否winphone终端
				webApp : ua.indexOf('Safari') == -1, 		    		//是否web应该程序
				webKit : ua.indexOf('AppleWebKit') > -1,        		//webkit内核
			};
		};
		// 判断一个链接是否有search部分
		lib.isHasSearchByUrl = function(u){
			var pos = u.indexOf('?');
			return pos != -1 ? (u.substring(pos + 1) == '' ? false : true) : false;
		}
		// 给一个链接增加GET传递的参数
		lib.urlConcatTrack = function(u, json){
			var conc = '?';
			if(lib.isHasSearchByUrl(u)) conc = '&';
			return u += conc + lib.jsonToQueryStr(json, true);
		};
		// 控制台输出
		lib.log = function(str){
			console.log(str);
		};
		/* ajax数据提交
		 * param json object 
		 *     url 数据提交的地址
		 *     data 提交的数据，json或query格式
		 *     type 提交方式，get/post
		 *     fun 回调方法
		 *     dataType 返回数据类型
		 */
		lib.ajax = function(param){
			param.type = param.type || 'get';
			param.type.toLowerCase();
			if(!param.url) lib.log('submit url is empty');
			param.fun = param.fun || function(s){};
			if(param.type == 'get'){
				if(param.data){
					param.url = lib.urlConcatTrack(param.url, param.data);
				}
				param.data = null;
			}else if(param.type == 'post'){
				param.data = lib.jsonToQueryStr(param.data, true) || null;
			}else{
				lib.log('do not support the type of submission: ' + param.type);
			}
			var xhr = new XMLHttpRequest();
			xhr.open(param.type, param.url, true);
			param.type == 'post' ? xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded') : null;
			xhr.send(param.data);
			xhr.onreadystatechange = function(){
				if(xhr.readyState == 4 && xhr.status == 200){
					var res = xhr.responseText;
					if(param.dataType == 'json'){
						try{
							res = eval('(' + res + ')')
						}catch(e){}
					}
					param.fun(res);
				}
			}
		};
		// 追加css样式到页面
		lib.addCss = function(css) { 
			try{
				var style = document.createStyleSheet();
				style.cssText = css;
			}catch (e) {
				var style = document.createElement("style");
				style.type = "text/css";
				style.textContent = css;
				document.getElementsByTagName("HEAD").item(0).appendChild(style);
			}
		};
		// cookie操作
		(function(cookie){
			// 获取cookie
			cookie.get = function(name){ 
				var cookies = document.cookie.split('; '); 
				for(var i = 0; i < cookies.length; i++){ 
					var arr = cookies[i].split('='); 
					if(arr[0] == name)
						return decodeURIComponent(arr[1]); 
				} 
				return null; 
			};
			// 存cookie
			cookie.set = function(name, value, expiresHours){ 
				var cookieStr = name + '=' + encodeURIComponent(value); 
				expiresHours = expiresHours || 0;
				if(expiresHours > 0){
					var date = new Date(); 
					date.setTime(date.getTime() + expiresHours*3600*1000); 
					cookieStr += '; expires=' + date.toGMTString() + ";path=/; domain=" + that.lib.getDomain(); 
				} 
				document.cookie = cookieStr; 
			};
		})(lib.cookie = {});

		// localStorage
		(function(storage){
			storage.get = function(name){
				if(that.lib.checkStorage){
					return window.localStorage.getItem(name);
				} else {
					return that.lib.log('not support localStorage');
				}
			};

			storage.set = function(name, value){
				if(that.lib.checkStorage){
					return window.localStorage.setItem(name, value);
				} else {
					return that.lib.log('not support localStorage');
				}
			};
		})(lib.storage = {});
	})(that.lib = {});

    /* 核心的一些方法 */
    (function(core){
		// 添加基础CSS代码 
		core.addBaseCSS = function(){
			that.lib.addCss(
				'html,body{margin:0}' + 
				'*{-webkit-tap-highlight-color:rgba(0,0,0,0);tap-highlight-color:rgba(0,0,0,0);}' + 
				'body{opacity:0;-webkit-transition: opacity 0.5s}' + 
				'.dm-hidden{width:0;height:0;display:none;position:absolute}')
		};
		// 设置默认viewport宽度 
		core.setViewport = function(){
			var meta = document.createElement('meta');
			meta.name = 'viewport';
			meta.content = 'width=device-width,user-scalable=no';
			document.getElementsByTagName('head')[0].appendChild(meta);
		};
		// 页面展示 
		core.showPage = function(dWidth){
			var deviceWidth = document.documentElement.clientWidth;
			if(that.reScale){
				dWidth = dWidth || 320;
				scale = deviceWidth/dWidth;
				if(!that.debug) document.body.style.zoom = scale;
			}
			document.body.style.opacity = '1';
		};
		// 检查当前是否是debug模式
		core.checkDebug = function(){
			if(that.lib.cookie.get('dmDebug')){
				that.debug = true;
			} 
		};

		// track操作
		(function(track){
			// 获取全站全局参数
			track.get = function(k, enableStorage){
				if(!k) return;
				enableStorage = enableStorage || false;
				var v = that.queryArgv[k] || getByRefer(k) || getByCookie(k) || null;

				if(enableStorage && !v){
					v = getByLocalStorage(k) || null;
				}
				return v;	
			};
			// 绑定参数为全站全局参数
			track.set = function(k, v, enableStorage, expiresHours){
				if(!k || !v) return;
				enableStorage = enableStorage || false;
				expiresHours = expiresHours || 0;
				// setByTag(k, v);
				if(enableStorage) setByLocalStorage(k, v);

				setByCookie(k, v, expiresHours);
			};
			// 从referer中获取一个参数名的值
			var getByRefer = function(v){
				try{
					var p = that.lib.queryStrToJson(document.referer.split('?').pop() ,true);
					return P[v];
				}catch(e){}
				return null;
			};
			
			// 从cookie中获取一个参数名的值
			var getByCookie = function(v){
				return that.lib.cookie.get(v);
			};
				
			// 从localStorage 中取
			var getByLocalStorage = function(k){
				return that.lib.storage.get(k);
			};

			// 替换页面上的所有a标签的href值
			var setByTag = function(k, v){
				var aTags = document.getElementsByTagName('a');
				var json = {};
				json[k] = v;
				for(var i = 0; i < aTags.length; i++){
					if(!aTags[i].href || aTags[i].href.match(/^javascript/)) continue;
					aTags[i].href = that.lib.urlConcatTrack(aTags[i].href, json);
				}
			};
			// 设置参数存储到cookie中
			var setByCookie = function(k, v, expiresHours){
				that.lib.cookie.set(k, v, expiresHours);
			}

			// 设置参数存储到 localStorage 中
			var setByLocalStorage = function(k, v){
				that.lib.storage.set(k,v);
			};

		})(core.track = {})
		// 载入扩展库
		core.loadExtend = function(){
			if(!that.bootArgv.extend)return;
			var script = document.createElement('script');
			script.type = 'text/javascript';
			script.src = 'http://service.moxz.cn/landingjs/extend_' + that.bootArgv.extend + '.js';
			document.getElementsByTagName('head')[0].appendChild(script);
		};
		// 监测汇报数据整理
		core.monitor = function(type, dParam, fun){
			var os = null;
			for(var i in that.osType){
				if(that.osType[i]) {
					os = i;break;
				}
			}
			var param = {
				'name' : that.landingName,
				'page' : that.pageName,
				'monitortype' : type,
				'os' : os,
				'second' : new Date().getTime(),
				'version' : that.version,
				'domobid' : that.domobid
			}
			that.domobtrack ? param['domobtrack'] = that.domobtrack : null;
			that.dmsource ? param['dmsource'] = that.dmsource : null;
			that.dmrtbsr ? param['dmrtbsr'] = that.dmrtbsr : null;
			that.dmrtbtrack ? param['dmrtbtrack'] = that.dmrtbtrack : null;
			that.dmrid ? param['dmrid'] = that.dmrid : null;
			param = that.lib.jsonMerge(param, dParam);
			request(param, fun);
		};
		// 像服务器汇报监测
		var request = function(param, fun){
			var paramStr = that.lib.jsonToQueryStr(param, true);
			var src = (("https:" == document.location.protocol) ? "https://" : "http://") + 'duomeng.cn/e.gif?' + paramStr;
			var tmpImg = document.createElement('IMG');
			tmpImg.className = "dm-hidden";
			tmpImg.src = src;
			tmpImg.onload = function(){
				if(fun) fun({'status':true, 'param':param});
				that.lib.log(paramStr + ' | success!');
			};
			tmpImg.onerror = function(){
				if(fun) fun({'status':false, 'param':param});
				that.lib.log(paramStr + ' | error!');
			};
			document.body.appendChild(tmpImg);
		};
    })(that.core = {});

    // 初始化类型
    (function(init){
		// 页面加载过程中初始化
		init.loading = function(){
			that.queryArgv = that.lib.queryStrToJson(that.lib.getQueryStr(), true);
			that.bootArgv  = that.lib.queryStrToJson(that.lib.getBootStr(), true);
			that.bootArgv['landingid'] ? that.landingName = that.bootArgv['landingid'] : that.lib.log('landingid not defined');
			that.pageName = that.bootArgv['pageid'] ? that.bootArgv['pageid'] : 'index';
			that.reScale = that.bootArgv['rescale'] == '0' ? false : true;
			that.core.loadExtend();
			that.domobtrack = that.core.track.get('domobtrack'); 
			that.dmrtbsr = that.core.track.get('dmrtbsr'); 
			that.dmsource   = that.core.track.get('dmsource'); 
			that.dmrtbtrack = that.core.track.get('dmrtbtrack'); 
			that.dmrid = that.core.track.get('dmrid');
			that.domobid = that.core.track.get('domobid', true) || that.lib.getRandString();
			that.osType = that.lib.getBrowserType();
			that.core.setViewport();
			that.core.addBaseCSS();
		};
		// 加载完成
		init.loaded = function(){
			that.core.checkDebug();
			that.core.showPage();
			that.core.track.set('domobtrack', that.domobtrack || null);
			that.core.track.set('dmsource', that.dmsource || null);
			that.core.track.set('dmrtbsr', that.dmrtbsr || null);
			that.core.track.set('dmrtbtrack', that.dmrtbtrack || null);
			that.core.track.set('dmrid', that.dmrid || null);
			that.core.track.set('domobid', that.domobid || null, true, 10 * 365 * 24);
			that.monitor.pv();
		};
		// 旋转
		init.rotating = function(){
			that.core.showPage();
		}	
    })(that.init = {});

     // 提供的常用monitor
    (function(monitor){
		/** 
		 * 点击监测
		 * clkname string 给点击按钮赋予的名
		 * action object or function or undefined 监测回报成功后调用的动作
		 *     object 如果传入的是个对象，会将该对象当成被点击的dom对象，获取起的href属性，然后跳转
		 *     function 如果传入的是一个方法，会在对象回报成功后执行这个方法
		 */
		monitor.clk = function(clkname, action){
			var type = typeof(action);
			var fun = null;
			if(type == 'undefined' || (type == 'object' && !action.getAttribute('href'))){
				fun = null;
			}else if(type == 'object'){
				fun = function(){
					location.href = action.getAttribute('href');
				}
			}else if(type == 'function'){
				fun = action;
			}
			that.core.monitor('click', {'button':clkname}, fun);
		};
		/**
		 * pv监测
		 */
		monitor.pv = function(){
			that.core.monitor('pv');
		};
		/** 
		 * 提供一个注册数据监测
		 * regname string 给这个注册赋予的名
		 * fun 注册成功后调用该方法
		 * report 汇报{url:,data:,type:(get/post),fun:function(json){return true/false}}
		 */
		monitor.reg = function(param, fun, regName, report){
			regName = regName ? 'register_' + regName : 'register';
			typeof(fun) != 'function' ? fun = undefined : null;

			if(typeof(report) != 'object'){
				that.core.monitor(regName, param, fun);
			}else{
				report.fun ? ajaxRequest = report.fun : function(s){return true};
				ajaxFun = function(s){
					var reportstatus = ajaxRequest(s) ? '1' : '2';
					that.core.monitor(regName, that.lib.jsonMerge(param, {'apista':reportstatus}), fun);
				};
				report.dataType = 'json';
				report.fun = ajaxFun;
				that.lib.ajax(report);
			}
		};
    })(that.monitor = {});
})(window.DomobLanding = window.DomobLanding || {});

// init  
DomobLanding.init.loading();
try{
	/*
    window.addEventListener('load', function(){
		DomobLanding.init.loaded();
    }, false);
	*/
    window.addEventListener('load', DomobLanding.init.loaded, false);
    window.addEventListener('orientationchange', function(){
		DomobLanding.init.rotating()
    }, false);
}catch(e){
	/*
    window.attachEvent('onload', function(){
		DomobLanding.init.loaded();
    }, false);
	*/
    window.attachEvent('load', DomobLanding.init.loaded, false);
    window.attachEvent('onorientationchange', function(){
		DomobLanding.init.rotating()
    }, false);
}
