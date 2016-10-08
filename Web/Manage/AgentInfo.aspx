<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="AgentInfo.aspx.cs" Inherits="GS.Web.Manage.AgentInfo" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/AspNetPage.css" rel="stylesheet" />
        <script type="text/javascript" src="/js/jquery-1.4.2.min.js"></script>
<script type="text/javascript" src="/js/tipswindown.js"></script>

    <link href="/css/tipswindown.css" rel="stylesheet" />
<%--    <script src="/js/zDialog.js"></script>
    <script src="/js/zDrag.js"></script>
    <script type="text/javascript">

    function open3(title,url)
     {
	var diag = new Dialog();
	diag.Width = 380;
	diag.Height = 200;
	diag.Title = title;
	diag.URL = url;
	diag.show();
    }
      </script>  --%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <form id="form1" runat="server">
      <div id="content-header">
    <div id="breadcrumb"> <a href="#" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a><a href="AgentInfo.aspx" class="tip-bottom">代理</a> <%=navlist %> </div>
         <h1>代理信息</h1>
  </div>
           

        <div class="container-fluid">

    <hr/>
<div >代理帐号： <asp:TextBox ID="txtAgentAccount" runat="server"></asp:TextBox>
                &nbsp;
                <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btn btn-info" OnClick="btnSearch_Click" /></div>
            
            <div class="row-fluid">
     <div class="widget-box">
         <span style="color:red">注：点击代理帐号，可以查看下级代理信息</span>
          <div class="widget-title"> <span class="icon"> <i class="icon-th"></i> </span>
            <h5>代理信息列表</h5>
          </div>
         
          <div class="widget-content nopadding">
     

         <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
             <HeaderTemplate>

                    <table class="table table-bordered table-striped">
              <thead>
                <tr>
                  <th>代理帐号</th>
                  <th>代理名称</th>
                  <th>代理密码</th>
                  <th>代理QQ</th>
                  <th>代理手机</th>
                    <th>代理邮箱</th>
                    <th>注册时间</th>
                    <th>登录时间</th>
                    <th>登录IP</th>
                      <th>在线状态</th>
                    <th>基本操作</th>
                </tr>
              </thead>
             </HeaderTemplate>

             <ItemTemplate>
                    <tbody>
                <tr class="odd gradeX">
                  <td><a href="AgentInfo.aspx?id=<%# Eval("AgentID") %>"> <%# Eval("AgentAccount") %></a></td>
                        <td><%# Eval("AgentName") %></td>
                    <td><%# Eval("AgentPasswd") %></td>
                    <td><%# Eval("AgentQQ") %></td>
                      <td><%# Eval("AgentTel") %></td>
                      <td><%# Eval("AgentEmail") %></td>
                        <td><%# Eval("RegisterTime") %></td>
                 
                    <td><%# Eval("LoginTime") %></td>
                     <td><%# Eval("LoginIP") %></td>
                     <td><%# GetOnlineState(Eval("OnlineState").ToString()) %></td>
                    <td style="text-align:center"> <asp:LinkButton ID="linkdel" CommandArgument='<%# Eval("AgentID") %>'  OnClientClick="return confirm('确定删除？')" CommandName="Del" CssClass="btn btn-danger btn-mini" runat="server"  Text="删除"></asp:LinkButton>
                      | <asp:LinkButton ID="linkupdate"  CommandArgument='<%# Eval("AgentID") %>' Text=" 编 辑 " CssClass="btn btn-primary btn-mini"  CommandName="Update" runat="server"></asp:LinkButton>
                       | <asp:LinkButton ID="linkagshift" Text="代理转移" CommandArgument='<%# Eval("AgentID") %>' CommandName="AgShift"  CssClass="btn btn-warning btn-mini" runat="server"></asp:LinkButton>
              

                  </td>
                 
                </tr>
              
              </tbody>
          

             </ItemTemplate>
             <FooterTemplate>
                
         <tbody>                <tr >
                     <td  colspan="11" style="text-align:center;">

  <asp:Label ID="lblEmpty"     
     
Text="暂无数据!" Font-Bold="true" ForeColor="Red"  Font-Size="Large" runat="server"     
     
Visible='<%# bool.Parse((Repeater1.Items.Count==0).ToString())%>'>      
     
</asp:Label> 
                     </td>


                 </tr>
                 </tbody>

                   </table>


        
    
             </FooterTemplate>
             
         </asp:Repeater>
           
          </div>
         
    <webdiyer:aspnetpager ID="AspNetPager1"  CssClass="paginator" CurrentPageButtonClass="cpb" runat="server" PageSize="10"    FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页" OnPageChanged="AspNetPager1_PageChanged"></webdiyer:aspnetpager>
        </div>
        </div></div>
        </form>
</asp:Content>
