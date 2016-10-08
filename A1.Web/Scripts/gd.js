// JavaScript Document
    function ZoomPic() {
        this.initialize.apply(this, arguments)
    }
    ZoomPic.prototype =
    {
        initialize : function (id) {
            var _this = this;
            this.wrap = typeof id === "string" ? document.getElementById(id) : id;
            this.oUl = this.wrap.getElementsByTagName("ul")[0];
            this.aLi = this.wrap.getElementsByTagName("li");
            this.prev = this.wrap.getElementsByTagName("pre")[0];
            this.next = this.wrap.getElementsByTagName("pre")[1];
            this.timer = null;
            this.aSort = [];
            this.iCenter = 1;
            this._doPrev = function () {
                return _this.doPrev.apply(_this)
            };
            this._doNext = function () {
                return _this.doNext.apply(_this)
            };
            this.options = [
                {width:571, height:295, top:49, left:190, zIndex:3},
                {width:571, height:295, top:19, left:30, zIndex:4},
            ];
            for (var i = 0; i < this.aLi.length; i++) this.aSort[i] = this.aLi[i];
            this.aSort.unshift(this.aSort.pop());
            this.setUp();
            this.addEvent(this.prev, "click", this._doPrev);
            this.addEvent(this.next, "click", this._doNext);
            this.doImgClick();						    
            this.wrap.onmouseover = function () {
               clearInterval(_this.timer);
            };
            this.wrap.onmouseout = function () {
               // _this.timer = setInterval(function () {
               //     _this.doNext()
               // }, 5000);
            }
        },
        doStop:function(){
           clearInterval(this.timer);	
        },
        doPrev : function () {
            this.aSort.unshift(this.aSort.pop());
            this.setUp()
            this.doStop();
        },
        doNext : function () {
            this.aSort.push(this.aSort.shift());
            this.setUp()
            this.doStop();
        },
        doImgClick : function () {
            var _this = this;
            for (var i = 0; i < this.aSort.length; i++) {
                this.aSort[i].onclick = function (e) {
                    if (this.index > _this.iCenter) {
                        for (var i = 0; i < this.index - _this.iCenter; i++) _this.aSort.push(_this.aSort.shift());
                        _this.setUp();
                        return false;
                    }
                    else if (this.index < _this.iCenter) {
                        for (var i = 0; i < _this.iCenter - this.index; i++) _this.aSort.unshift(_this.aSort.pop());
                        _this.setUp();
                        return false;
                    }
                }
            }
        },
        setUp : function () {
			
            var _this = this;
            var i = 0;
            for (i = 0; i < this.aSort.length; i++) this.oUl.appendChild(this.aSort[i]);
              for (i = 0; i < this.aSort.length; i++) {
                this.aSort[i].index = i;
                if (i < 2) {
                    this.css(this.aSort[i], "display", "block");
                    this.doMove(this.aSort[i], this.options[i], function () {
                        _this.doMove(_this.aSort[_this.iCenter].getElementsByTagName("span")[0], {opacity:0})
						_this.doMove(_this.aSort[_this.iCenter].getElementsByTagName("div")[0], {opacity:100})
						_this.doMove(_this.aSort[_this.iCenter].getElementsByTagName("a")[0], {opacity:100})
						_this.doMove(_this.aSort[_this.iCenter].getElementsByTagName("div")[2], {opacity:100})
						 _this.doMove(_this.aSort[_this.iCenter].getElementsByTagName("div")[3], {opacity:100})
                    });
                }
                else {
                    this.css(this.aSort[i], "display", "none");
                    this.css(this.aSort[i], "width", 0);
                    this.css(this.aSort[i], "height", 0);
                }
                if (i==0) {
                     this.css(this.aSort[i].getElementsByTagName("span")[0], "opacity", 100);
					 
					 this.css(this.aSort[i].getElementsByTagName("div")[0], "opacity", 0)
					 this.css(this.aSort[i].getElementsByTagName("a")[0], "opacity", 0)
					 this.css(this.aSort[i].getElementsByTagName("a")[0], "height", 0)
					 this.css(this.aSort[i].getElementsByTagName("a")[0], "width", 0)
					 this.css(this.aSort[i].getElementsByTagName("div")[2], "opacity", 0)
					this.css(this.aSort[i].getElementsByTagName("div")[0], "width", 0);
					 this.css(this.aSort[i].getElementsByTagName("div")[0], "height", 0);
					 this.css(this.aSort[i].getElementsByTagName("div")[3], "opacity", 0);
					 this.css(this.aSort[i].getElementsByTagName("div")[3], "width", 0);
					 this.css(this.aSort[i].getElementsByTagName("div")[3], "height", 0);
                    this.aSort[i].onmouseover = function () {
                        _this.doMove(this.getElementsByTagName("span")[0], {opacity:30})
						_this.doMove(this.getElementsByTagName("div")[0], {opacity:0})
						_this.doMove(this.getElementsByTagName("a")[0], {opacity:0})
						_this.doMove(this.getElementsByTagName("div")[2], {opacity:0})
						_this.doMove(this.getElementsByTagName("div")[3], {opacity:0})
						
                    };
                    this.aSort[i].onmouseout = function () {
                        _this.doMove(this.getElementsByTagName("span")[0], {opacity:30})
						_this.doMove(this.getElementsByTagName("div")[0], {opacity:0})
						_this.doMove(this.getElementsByTagName("a")[0], {opacity:0})
						_this.doMove(this.getElementsByTagName("div")[2], {opacity:0})
						_this.doMove(this.getElementsByTagName("div")[3], {opacity:0})
                    };
                }
				 if (i==1) {
                     this.css(this.aSort[i].getElementsByTagName("span")[0], "opacity", 0);
					 this.css(this.aSort[i].getElementsByTagName("div")[0], "opacity", 100)
					 this.css(this.aSort[i].getElementsByTagName("div")[0], "width", 507);
					 this.css(this.aSort[i].getElementsByTagName("div")[0], "height", 164);
					 this.css(this.aSort[i].getElementsByTagName("a")[0], "opacity", 100)
					  this.css(this.aSort[i].getElementsByTagName("a")[0], "height", 84)
					 this.css(this.aSort[i].getElementsByTagName("a")[0], "width",334)
					 this.css(this.aSort[i].getElementsByTagName("div")[2], "opacity", 100)
					 this.css(this.aSort[i].getElementsByTagName("div")[3], "width", 120)
					 this.css(this.aSort[i].getElementsByTagName("div")[3], "height", 25)
					 this.css(this.aSort[i].getElementsByTagName("div")[3], "opacity", 100)
                    this.aSort[i].onmouseover = function () {
                        _this.doMove(this.getElementsByTagName("span")[0], {opacity:0})
						_this.doMove(this.getElementsByTagName("div")[0], {opacity:100})
						_this.doMove(this.getElementsByTagName("a")[0], {opacity:100})
						_this.doMove(this.getElementsByTagName("div")[2], {opacity:100})
						_this.doMove(this.getElementsByTagName("div")[3], {opacity:100})
                    };
                    this.aSort[i].onmouseout = function () {
                        _this.doMove(this.getElementsByTagName("span")[0], {opacity:0})
						_this.doMove(this.getElementsByTagName("div")[0], {opacity:100})
						_this.doMove(this.getElementsByTagName("a")[0], {opacity:100})
						_this.doMove(this.getElementsByTagName("div")[2], {opacity:100})
						_this.doMove(this.getElementsByTagName("div")[3], {opacity:100})
                    };
                }
                else {
                    this.aSort[i].onmouseover = this.aSort[i].onmouseout = null
                }
            }
        },
        addEvent : function (oElement, sEventType, fnHandler) {
            return oElement.addEventListener ? oElement.addEventListener(sEventType, fnHandler, false) : oElement.attachEvent("on" + sEventType, fnHandler)
        },
        css : function (oElement, attr, value) {
            if (arguments.length == 2) {
                return oElement.currentStyle ? oElement.currentStyle[attr] : getComputedStyle(oElement, null)[attr]
            }
            else if (arguments.length == 3) {
                switch (attr) {
                    case "width":
                    case "height":
                    case "top":
                    case "left":
                    case "bottom":
                        oElement.style[attr] = value + "px";
                        break;
                    case "opacity" :
                        oElement.style.filter = "alpha(opacity=" + value+ ")";
                        oElement.style.opacity = value / 100;
						
                        break;
                    default :
                        oElement.style[attr] = value;
                        break
                }
            }
        },
        doMove : function (oElement, oAttr, fnCallBack) {
            var _this = this;
            clearInterval(oElement.timer);
            oElement.timer = setInterval(function () {
                var bStop = true;
                for (var property in oAttr) {
                    var iCur = parseFloat(_this.css(oElement, property));
                    property == "opacity" && (iCur = parseInt(iCur.toFixed(2) * 100));
                    var iSpeed = (oAttr[property] - iCur) / 5;
                    iSpeed = iSpeed > 0 ? Math.ceil(iSpeed) : Math.floor(iSpeed);
                    if (iCur != oAttr[property]) {
                        bStop = false;
                        _this.css(oElement, property, iCur + iSpeed)
                    }
                }
                if (bStop) {
                    clearInterval(oElement.timer);
                    fnCallBack && fnCallBack.apply(_this, arguments)
                }
            }, 10)
        }
    };
    //容器的ID值
	$(document).ready(function() {
	$(".paging").show();
	$(".paging a:first").addClass("active");
	var imageWidth = $(".window").width();
	var imageSum = $(".image_reel img").size();
	var imageReelWidth = imageWidth * imageSum;
	$(".image_reel").css({'width' : imageReelWidth});
	rotate = function(){	
		var triggerID = $active.attr("rel") - 1;
		var image_reelPosition = triggerID * imageWidth; 
		$(".paging a").removeClass('active');
		$active.addClass('active');
		$(".image_reel").animate({ 
			left: -image_reelPosition
		}, 500 );
		
	};
	rotateSwitch = function(){		
		play = setInterval(function(){ 
			$active = $('.paging a.active').next();
			if ( $active.length === 0) { 
							$active = $('.paging a:first');
			}
			rotate();
		}, 3000); 
	};
	
	rotateSwitch();
	$(".image_reel a").hover(function() {
		clearInterval(play);
	}, function() {
		rotateSwitch();
	});	

	$(".paging a").click(function() {	
		$active = $(this);
		clearInterval(play);
		rotate();
		rotateSwitch(); 
		return false;
	});	
	
	$(".paging1").show();
	$(".paging1 a:first").addClass("active1");
	var imageWidth1 = $(".window1").width();
	var imageSum1 = $(".image_reel1 img").size();
	var imageReelWidth1 = imageWidth1 * imageSum1;
	$(".image_reel1").css({'width' : imageReelWidth1});
	rotate1 = function(){	
		var triggerID1 = $active1.attr("rel") - 1;
		var image_reelPosition1 = triggerID1 * imageWidth1; 
		$(".paging1 a").removeClass('active1');
		$active1.addClass('active1');
		$(".image_reel1").animate({ 
			left: -image_reelPosition1
		}, 500 );
		
	};
	rotateSwitch1 = function(){		
		play1 = setInterval(function(){ 
			$active1 = $('.paging1 a.active1').next();
			if ( $active1.length === 0) { 
							$active1 = $('.paging1 a:first');
			}
			rotate1();
		}, 3000); 
	};
	
	rotateSwitch1();
	$(".image_reel1 a").hover(function() {
		clearInterval(play1);
	}, function() {
		rotateSwitch1();
	});	

	$(".paging1 a").click(function() {	
		$active1 = $(this);
		clearInterval(play1);
		rotate1();
		rotateSwitch1(); 
		return false;
	});	
	
});