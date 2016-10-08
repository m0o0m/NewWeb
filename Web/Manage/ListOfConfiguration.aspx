<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="ListOfConfiguration.aspx.cs" Inherits="GS.Web.Manage.ListOfConfiguration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/tipswindown.css" rel="stylesheet" />
    <script src="/js/jquery-1.4.2.min.js"></script>
    <script src="/js/tipswindown.js"></script>

          <style type="text/css">
              .auto-style2 {
                  width: 100%;
              }

              .treeCss td div {height: 20px !important}
          </style>


    <script type="text/javascript">

        function yesno() {
            if (confirm("是否将此信息删除?")) {
                return true;
            }
            else
                return false;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1"  runat="server">
        <div id="content-header">
    <div id="breadcrumb"> <a href="#" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a><a href="#" class="tip-bottom">系统设置</a> <a href="#" class="current">游戏初始配置</a>  </div>
         <h1>游戏初始配置</h1>
  </div>

      <div class="container-fluid">

            <div class="row-fluid">
     <div class="widget-box">
  
         
         
          <div class="widget-content nopadding">

              <div class="wrap">
  <span class="title">游戏列表配置</span>
  <div class="content">
      <asp:TreeView CssClass="treeCss" SelectedNodeStyle-BackColor="#0099ff" ID="TreeView1" runat="server" ShowLines="True" OnTreeNodeCollapsed="TreeView1_TreeNodeCollapsed" OnTreeNodeExpanded="TreeView1_TreeNodeExpanded">
         
       
      </asp:TreeView>
  </div>
  <!--content-->
  <div class="btns">
    <p class="one">
        <asp:Button ID="btnAddGameSpecies" Width="88"   runat="server" Text="新增游戏种类" OnClick="btnAddGameSpecies_Click" />
    <%--    <input style="width:88px" type="button" onclick="showTipsWindowns('dsa', 'gameSpecies', 800, 900);" id="btnAddGameSpecies" value="新增游戏种类" />--%>
            <%--   <input style="width:88px" type="button" id="btnAddGameType" value="新增游戏类型" />--%>
        <asp:Button ID="btnAddGameType" Width="88"  runat="server" Text="新增游戏类型" OnClick="btnAddGameType_Click" />

          <%--<input style="width:88px" type="button" id="btnAddGameTypeServe" value="新增游戏服务" />--%>
         <asp:Button ID="btnAddGameTypeServe" Width="88"   runat="server" Text="新增游戏服务" OnClick="btnAddGameTypeServe_Click"  />

        <asp:Button ID="btnenable" runat="server" Text=" 启 用 " OnClick="btnenable_Click" />
        <asp:Button ID="btndisable" runat="server" Text=" 禁 用 " OnClick="btndisable_Click" />

    <%--    <button>新增游戏种类</button><button>新增游戏类型</button><button>新增游戏服务</button><button>停用</button><button>启用</button>--%></p>
      <p class="one">      
        
    <asp:Button ID="btndelnode" Width="88"   runat="server" Text="删除节点" OnClick="btndelnode_Click" />
<%-- <input style="width:88px" type="button" id="btndelnode" value="删除节点" />.--%>
            <asp:Button ID="btnnodepz" Width="88"   runat="server" Text="节点配置" OnClick="btnnodepz_Click"  />
      
       <%--   <button>刷新列表</button>--%>
          <button>生成游戏服务器脚本</button>

      </p>
<%--    <p class="two"><button>确定 </button></p>--%>
  </div>
  <!--btns-->
</div>
<!--warp-->
<div class=""></div>

            
              </div>

         </div>
  </div>
   
</div>
   
       <%--   <div  id="gameSpecies" style="display:none"  >
<%-- <p class="title"></p>
 <div class="main">
   <div class="tr">
     <span class="sm">游戏种类配置：</span>
     <div class="ul">
       <p><span class="th">名称：</span><input type="text"  /></p>
       <p><span class="th">排序：</span><input type="text" /></p>
       <p><span class="th">最大人数：</span><input type="text" /></p>
     </div>
  
   </div><!--tr-->
  <div  style="text-align:center">

      <asp:Button ID="btnSubSpecies" Width="80" runat="server" Text=" 确 定 " />
      &nbsp; &nbsp; &nbsp; &nbsp;
    <input type="reset" value="取消" style="width:80px"   id="btnqx"/>
     --%>

       <%--
   <asp:Panel ID="Panel1" runat="server">
              <table class="auto-style2">
                  <tr>
                      <td>名称：</td>
                      <td>
                          <asp:TextBox ID="txtTypeName"  Text="" runat="server"></asp:TextBox>
                      </td>
                  </tr>
                  <tr>
                      <td>排序：</td>
                      <td>
                          <asp:TextBox ID="txtSortID" Text="" runat="server"></asp:TextBox>
                      </td>
                  </tr>
                  <tr>
                      <td>图标ID：</td>
                      <td>
                          <asp:DropDownList ID="dropImgID" runat="server">
                              <asp:ListItem Value="0">真人视频</asp:ListItem>
                              <asp:ListItem Value="1">对战</asp:ListItem>
                              <asp:ListItem Value="2">电子</asp:ListItem>
                              <asp:ListItem Value="3">博彩</asp:ListItem>
                          </asp:DropDownList>
                      </td>
                  </tr>
                  <tr>
                      <td colspan="2" style="text-align:center"> 
                           <asp:Button ID="btnSubSpecies" Width="80" runat="server" Text=" 确 定 " OnClick="btnSubSpecies_Click" />
      &nbsp; &nbsp; &nbsp; &nbsp;
    <input type="reset" value="取消" style="width:80px"   id="btnqx"/></td>
                  </tr>
              </table>



</asp:Panel>
</div>--%>



</form>
</asp:Content>
