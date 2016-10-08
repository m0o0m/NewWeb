<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="UpdateAgent.aspx.cs" Inherits="GS.Web.Manage.UpdateAgent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style type="text/css">
        .auto-style1 {
            padding-top:10px !important;
        }
          </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <form id="form1" runat="server">
      <div id="content-header">
  <div id="breadcrumb"> <a href="index.html" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a> <a href="#" class="tip-bottom">代理</a> <a href="#" class="current">添加代理</a> </div>
  <h1>添加代理</h1>
</div>
   
     <div class="container-fluid">
    <hr>
    <div class="row-fluid">
        
      <div class="widget-title">
            <span class="icon"><i class="icon-align-justify"></i></span>
            <h5></h5>
        </div>
       <div class="widget-box">
        <div class="widget-content  ">
   
          <table class="auto-style1 ">
                <tr>
                    <td>代理帐号：</td>
                    <td>
                        <asp:TextBox ID="txtAgentAccount" ReadOnly="true"  onKeyUp="value=value.replace(/[^\d|chun]/g,'')"   runat="server"></asp:TextBox> 
                    </td>
                </tr>
                <tr>
                    <td>代理昵称：</td>
                    <td>
                        <asp:TextBox ID="txtAgentName" runat="server"></asp:TextBox> 
                    </td>
                </tr>
                <tr>
                    <td>代理密码：</td>
                    <td>
                        <asp:TextBox ID="txtAgentPasswd" onKeyUp="value=value.replace(/[^\d|chun]/g,'')"  runat="server" TextMode="Password" > </asp:TextBox>
                       <span style="color:red">(注：修改密码请输入,保持原密码则不用输入)</span></td>
                </tr>

                 <tr>
                    <td>代理QQ：</td>
                    <td>
                        <asp:TextBox ID="txtAgentQQ" MaxLength="10"  onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"   runat="server"  > </asp:TextBox>
                    </td>
                </tr>

                 <tr>
                    <td>代理手机：</td>
                    <td>
                        <asp:TextBox ID="txtAgentTel" runat="server"  > </asp:TextBox>

                        <asp:RegularExpressionValidator ID="rev_tel" runat="server" ControlToValidate="txtAgentTel" ValidationExpression="(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)" ErrorMessage="电话号码格式不正确"></asp:RegularExpressionValidator>
                    </td>
                </tr>

                <tr>
                    <td>代理邮箱：</td>
                    <td>
                        <asp:TextBox ID="txtAgentEmail" runat="server"  > </asp:TextBox>  
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ErrorMessage="邮箱格式不符！" ControlToValidate="txtAgentEmail" 
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </td>
                </tr>

                 <tr>
                    <td>收益模式：</td>
                    <td>
                        
                        <asp:DropDownList ID="dropModel" runat="server">
                            <asp:ListItem Value="0">收益的百分比占成</asp:ListItem>
                            <asp:ListItem Value="1">佣金</asp:ListItem>
                        </asp:DropDownList>


                        &nbsp;</td>
                </tr>

                     <tr>
                    <td>权限分配：</td>
                    <td >
                        
                        <%--<asp:DropDownList ID="dropQx" runat="server" >
                            <asp:ListItem Value="0">增加</asp:ListItem>
                            <asp:ListItem Value="1">删除</asp:ListItem>
                             <asp:ListItem Value="2">修改</asp:ListItem>
                        </asp:DropDownList>--%>
                     
                          <asp:CheckBox ID="c1" Width="20%" Text="增加"  runat="server" />
                      
                          <asp:CheckBox ID="c2" Width="20%" Text="删除"  runat="server" />
                          <asp:CheckBox ID="c3" Width="20%" Text="修改"  runat="server" />


                        &nbsp;</td>
                </tr>

                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSava" CssClass="btn btn-success" runat="server" Text="保 存" OnClick="btnSava_Click"  /></td>
                </tr>
            </table>
    
    </div>
           </div>
        </div>
           </div>
    </form>

</asp:Content>
