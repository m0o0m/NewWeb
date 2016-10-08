$(document).ready(function(){
	/*
 * jQuery placeholder, fix for IE6,7,8,9
 * @author JENA
 * @since 20131115.1504
 * @website ishere.cn
 */
var JPlaceHolder = {
    //检测
    _check : function(){
        return 'placeholder' in document.createElement('input');
    },
    //初始化
    init : function(){
        if(!this._check()){
            this.fix();
        }
    },
    //修复
    fix : function(){
        jQuery(':input[placeholder]').each(function(index, element) {
            var self = $(this), txt = self.attr('placeholder');
            self.wrap($('<div class="ie-placeholder-wrap"></div>').css({position:'relative', zoom:'1', border:'none', background:'none', padding:'none', margin:'none'}));
            var pos = self.position(), h = parseInt(self.outerHeight(true)), paddingleft = self.css('padding-left');
            var holder = $('<span class="ie-placeholder"></span>').text(txt).css({position:'absolute', left:pos.left, top:pos.top, height:h+'px', lineHeight:h+'px', paddingLeft:paddingleft, color:'#aaa'}).appendTo(self.parent());
            self.focusin(function(e) {
                holder.hide();
            }).focusout(function(e) {
                if(!self.val()){
                    holder.show();
                }
            });
            holder.on('click',function (e) {
            	$(this).hide();
                self.focus();
            });
        });
    }
};
//执行
jQuery(function(){
    JPlaceHolder.init();    
});



//导航条切换
var menu_href = location.href.match(/\/\/.+?\/([\w-]*)/)[1];
    if (menu_href == ""){
        $(".global-header .main-nav li:first").addClass("active");
    }
    else{
       $(".global-header .main-nav li a[href$='" + menu_href + "']").closest('li').addClass("active");
    }

});