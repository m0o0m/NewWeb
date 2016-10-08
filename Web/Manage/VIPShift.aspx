<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VIPShift.aspx.cs" Inherits="GS.Web.Manage.VIPShift" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body style="background-color:#ffffff">
    <form id="form1" runat="server">
   
    <div  style="font-size:12px;padding-left:10px;padding-top:1px;padding-right:10px;padding-bottom:10px;text-align:center;vertical-align:central; border:0px;">
    代理帐号：<asp:TextBox ID="txtAgentAccount" runat="server"></asp:TextBox> <asp:Button ID="btnSava" runat="server" Text=" 确 定 " OnClick="btnSava_Click" />
    </div>
    
    </form>
</body>
</html>
