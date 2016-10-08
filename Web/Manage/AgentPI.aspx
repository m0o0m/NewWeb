<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="AgentPI.aspx.cs" Inherits="GS.Web.Manage.AgentPI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

         <form id="form1" runat="server">
      <div id="content-header">
    <div id="breadcrumb"> <a href="#" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a><a href="#" class="tip-bottom">代理</a> <a href="#" class="current">代理个人信息</a> </div>
         <h1>代理个人信息</h1>
  </div>
           

        <div class="container-fluid">

    <hr>
    <div class="row-fluid">
     <div class="widget-box">
          <div class="widget-title"> <span class="icon"> <i class="icon-th"></i> </span>
            <h5>代理个人信息</h5>
          </div>

          <div class="widget-content nopadding">
     

         <asp:Repeater ID="Repeater1" runat="server">
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
              
                </tr>
              </thead>
             </HeaderTemplate>

             <ItemTemplate>
                    <tbody>
                <tr class="odd gradeX">
                  <td><%# Eval("AgentAccount") %></td>
                  <td><%# Eval("AgentName") %></td>
                  <td><%# Eval("AgentPasswd") %></td>
                    <td><%# Eval("AgentQQ") %></td>
                      <td><%# Eval("AgentTel") %></td>
                  <td><%# Eval("AgentEmail") %></td>
                        <td><%# Eval("RegisterTime") %></td>
                  <td><%# Eval("LoginIP") %></td>
                    <td><%# Eval("LoginTime") %></td>
                  </td>
                 
                </tr>
              
              </tbody>
          

             </ItemTemplate>
             <FooterTemplate>
                   </table>
             </FooterTemplate>

             

         </asp:Repeater>
           
          </div>
         
        </div>
        </div></div>
        </form>



</asp:Content>
