<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="Management.aspx.cs" Inherits="GS.Web.Manage.Management" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/AspNetPage.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
      <div id="content-header">
    <div id="breadcrumb"> <a href="#" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a><a href="#" class="tip-bottom">管理员</a> <a href="#" class="current">管理员信息</a> </div>
         <h1>管理员信息</h1>
  </div>

        <div class="container-fluid">

    <hr>
    <div class="row-fluid">
     <div class="widget-box">
          <div class="widget-title"> <span class="icon"> <i class="icon-th"></i> </span>
            <h5>管理员信息列表</h5>
          </div>

          <div class="widget-content nopadding">
     

         <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
             <HeaderTemplate>

                    <table class="table table-bordered table-striped">
              <thead>
                <tr>
                  <th>管理员帐号</th>
                  <th>管理员昵称</th>
                  <th>管理员密码</th>
                  <th>创建时间</th>
                  <th>基本操作</th>
                </tr>
              </thead>
             </HeaderTemplate>

             <ItemTemplate>
                    <tbody>
                <tr class="odd gradeX">
                  <td><%# Eval("AdminAccount") %></td>
                  <td><%# Eval("AdminName") %></td>
                  <td><%# Eval("AdminPasswd") %></td>
                    <td><%# Eval("OperDate") %></td>
                  <td style="text-align:center"> <asp:LinkButton ID="linkdel" CommandArgument='<%# Eval("AdminID") %>'  OnClientClick="return confirm('确定删除？')" CommandName="Del" CssClass="btn btn-danger btn-mini" runat="server"  Text="删除"></asp:LinkButton>
                      | <asp:LinkButton ID="linkupdate"  CommandArgument='<%# Eval("AdminID") %>' Text=" 编 辑 " CssClass="btn btn-primary btn-mini"  CommandName="Update" runat="server"></asp:LinkButton>

                  </td>
                 
                </tr>
              
              </tbody>
          

             </ItemTemplate>
             <FooterTemplate>
                   </table>
             </FooterTemplate>

         </asp:Repeater>
           
          </div>

         
    <webdiyer:AspNetPager ID="AspNetPager1"  CssClass="paginator" CurrentPageButtonClass="cpb" runat="server" PageSize="10" OnPageChanged="AspNetPager1_PageChanged"   FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页"></webdiyer:AspNetPager>
        </div>
        </div></div>
        </form>
</asp:Content>
