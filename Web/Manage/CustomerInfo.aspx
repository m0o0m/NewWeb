<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="CustomerInfo.aspx.cs" Inherits="GS.Web.Manage.CustomerInfo" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="/css/AspNetPage.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

       <form id="form1" runat="server">
      <div id="content-header">
    <div id="breadcrumb"> <a href="#" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a><a href="" class="tip-bottom">客服管理</a>  <a href="#" class="current">客服信息</a> </div>
         <h1>客服信息</h1>
  </div>
           

        <div class="container-fluid">

    <hr>
<div >客服帐号： <asp:TextBox ID="txtAgentAccount" runat="server"></asp:TextBox>
                &nbsp;
                <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btn btn-info" OnClick="btnSearch_Click" /></div><div class="row-fluid">
     <div class="widget-box">
     
          <div class="widget-title"> <span class="icon"> <i class="icon-th"></i> </span>
            <h5>客服信息列表</h5>
          </div>
         
          <div class="widget-content nopadding">
     

         <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
             <HeaderTemplate>

                    <table class="table table-bordered table-striped">
              <thead>
                <tr>
                  <th>客服账号</th>
                  <th>客服密码</th>
                  <th>客服昵称</th>
                  <th>客服状态</th>
                  <th>创建时间</th>
                
                    <th>基本操作</th>
                </tr>
              </thead>
             </HeaderTemplate>

             <ItemTemplate>
                    <tbody>
                <tr class="odd gradeX">
                 
                    <td><%# Eval("CustomerAccount") %></td>
                      <td><%# Eval("CustomerPwd") %></td>
                      <td><%# Eval("CustomerName") %></td>
                        <td><%# GetState(Eval("CustomerState").ToString()) %></td>
                 
                    <td><%# Eval("CreateDate") %></td>
                 
                    <td style="text-align:center"> <asp:LinkButton ID="linkdel" CommandArgument='<%# Eval("CustomerID") %>'  OnClientClick="return confirm('确定删除？')" CommandName="Del" CssClass="btn btn-danger btn-mini" runat="server"  Text="删除"></asp:LinkButton>
                      | <asp:LinkButton ID="linkupdate"  CommandArgument='<%# Eval("CustomerID") %>' Text=" 编 辑 " CssClass="btn btn-primary btn-mini"  CommandName="Update" runat="server"></asp:LinkButton>

              

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
