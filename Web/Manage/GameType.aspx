<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GameType.aspx.cs" Inherits="GS.Web.Manage.GameType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="/css/style.css" rel="stylesheet" />
    <script src="/js/jquery-1.4.2.min.js"></script>
    <script type="text/javascript">


    </script>
</head>
<body>
    <form id="form1" runat="server">
  <div class="panel">
 <p class="title"></p>
 <div class="main">
   <div class="tr">
     <span class="sm">基本配置</span>
     <div class="ul">
       <p><span class="th">名称：</span><asp:TextBox ID="txtKindName" runat="server"></asp:TextBox></p>
       <p><span class="th">排序：</span><asp:TextBox ID="txtSortID" runat="server" MaxLength="3"  onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox></p>
         <p><span class="th">进程ID：</span><asp:TextBox ID="txtProcessType" runat="server" ></asp:TextBox></p>
     </div>
   </div>
   <!--tr-->
   <div class="tr">
     <span class="sm">游戏配置</span>
     <div class="ul">
       <p><span class="th">游戏类别：</span><asp:DropDownList ID="dropType" runat="server">
           </asp:DropDownList></p>
       <p><span class="th">桌子数目：</span><asp:TextBox ID="txtTableCount" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox></p>
     </div>
   </div>
   <!--tr-->
   <div class="tr">
     <span class="sm">投注配置</span>
     <div class="ul">
       <p><span class="th">最大投注：</span><asp:TextBox ID="txtHighScore" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" runat="server"></asp:TextBox></p>
       <p><span class="th">游戏底注：</span><asp:TextBox ID="txtCellScore" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" runat="server"></asp:TextBox></p>
       <p><span class="th">入场率：</span><asp:TextBox ID="txtLessScore"  onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" runat="server"></asp:TextBox></p>
       <p><span class="th">抽水率：</span><asp:TextBox ID="txtTaxRate" runat="server" onkeyup="if(isNaN(value))execCommand('undo')" onafterpaste="if(isNaN(value))execCommand('undo')"></asp:TextBox>(千分比)</p>
     </div>
   </div>
   <!--tr-->
   <div class="tr">
     <span class="sm">智能机器人配置</span>
     <div class="ul">
       <p><span class="th">最大人数：</span><asp:TextBox ID="txtAIUserCount" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" runat="server"></asp:TextBox></p>
       <p><span class="th">智商：</span><span class="range">
         
             <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
           <asp:TextBox ID="TextBox1"  runat="server" ></asp:TextBox>
          <cc1:SliderExtender ID="SliderExtender1" runat="server"
         TargetControlID="TextBox1"
         BoundControlID="lbAILevel"
         Decimals ="0"
         Maximum ="100"
         Minimum ="0"
         EnableHandleAnimation ="false"   
         >       </cc1:SliderExtender>
           <asp:Label ID="lbAILevel" runat="server" ></asp:Label>
        <%--   <asp:HiddenField ID="hidAILevel" runat="server" />--%>
      
                                     </span>

       </p>
       <p><span class="th">彩池：</span><asp:TextBox ID="txtBetPool" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" runat="server"></asp:TextBox></p>
     </div>
   </div>
   <!--tr-->
   <p class="btn"><asp:Button ID="btnSubSpecies" Width="80" runat="server" Text=" 确 定 " OnClick="btnSubSpecies_Click" />
      &nbsp; &nbsp; &nbsp; &nbsp;
    <input type="reset" value=" 重 置 "  style="width:80px"   id="btnqx"/></p>
 </div>
</div>

    </form>
</body>
</html>
