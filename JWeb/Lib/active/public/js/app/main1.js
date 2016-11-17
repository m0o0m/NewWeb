console.log('load main1.js');
define(function (require) {
	
    var $ = require('jquery'),
	    //require(string)的参数 参照的路径规则是
		// ./ , ../ 打头的总是以当前文件为路径基准的,
		// 直接字符的话,那么就是先找config中的paths配置,如果没有找到的话,再以baseUrl为基准
        Flotr=require('flotr');

    
    $(function () {
       
       var container = document.getElementById('container'),
       data = [],
       graph, i;

     // Sample the sine function
     for (i = 0; i < 4 * Math.PI; i += 0.2) {
       data.push([i, Math.sin(i)]);
     }

     // Draw Graph
     graph = Flotr.draw(container, [ data ], {
       yaxis : {
         max : 2,
         min : -2
       }
     });
    });
});
