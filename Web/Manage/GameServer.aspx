<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GameServer.aspx.cs" Inherits="GS.Web.Manage.GameServer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="/css/style.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     <div class="panel">
 <p class="title"></p>
 <div class="main">
  <div class="tr">
     <span class="sm">游戏服务器配置：</span>
     <div class="ul">
       <p><span class="th">名称：</span> <asp:TextBox ID="txtServerName" Text="" runat="server"></asp:TextBox></p>
       <p><span class="th">排序：</span> <asp:TextBox ID="txtSortID"  onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" Text="" runat="server"></asp:TextBox></p>
       <p><span class="th">最大人数：</span> <asp:TextBox ID="txtMaxUserCount" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"  Text="" runat="server"></asp:TextBox></p>
     </div>
   </div>
   <!--tr-->
   <p class="btn">  <asp:Button ID="btnSubServer" Width="80" runat="server" Text=" 确 定 " OnClick="btnSubServer_Click"  />
      &nbsp; &nbsp; &nbsp; &nbsp;
    <input type="reset" value=" 重 置 " style="width:80px"   id="btnqx"/></p>
 </div>
</div>

    </form>
</body>
</html>
