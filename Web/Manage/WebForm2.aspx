<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="GS.Web.Manage.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <script type="text/javascript" src="/js/jquery-1.4.2.min.js"></script>
<script type="text/javascript" src="/js/tipswindown.js"></script>
    <script  type="text/javascript">
        //tipsWindown.close = function () { $("#windownbg").remove(); $("#windown-box").fadeOut("slow", function () { $(this).remove(); }); }


        function closes() {
            showselect('t123_');
            $("#windownbg").remove();
            $("#windown-box").fadeOut("slow", function () { $(this).remove(); })

            hideselect('t123_');
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" OnClientClick="return close();" runat="server" Text="Button" />

        <input type="button" onclick="closes();" value="dsada" />
    </div>

        <asp:TreeView ID="TreeView1" runat="server">
            <Nodes>
                <asp:TreeNode Text="新建节点" Value="新建节点"></asp:TreeNode>
                <asp:TreeNode Text="新建节点" Value="新建节点"></asp:TreeNode>
                <asp:TreeNode Text="新建节点" Value="新建节点">
                    <asp:TreeNode Text="新建节点" Value="新建节点">
                        <asp:TreeNode Text="新建节点" Value="新建节点">
                            <asp:TreeNode Text="新建节点" Value="新建节点">
                                <asp:TreeNode Text="新建节点" Value="新建节点"></asp:TreeNode>
                            </asp:TreeNode>
                        </asp:TreeNode>
                    </asp:TreeNode>
                </asp:TreeNode>
                <asp:TreeNode Text="新建节点" Value="新建节点"></asp:TreeNode>
                <asp:TreeNode Text="新建节点" Value="新建节点"></asp:TreeNode>
            </Nodes>


        </asp:TreeView>



        <div  class="panel">
 <p class="title">
     <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" />
     游戏种类</p>
 <div class="main">
  <div class="tr">
     <span class="sm">游戏服务器配置：</span>
     <ul>
       <li><span class="th">名称：</span><input type="text" /></li>
       <li><span class="th">排序：</span><input type="text" /></li>
       <li><span class="th">图标ID：</span><input type="text" /></li>
     </ul>
   </div>
   <!--tr-->
   <p class="btn"><button>确定</button><button>取消</button></p>
 </div>
</div>
    </form>
</body>
</html>
