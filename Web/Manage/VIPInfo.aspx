<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="VIPInfo.aspx.cs" Inherits="GS.Web.Manage.VIPInfo" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/AspNetPage.css" rel="stylesheet" />

<script type="text/javascript" src="/js/jquery-1.4.2.min.js"></script>
<script type="text/javascript" src="/js/tipswindown.js"></script>

    <link href="/css/tipswindown.css" rel="stylesheet" />
<script type="text/javascript">
    /*
    *弹出本页指定ID的内容于窗口
    *id 指定的元素的id
    *title:	window弹出窗的标题
    *width:	窗口的宽,height:窗口的高
    */
    //function showTipsWindown(title, url, width, height) {
    //    tipsWindown(title, "iframe:" + url, width, height, "true", "", "true", "leotheme");

    //}
   

        //function open3(title, url) {
        //    var diag = new Dialog();
        //    diag.Width = 380;
        //    diag.Height = 200;
        //    diag.Title = title;
        //    diag.URL = url;
        //    diag.show();
        //}
      </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <form id="form1" runat="server">
      <div id="content-header">
    <div id="breadcrumb"> <a href="#" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a><a href="AgentInfo.aspx" class="tip-bottom">会员管理</a> <a href="#" class="current">会员信息</a>  </div>
         <h1>会员信息</h1>
  </div>
           

        <div class="container-fluid">
           
    <hr>
    <div class="row-fluid">
    
     <div class="widget-box">
      
          <div class="widget-title"> <span class="icon"> <i class="icon-th"></i> </span>
            <h5>会员信息列表</h5>
          </div>
         
          <div class="widget-content nopadding">
       

         <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
             <HeaderTemplate>

                    <table class="table table-bordered table-striped"   >
              <thead>
                <tr>
                  <th>帐号</th>
                  <th>密码</th>
                  <th>昵称</th>
                  <th>邮箱</th>
                  <th>电话</th>
                    <th>姓名</th>
                    <th>身份证号码</th>
                    <th>生日</th>
                    <th>性别</th>
                      <th>地址</th>
                    <th>签名</th>
                     <th>所属代理</th>
                    <th>基本操作</th>
                </tr>
              </thead>
             </HeaderTemplate>

             <ItemTemplate>
                    <tbody>
                <tr class="odd gradeX">
                  
                        <td><%# Eval("GUAccount") %></td>
                    <td><%# Eval("GUPasswd") %></td>
                    <td><%# Eval("GUName") %></td>
                      <td><%# Eval("GUEmail") %></td>
                      <td><%# Eval("GUTel") %></td>
                        <td><%# Eval("GUExtend_RealName") %></td>
                 
                    <td><%# Eval("GUExtend_IDCardNo") %></td>
                     <td><%# getInfo(Eval("GUExtend_Birthday").ToString(),"1") %></td>
                      <td><%# getInfo(Eval("GUExtend_Sex").ToString(),"2") %></td>
                       <td><%# Eval("GUExtend_Address") %></td>
                      <td><%# Eval("GUExtend_Signature") %></td>
                      <td><%# getInfo(Eval("GUParentUserID").ToString(),"3") %></td>
                     
                    <td  style="text-align:center;"> 
                    <asp:LinkButton ID="linkdel" CommandArgument='<%# Eval("GUUserID") %>'  OnClientClick="return confirm('确定删除？')" CommandName="Del" CssClass="btn btn-danger btn-mini" runat="server"  Text="删除"></asp:LinkButton>
                      |<asp:LinkButton ID="linkupdate"  CommandArgument='<%# Eval("GUUserID") %>' Text=" 编 辑 " CssClass="btn btn-primary btn-mini"  CommandName="Update" runat="server"></asp:LinkButton>
                       |<asp:LinkButton ID="linkagshift" Text="会员转移" CommandArgument='<%# Eval("GUUserID") %>' CommandName="VIPShift"  CssClass="btn btn-warning btn-mini" runat="server"></asp:LinkButton>
                         |<asp:LinkButton ID="linkPullblack" Text="拉黑" CommandArgument='<%# Eval("GUUserID") %>' CommandName="VIPPB"  CssClass=" btn btn-inverse btn-mini" runat="server"></asp:LinkButton>
                 
                         </td>
                 
                </tr>
              
              </tbody>
          

             </ItemTemplate>
             <FooterTemplate>
                
         <tbody>                <tr >
                     <td  colspan="13" style="text-align:center;">

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
        </div>
               </div>
        </form>
</asp:Content>
