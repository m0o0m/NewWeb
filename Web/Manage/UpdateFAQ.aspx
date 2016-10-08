<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="UpdateFAQ.aspx.cs" Inherits="GS.Web.Manage.UpdateFAQ" %>
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
  <div id="breadcrumb"> 
        <a href="index.html" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a> <a href="#" class="tip-bottom">知识库</a> <a href="#" class="current">修改FAQ</a> </div>
  <h1>修改FAQ</h1>
</div>
  <div class="container-fluid">
    <hr>
      
    <div class="row-fluid">
       
    <div class="widget-box">
        <div class="widget-title">
            <span class="icon"><i class="icon-align-justify"></i></span>
            <h5>修改FAQ</h5>
        </div>
        <div class="widget-content ">
   
          <table class="auto-style1">
                <tr>
                    <td>标题：</td>
                    <td>
                        <asp:TextBox ID="txtfaqtitle"    runat="server"></asp:TextBox> <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtfaqtitle" runat="server" ErrorMessage="标题不能为空"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>内容：</td>
                    <td>
                        <asp:TextBox   Width="555"   ID="txtfaqcontent" runat="server" TextMode="MultiLine" Height="330"></asp:TextBox> <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtfaqcontent" runat="server" ErrorMessage="内容不能为空"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>类型：</td>
                    <td>
                        <asp:DropDownList ID="droptype" runat="server">
                            <asp:ListItem Value="1">游戏问题</asp:ListItem>
                            <asp:ListItem Value="2">充值问题</asp:ListItem>
                            <asp:ListItem Value="3">兑换问题</asp:ListItem>
                            <asp:ListItem Value="4">其他问题</asp:ListItem>
                        </asp:DropDownList>
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
