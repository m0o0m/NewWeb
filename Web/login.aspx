<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="GS.Web.Manage.login" %>

<!DOCTYPE html>
<html lang="en">
    
<head>
        <title>后台登录</title><meta charset="UTF-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
		<link rel="stylesheet" href="/css/bootstrap.min.css" />
		<link rel="stylesheet" href="/css/bootstrap-responsive.min.css" />
        <link rel="stylesheet" href="/css/matrix-login.css" />
        <link href="font-awesome/css/font-awesome.css" rel="stylesheet" />
		<link href='http://fonts.googleapis.com/css?family=Open+Sans:400,700,800' rel='stylesheet' type='text/css'>

    </head>
    <body>
                    <div id="loginbox">            
            <form id="loginform" class="form-vertical" runat="server" >
				 <div class="control-group normal_text"> <h3><img src="img/logo.png" alt="Logo" /></h3></div>
                <div class="control-group">
                    <div class="controls">
                        <div class="main_input_box">
                            <span class="add-on bg_lg"><i class="icon-user"></i></span><input type="text" id="username" name="username" placeholder="账号" value="admin" />
                        </div>
                    </div>
                </div>
                <div class="control-group">
                    <div class="controls">
                        <div class="main_input_box">
                            <span class="add-on bg_ly"><i class="icon-lock"></i></span><input type="password" id="password" name="password" placeholder="密码" value="123" />
                        </div>
                    </div>
                    </div>
                <div class="control-group">
                    <div class="controls">
                        <div class="main_input_box">
                        <span class="add-on bg_lg"  > 模 式 </span>
                        <asp:DropDownList style="height:38px; border:0px;display:inline-block; width:77%; line-height:28px;  margin-bottom:3px;" ID="dropModel" runat="server">
                            <asp:ListItem Value="0">管理模式</asp:ListItem>
                            <asp:ListItem Value="1">代理模式</asp:ListItem>
                            <asp:ListItem Value="2">客服模式</asp:ListItem>
                        </asp:DropDownList>
                        </div>
                    </div>
                </div>
        
                <div class="form-actions">
                    <span class="pull-right"> <asp:Button ID="btnlogin" class="btn btn-success"  runat="server" Text="登录" OnClick="btnlogin_Click" /> </span>
                </div>
            </form>
        </div>
        
        <script src="/js/jquery.min.js"></script>  
        <script src="/js/matrix.login.js"></script> 
     
    </body>
  
</html>
