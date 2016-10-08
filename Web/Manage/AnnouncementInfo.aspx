<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="AnnouncementInfo.aspx.cs" Inherits="GS.Web.Manage.AnnouncementInfo" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="/css/AspNetPage.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

            <form id="form1" runat="server">
      <div id="content-header">
    <div id="breadcrumb"> <a href="#" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a><a href="#" class="tip-bottom">系统设置</a> <a href="#" class="current">公告信息</a>  </div>
         <h1>公告信息</h1>
  </div>
           

        <div class="container-fluid">

            <div class="row-fluid">
     <div class="widget-box">
  
          <div class="widget-title"> <span class="icon"> <i class="icon-th"></i> </span>
            <h5>公告信息列表</h5>
          </div>
         
          <div class="widget-content nopadding">
     

    


         <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
             <HeaderTemplate>

                    <table   class="table table-bordered table-striped">
              <thead>
                <tr>
                  <th>公告标题</th>
                  <th>公告内容</th>
        
                    <th>发表时间</th>
                      <th>公告类型</th>
                    <th>基本操作</th>
                </tr>
              </thead>
             </HeaderTemplate>

             <ItemTemplate>
                    <tbody>
                <tr class="odd gradeX">
               
                        <td><%# Eval("ATitle") %></td>
           
                      <td><%# Eval("AContent") %></td>
                      <td><%# Eval("ATime") %></td>
                        <td><%#GetTypeName(Eval("AType").ToString()) %></td>
               
                  
                    <td style="text-align:center"> <asp:LinkButton ID="linkdel" CommandArgument='<%# Eval("AID") %>'  OnClientClick="return confirm('确定删除？')" CommandName="Del" CssClass="btn btn-danger btn-mini" runat="server"  Text="删除"></asp:LinkButton>
                      | <asp:LinkButton ID="linkupdate"  CommandArgument='<%# Eval("AID") %>' Text=" 编 辑 " CssClass="btn btn-primary btn-mini"  CommandName="Update" runat="server"></asp:LinkButton>
              

                  </td>
                 
                </tr>
              
              </tbody>
          

             </ItemTemplate>
             <FooterTemplate>
                
         <tbody>                <tr >
                     <td  colspan="5" style="text-align:center;">

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
