<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="UpdateAnn.aspx.cs" Inherits="GS.Web.Manage.UpdateAnn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
            <style type="text/css">
        .auto-style1 {
            padding-top:10px !important;
        }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          <form id="form1" runat="server">
      <div id="content-header">
  <div id="breadcrumb"> <a href="index.html" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a> <a href="#" class="tip-bottom">公告管理</a> <a href="#" class="current">修改公告</a> </div>
  <h1>修改公告</h1>
</div>
   
     <div class="container-fluid">
    <hr>
    <div class="row-fluid">
        
      <div class="widget-title">
            <span class="icon"><i class="icon-align-justify"></i></span>
            <h5></h5>
        </div>
       <div class="widget-box">
        <div class="widget-content  ">
   
          <table class="auto-style1 ">
                <tr>
                    <td>公告标题：</td>
                    <td>
                        <asp:TextBox ID="txtATitle"     runat="server"></asp:TextBox> 
                    </td>
                </tr>
                <tr>
                    <td>公告内容：</td>
                    <td>
                        <asp:TextBox ID="txtAContent" Height="60" runat="server" TextMode="MultiLine"></asp:TextBox> 
                    </td>
                </tr>
         <tr>
                    <td>公告类型：</td>
                    <td>
                        
                        <asp:DropDownList ID="droptype" runat="server">
                            <asp:ListItem Value="0">游戏普通</asp:ListItem>
                            <asp:ListItem Value="1">游戏左栏置顶</asp:ListItem>
                             <asp:ListItem Value="2">游戏滚动</asp:ListItem>
                             <asp:ListItem Value="3">代理公告</asp:ListItem>

                        </asp:DropDownList>


                        &nbsp;</td>
                </tr>

                

                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSava" CssClass="btn btn-success" runat="server" Text="保 存" OnClick="btnSava_Click"  /></td>
                </tr>
            </table>
    
    </div>
           </div>
        </div>
           </div>
    </form>


</asp:Content>
