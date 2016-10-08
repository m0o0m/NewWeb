<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="OnlineProblem.aspx.cs" Inherits="GS.Web.Manage.OnlineProblem" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="/css/AspNetPage.css" rel="stylesheet" />
<%--      <script src="/js/zDialog.js"></script>
    <script src="/js/zDrag.js"></script>--%>
    <script type="text/javascript" src="/js/jquery-1.4.2.min.js"></script>
<script type="text/javascript" src="/js/tipswindown.js"></script>

    <link href="/css/tipswindown.css" rel="stylesheet" />

    <script type="text/javascript">

        //function open3(title, url) {
        //    var diag = new Dialog();
        //    diag.Width = 800;
        //    diag.Height = 600;
        //    diag.Title = title;
        //    diag.URL = url;
        //    diag.show();
        //}
      </script>     
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <form id="form1" runat="server">
      <div id="content-header">
    <div id="breadcrumb"> <a href="#" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a><a href="#" class="tip-bottom">在线问题</a> <a href="#" class="current">在线问题列表</a> </div>
         <h1>在线问题列表</h1>
  </div>

        <div class="container-fluid">

    <hr>
    <div class="row-fluid">
     <div class="widget-box">
          <div class="widget-title"> <span class="icon"> <i class="icon-th"></i> </span>
            <h5>在线问题列表</h5>
          </div>

          <div class="widget-content nopadding">
     

         <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" >
             <HeaderTemplate>

                    <table class="table table-bordered table-striped">
              <thead>
                <tr>
<%--                  <th>提问者昵称</th>--%>
                  <th>提问者账号</th>
                  <th>提问时间</th>
                  <th>提问标题</th>
                  <th>提问内容</th>
                  <th>提问类型</th>
                    <th>解决状态</th>
                    <th>基本操作</th>
                </tr>
              </thead>
             </HeaderTemplate>

             <ItemTemplate>
                    <tbody>
                <tr class="odd gradeX">
<%--                  <td><%# Eval("GUName") %></td>--%>
                  <td><%# Eval("GUAccount") %></td>
                  <td><%# getzh(Eval("CSCTime").ToString(),"1") %></td>
                    <td><%# Eval("CSCTitle") %></td>
                      <td><%# Eval("CSCContent") %></td>
                  <td><%# getzh(Eval("CSCType").ToString(),"2") %></td>
                    <td><%# getzh(Eval("CSCState").ToString(),"3") %></td>
                  <td style="text-align:center">
                       <asp:LinkButton ID="linkdel" CommandArgument='<%# Eval("CSCMainID") %>'  Visible='<%# bool.Parse((Session["Cust"]==null).ToString()) %>'  OnClientClick="return confirm('确定删除？')" CommandName="Del" CssClass="btn btn-danger btn-mini" runat="server"  Text="删除"></asp:LinkButton>
                      |<asp:LinkButton ID="linksearch" CommandArgument='<%# Eval("GUUserID")+","+Eval("CSCMainID") %>'   CommandName="Search" CssClass="btn btn-warning btn-mini" runat="server"  Text="对话查看"></asp:LinkButton>
                  </td>
                 
                </tr>
              
              </tbody>
          

             </ItemTemplate>
             <FooterTemplate>
                   </table>
             </FooterTemplate>

         </asp:Repeater>
           
          </div>
         
    <webdiyer:AspNetPager ID="AspNetPager1"  CssClass="paginator" CurrentPageButtonClass="cpb" runat="server"  FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:AspNetPager>
        </div>
        </div></div>
        </form>

</asp:Content>
