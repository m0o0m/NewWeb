window.Dmlib = {}

var serverUrl = 'http://ut.domobcdn.com/api.php?callback=DmCallback';
var syncFields = new Array('domobid');
var isRemoveDlLoadEvent = false;
var aliveTime = 0;
var isLoaded = false;
var isSendMonitor = false;

Dmlib.getDomain = function(){
	var host = window.location.host;
	if(/^[\d.]+$/.test(host)){
		return host;
	}else{
		return host.match(/\.?[\w-]+\.\w+(:\d+)?$/)[0];
	}
};

(function(cookie){
	cookie.get = function(name){ 
		var cookies = document.cookie.split('; '); 
		for(var i = 0; i < cookies.length; i++){ 
			var arr = cookies[i].split('='); 
			if(arr[0] == name)
			return decodeURIComponent(arr[1]); 
		} 
		return null; 
	};
	cookie.set = function(name, value, expiresHours){ 
		var cookieStr = name + '=' + encodeURIComponent(value); 
		expiresHours = expiresHours || 0;
		if(expiresHours > 0){
			var date = new Date(); 
			date.setTime(date.getTime() + expiresHours*3600*1000); 
			cookieStr += '; expires=' + date.toGMTString() + ";path=/; domain=" + Dmlib.getDomain(); 
		} 
		document.cookie = cookieStr; 
	};
})(window.Dmlib.cookie = {});

(function(storage){
	storage.get = function(name){
		if(window.localStorage){
			return window.localStorage.getItem(name);
		} else {
			console.log('not support localStorage');
		}
	};
	storage.set = function(name, value){
		if(window.localStorage){
			window.localStorage.setItem(name, value);
		} else {
			console.log('not support localStorage');
		}
	};
})(window.Dmlib.storage = {});


function sendMessage(){
	var data = {};
	for(var k in syncFields){
		k = syncFields[k];
		data[k] = Dmlib.cookie.get(k) || Dmlib.storage.get(k) || '';
	}

	var script = document.createElement('script');
	script.type = 'text/javascript';
	script.src = serverUrl + '&args=' + encodeURIComponent(JSON.stringify(data));
	document.getElementsByTagName('HEAD').item(0).appendChild(script);
}

function sendMonitor(){
	if(!isSendMonitor){
		DomobLanding.domobid = DomobLanding.core.track.get('domobid', true) || DomobLanding.lib.getRandString();
		DomobLanding.init.loaded();
		isSendMonitor = true;
	}
}

function DmCallback(response){
	if(!response.status){
		sendMonitor();
		console.log('server error.');
		return;
	}
	for(var k in syncFields){
		k = syncFields[k];
		var v = response.info[k] || '';
		Dmlib.cookie.set(k, v, 10 * 365 * 24);
		Dmlib.storage.set(k, v); 
	}
	sendMonitor();
}

// Send the jsonp to request the cookie data.
sendMessage();

var interval = 50;
var intervalHandle = setInterval(function(){
	try{
		if(isLoaded){
			aliveTime += interval;
		}
		if(aliveTime == 1000){
			sendMonitor();
		}
		if(!isRemoveDlLoadEvent && DomobLanding){
			window.removeEventListener('load', DomobLanding.init.loaded, false)
		}
		// clearinterval(intervalHandle);
	}catch(e){}
}, interval)

try{
	window.addEventListener('load', function(){
		isLoaded = true;
	}, false);
}catch(e){
	window.attachEvent('onload', function(){
		isLoaded = true;
	}, false);
}

