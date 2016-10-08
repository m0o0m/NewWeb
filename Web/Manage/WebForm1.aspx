<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="GS.Web.Manage.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>动软卓越首页</title>
    <style type="text/css">
*{margin:0;padding:0;list-style-type:none;}
a,img{border:0;}
body{font:12px/180% Arial, Helvetica, sans-serif;}
label{cursor:pointer;}
.democode{width:400px;margin:30px auto 0 auto;line-height:24px;}
.democode h2{font-size:14px;color:#3366cc;height:28px;}
.agree{margin:40px auto;width:400px;font-size:16px;font-weight:800;color:#3366cc;}
.mainlist{padding:10px;}
.mainlist li{height:28px;line-height:28px;font-size:12px;}
.mainlist li span{margin:0 5px 0 0;font-family:"宋体";font-size:12px;font-weight:400;color:#ddd;}
.btnbox{text-align:center;height:30px;padding-top:10px;background:#ECF9FF;}

#windownbg{display:none;position:absolute;width:100%;height:100%;background:#000;top:0;left:0;}
#windown-box{position:fixed;_position:absolute;border:5px solid #E9F3FD;background:#FFF;text-align:left;}
#windown-title{position:relative;height:30px;border:1px solid #A6C9E1;overflow:hidden;background:url(images/tipbg.png) 0 0 repeat-x;}
#windown-title h2{position:relative;left:10px;top:5px;font-size:14px;color:#666;}
#windown-close{position:absolute;right:10px;top:8px;width:10px;height:16px;text-indent:-10em;overflow:hidden;background:url(images/tipbg.png) 100% -49px no-repeat;cursor:pointer;}
#windown-content-border{position:relative;top:-1px;border:1px solid #A6C9E1;padding:5px 0 5px 5px;}
#windown-content img,#windown-content iframe{display:block;}
#windown-content .loading{position:absolute;left:50%;top:50%;margin-left:-8px;margin-top:-8px;}
</style>
<script type="text/javascript" src="/js/jquery-1.4.2.min.js"></script>
<script type="text/javascript" src="/js/tipswindown.js"></script>

    <link href="/css/tipswindown.css" rel="stylesheet" />
    <script>

        function GetSession() {
            var val = '<%=strSession %>';
            if (val != null) {
                alert(val);
            } else {
                alert("没数据")
            }
        }


        //window.onload = function s() {
        //    var arr = [1, 23, 34, 5];
        //    var strArr = arr.toString();
        //    document.write(strArr);
        //    var getArr = strArr.split(",");
        //    for (var i = 0; i < getArr.length; i++) {
        //        document.write(getArr[i]);
        //    }
        //}
    </script>
    
</head>
<body>
    <form id="form1" runat="server" oninit="form1_Init">
    <div>
        <asp:Button ID="Button1" runat="server" Text="Button" />
        <input type="submit" id="dasda" onclick="GetSession()"  value="大厦大厦" runat="server"/>
            <input type="button" value="测试" onclick="showTipsWindowns('dasd', 'dasd', 800, 900)";
    </div>

    <div id="dasd">

        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

        <asp:Button ID="Button2" runat="server" Text="Button" />

    </div>
    </form>
</body>
</html>
