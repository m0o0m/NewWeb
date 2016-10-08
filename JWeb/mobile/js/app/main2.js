console.log('main2 load');
define(['css!../../css/iscroll-load-data.css','jquery','IScrollLoadData'],
		function (css,$,IScrollLoadData) {

   
    $(function () {
    	function dropTopAction(cb){
    		setTimeout(function(){
    			var d=document.createDocumentFragment();
    			for(var i=0;i<3;i++){
    	          var li=document.createElement('li');
    	          li.innerText='Generated top row '+new Date().getTime();
    	          d.appendChild(li);
    			}
    			cb(d);
    		},5000);
    	}
    	function dropBottomAction(cb){
    		setTimeout(function(){
    			var d=document.createDocumentFragment();
    			for(var i=0;i<3;i++){
    	          var li=document.createElement('li');
    	          li.innerText='Generated bottom row '+new Date().getTime();
    	          d.appendChild(li);
    	          
    			}
    			cb(d);
    		},5000);
    	}
    	function loaded(){
    		var wrapper=document.querySelector('#wrapper');
    		var content=document.querySelector('#thelist');
    		
    		new IScrollLoadData(wrapper,content,dropTopAction,dropBottomAction,30);
    	}
    	document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);
    	loaded();
    });
});
