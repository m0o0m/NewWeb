$(document).ready(()->
    $('#form-pay').submit (e)->
        userId = $('.user-id').val()
        userIdAgain = $('.user-id-again').val()
        if( userId == '' || userIdAgain == '' )
            alert '账号不能为空'
            return false
        if( userId != userIdAgain )
            alert '两次输入的账号不一致'
            return false
    # 自定义select控件
    $('.js-custom-select ul li a').on 'click',()->
        $this = $(this)
        $thisCustomSelect = $this.closest('.js-custom-select')
        $inputEle = $thisCustomSelect.find('input.val')
        return false if( $this.closest('li').hasClass 'active' )
        $thisCustomSelect.find('ul li').removeClass 'active'
        $this.closest('li').addClass 'active'
        $inputEle.val $this.attr 'data-val'

)