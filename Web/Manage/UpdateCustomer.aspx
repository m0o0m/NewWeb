<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="UpdateCustomer.aspx.cs" Inherits="GS.Web.Manage.UpdateCustomer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

      <style type="text/css">
        .auto-style1 {
            padding-top:10px !important;
        }
         </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
           <form id="form1"   runat="server">
    <div id="content-header">
  <div id="breadcrumb"> <a href="index.html" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a> <a href="#" class="tip-bottom">客服管理</a> <a href="#" class="current">修改客服</a> </div>
  <h1>修改客服</h1>
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
                    <td>客服帐号：</td>
                    <td>
                        <asp:TextBox ReadOnly="true" ID="txtCustomerAccount" onkeyup="value=value.replace(/[^\w\.\/]/ig,'')"   runat="server"></asp:TextBox> <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtCustomerAccount" runat="server" ErrorMessage="账号不能为空"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>客服昵称：</td>
                    <td>
                        <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>                         
                    </td>
                </tr>
                <tr>
                    <td>客服密码：</td>
                    <td>
                        <asp:TextBox ID="txtCustomerPwd" runat="server" TextMode="Password" > </asp:TextBox>
                          <span style="color:red">(注：修改密码请输入,保持原密码则不用输入)</span>
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
