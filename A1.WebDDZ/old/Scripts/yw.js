$(document).ready(function () {
    $("#login").click(function () {
 
        $.post('/logins', {
            "UserName": $("#ipt-login-id").val(),
            "Password": $("#ipt-login-pwd").val()
        }, function (data, status) {
            if (status == "success") {
                if (data) {
                    var json = jQuery.parseJSON(data);
                    var message = '';
                    switch (json.code) {
                        case 0:
                            location.href = "/index";
                            break;
                        case 1:
                            message = "服务器繁忙,请稍候登录";
                            break;
                        case 2:
                            message = "密码错误";
                            break;
                        case 7:
                            message = "帐号不存在";
                            break;
                        case 13:
                            message = "帐号已经被冻结";
                            break;


                    }

                    $(".server-error").html(message);
                 
                }
            }
        });
     
    });


});