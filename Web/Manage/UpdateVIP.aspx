<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="UpdateVIP.aspx.cs" Inherits="GS.Web.Manage.UpdateVIP" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <form id="form1" runat="server">
      <div id="content-header">
  <div id="breadcrumb"> <a href="index.html" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a> <a href="#" class="tip-bottom">会员管理</a> <a href="#" class="current">修改会员</a> </div>
  <h1>修改会员</h1>
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
                    <td>帐号：</td>
                    <td>
                        <asp:TextBox ID="txtGUAccount"  onKeyUp="value=value.replace(/[^\d|chun]/g,'')"   runat="server"></asp:TextBox> 
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtGUAccount" runat="server" ErrorMessage="帐号不能为空"></asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td>密码：</td>
                    <td>
                        <asp:TextBox ID="txtGUPasswd" runat="server" onKeyUp="value=value.replace(/[^\d|chun]/g,'')"  TextMode="Password"></asp:TextBox> 
                       <span style="color:red">(注：修改密码请输入,保持原密码则不用输入)</span>

                    </td>
                </tr>
                <tr>
                    <td>昵称：</td>
                    <td>
                        <asp:TextBox ID="txtGUName"  runat="server" > </asp:TextBox>
                    </td>
                </tr>

                 <tr>
                    <td>邮箱：</td>
                    <td>
                        <asp:TextBox ID="txtGUEmail"   runat="server"  > </asp:TextBox>

                             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ErrorMessage="邮箱格式不符！" ControlToValidate="txtGUEmail" 
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>


                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtGUEmail" runat="server" ErrorMessage="邮箱不能为空"></asp:RequiredFieldValidator>


                    </td>
                </tr>

                 <tr>
                    <td>电话：</td>
                    <td>
                        <asp:TextBox ID="txtGUTel" runat="server"  > </asp:TextBox>
                                                <asp:RegularExpressionValidator ID="rev_tel" runat="server" ControlToValidate="txtGUTel" ValidationExpression="(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)" ErrorMessage="电话号码格式不正确"></asp:RegularExpressionValidator>

                    </td>
                </tr>

                 <tr>
                    <td>姓名：</td>
                    <td>
                        <asp:TextBox ID="txtGUExtend_RealName" runat="server"  > </asp:TextBox>
                         
                    </td>
                </tr>

                 <tr>
                    <td>身份证号码：</td>
                  
                    <td>
                        <asp:TextBox ID="txtGUExtend_IDCardNo" runat="server"   MaxLength="18"  onBlur="getBirthday();"   > </asp:TextBox>
                        


                           <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
            ErrorMessage="身份证格式不符！" ControlToValidate="txtGUExtend_IDCardNo" 
            ValidationExpression="^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$"></asp:RegularExpressionValidator>


                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtGUExtend_IDCardNo" runat="server" ErrorMessage="身份证不能为空"></asp:RequiredFieldValidator>

                        
                             


                    </td>
                </tr>

                <tr>
                    <td>生日：</td>
                    <td>
                        <asp:TextBox ID="txtGUExtend_Birthday" runat="server"   > </asp:TextBox>  
               
                    </td>
                </tr>

               <tr>
                    <td>性别：</td>
                    <td>
                     
                        <asp:DropDownList ID="dropsex" runat="server">
                            <asp:ListItem Value="0">男</asp:ListItem>
                            <asp:ListItem Value="1">女</asp:ListItem>
                        </asp:DropDownList>
                     
                    </td>
                </tr>

               <tr>
                    <td>地址：</td>
                    <td>
                        <asp:TextBox ID="txtGUExtend_Address" runat="server"  > </asp:TextBox>  
                     
                    </td>
                </tr>

               <tr>
                    <td>签名：</td>
                    <td>
                        <asp:TextBox ID="txtGUExtend_Signature" runat="server"  > </asp:TextBox>  
                     
                    </td>
                </tr>
              
              
               <tr>
                    <td>代理帐号：</td>
                    <td>
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                        <asp:TextBox ID="txtAgentAccount" runat="server"  > </asp:TextBox>  

                        <cc1:AutoCompleteExtender    ID="AutoCompleteExtender1" ServicePath="~/Manage/Service.asmx"  CompletionSetCount="10" MinimumPrefixLength="1" ServiceMethod="GetCompleteDepart" runat="server" TargetControlID="txtAgentAccount"></cc1:AutoCompleteExtender>
                        
                                                                    
                    
                    </td>
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
