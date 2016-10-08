<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="AddAgent.aspx.cs" Inherits="GS.Web.Manage.AddAgent" %>
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
                        <asp:TextBox ID="txtAgentAccount"  onKeyUp="value=value.replace(/[^\d|chun]/g,'')"   runat="server"></asp:TextBox> 
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
                    </td>
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

 <%--   <div class="span6">
      <div class="widget-box">
        <div class="widget-title"> <span class="icon"> <i class="icon-align-justify"></i> </span>
          <h5>Personal-info</h5>
        </div>
        <div class="widget-content nopadding">
          <form action="#" method="get" class="form-horizontal">
            <div class="control-group">
              <label class="control-label">First Name :</label>
              <div class="controls">
                <input type="text" class="span11" placeholder="First name" />
              </div>
            </div>
            <div class="control-group">
              <label class="control-label">Last Name :</label>
              <div class="controls">
                <input type="text" class="span11" placeholder="Last name" />
              </div>
            </div>
            <div class="control-group">
              <label class="control-label">Password input</label>
              <div class="controls">
                <input type="password"  class="span11" placeholder="Enter Password"  />
              </div>
            </div>
            <div class="control-group">
              <label class="control-label">Company info :</label>
              <div class="controls">
                <input type="text" class="span11" placeholder="Company name" />
              </div>
            </div>
            <div class="control-group">
              <label class="control-label">Description field:</label>
              <div class="controls">
                <input type="text" class="span11" />
                <span class="help-block">Description field</span> </div>
            </div>
            <div class="control-group">
              <label class="control-label">Message</label>
              <div class="controls">
                <textarea class="span11" ></textarea>
              </div>
            </div>

           <div class="control-group">
              <label class="control-label">Select input</label>
              <div class="controls">
                <select >
                  <option>First option</option>
                  <option>Second option</option>
                  <option>Third option</option>
                  <option>Fourth option</option>
                  <option>Fifth option</option>
                  <option>Sixth option</option>
                  <option>Seventh option</option>
                  <option>Eighth option</option>
                </select>
              </div>
            </div>
           
            <div class="form-actions">
              <button type="submit" class="btn btn-success">Save</button>
            </div>
        
        </div>
      </div>
        </form>
    </div>--%>
   

</asp:Content>
