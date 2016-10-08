    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnableGame.aspx.cs" Inherits="GS.Web.Manage.EnableGame" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
             .treeCss td div {height: 20px !important}
    </style>
    <script type="text/javascript">

        function ckqxxz() {
            var tree = document.getElementById("TreeView1").getElementsByTagName("input");
            for (var i = 0; i < tree.length; i++) {
                if (tree[i].type == "checkbox" && tree[i].checked==true) {
                    tree[i].checked = false;
                }
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding:10px;height:400px;overflow-x:scroll;overflow-y:scroll;">
        <span  style="color:red;font-size:12px">注：(本页数据均为禁用的游戏数据,启用的游戏数据不显示)</span>
        <div>
        <asp:TreeView ID="TreeView1" CssClass="treeCss" runat="server" ShowLines="True" Font-Size="Small" ForeColor="Black" >


        </asp:TreeView>
        </div>
        
    </div>
     <div style="text-align:center;padding:20px 0;width:100%;">
         <asp:Button  style="margin:0 20px;" ID="btnenable" runat="server" Text="确定启用" OnClick="btnenable_Click" />
      
         <input type="button" style="margin:0 20px;" onclick="ckqxxz();" value="取消" />
           
    </form>
</body>
</html>
