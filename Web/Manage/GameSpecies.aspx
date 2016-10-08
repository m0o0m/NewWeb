<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GameSpecies.aspx.cs" Inherits="GS.Web.Manage.GameSpecies" %>

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
     <span class="sm">游戏类配置：</span>
     <div class="ul">
       <p><span class="th">名称：</span> <asp:TextBox ID="txtTypeName" Text="" runat="server"></asp:TextBox></p>
       <p><span class="th">排序：</span> <asp:TextBox ID="txtSortID"  Text="" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox></p>
       <p><span class="th">图标ID：</span> <asp:DropDownList ID="dropImgID" runat="server">
                              <asp:ListItem Value="0">真人视频</asp:ListItem>
                              <asp:ListItem Value="1">对战</asp:ListItem>
                              <asp:ListItem Value="2">电子</asp:ListItem>
                              <asp:ListItem Value="3">博彩</asp:ListItem>
                          </asp:DropDownList></p>
     </div>
   </div>
   <!--tr-->
   <p class="btn">  <asp:Button ID="btnSubSpecies" Width="80" runat="server" Text=" 确 定 " OnClick="btnSubSpecies_Click" />
      &nbsp; &nbsp; &nbsp; &nbsp;
    <input type="reset" value=" 重 置 " style="width:80px"   id="btnqx"/></p>
 </div>
</div>

    </form>
</body>
</html>
