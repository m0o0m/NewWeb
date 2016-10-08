<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="AddManage.aspx.cs" Inherits="GS.Web.Manage.AddManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="/css/matrix-style.css" rel="stylesheet" />
  
  
    <style type="text/css">
        .auto-style1 {
            padding-top:10px !important;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1"   runat="server">
    <div id="content-header">
  <div id="breadcrumb"> <a href="index.html" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a> <a href="#" class="tip-bottom">管理员</a> <a href="#" class="current">添加管理员</a> </div>
  <h1>添加管理员</h1>
</div>
  <div class="container-fluid">
    <hr>
    <div class="row-fluid">
       
    <div class="widget-box">
        <div class="widget-title">
            <span class="icon"><i class="icon-align-justify"></i></span>
            <h5></h5>
        </div>
        <div class="widget-content ">
   
          <table class="auto-style1">
                <tr>
                    <td>管理员帐号：</td>
                    <td>
                        <asp:TextBox ID="txtAdminAccount" onkeyup="value=value.replace(/[^\w\.\/]/ig,'')"   runat="server"></asp:TextBox> <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtAdminAccount" runat="server" ErrorMessage="账号不能为空"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>管理员昵称：</td>
                    <td>
                        <asp:TextBox ID="txtAdminName" runat="server"></asp:TextBox> <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtAdminName" runat="server" ErrorMessage="昵称不能为空"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>管理员密码：</td>
                    <td>
                        <asp:TextBox ID="txtAdminPasswd" runat="server" TextMode="Password" > </asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtAdminPasswd" runat="server" ErrorMessage="密码不能为空"></asp:RequiredFieldValidator>
                    </td>

                </tr>
       
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSava" CssClass="btn btn-success" runat="server" Text="保 存" OnClick="btnSava_Click" /></td>
                </tr>
            </table>

</div>
             </div>        
        </div>
    </div>
        </form>
</asp:Content>
