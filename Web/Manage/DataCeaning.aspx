<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="DataCeaning.aspx.cs" Inherits="GS.Web.Manage.DataCeaning" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<link href="/css/manhuaDate.1.0.css" rel="stylesheet" />
  <script src="/js/jquery-1.4.2.min.js"></script>
<script src="/js/manhuaDate.1.0.js" charset="gb2312"></script>
        <script type="text/javascript">

            $(function () {
                $("input.mh_date").manhuaDate({
                    Event: "click",//可选				       
                    Left: 0,//弹出时间停靠的左边位置
                    Top: -16,//弹出时间停靠的顶部边位置
                    fuhao: "-",//日期连接符默认为-
                    isTime: false,//是否开启时间值默认为false
                    beginY: 1949,//年份的开始默认为1949
                    endY: 2049//年份的结束默认为2049
                });

            });




            function check() {
                var date1 = document.getElementById('<%=txtqs.ClientID%>').value;
                var date2 = document.getElementById('<%=txtjs.ClientID %>').value;
                if (date2 < date1) {
                    alert("结束日期不能小于开始日期");
                }
            }

</script>

   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


         <form id="form1"   runat="server">
    <div id="content-header">
  <div id="breadcrumb"> <a href="index.html" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a> <a href="#" class="tip-bottom">系统设置</a> <a href="#" class="current">数据清理</a> </div>
  <h1>数据清理</h1>
</div>
  <div class="container-fluid">
    <div class="row-fluid">
         <hr>
    <div class="widget-box">
        <div class="widget-title">
            <span class="icon"><i class="icon-align-justify"></i></span>
            <h5>数据清理</h5>
        </div>
        <div class="widget-content ">
   
          <table class="auto-style1">
                <tr>
                    <td> 数据类型：</td>
                    <td>
                        <asp:DropDownList ID="dropdatatype" runat="server">
                            <asp:ListItem Value="1">管理员日志记录</asp:ListItem>
                            <asp:ListItem Value="2">代理日志记录</asp:ListItem>
                            <asp:ListItem Value="3">会员日志记录</asp:ListItem>
                            <asp:ListItem Value="4">游戏记录</asp:ListItem>
                            <asp:ListItem Value="5">充值记录</asp:ListItem>
                            <asp:ListItem Value="6">兑换记录</asp:ListItem>

                        </asp:DropDownList>
                    </td>
                    <td>
                         起始时间:<asp:TextBox ID="txtqs" CssClass="mh_date" runat="server" ></asp:TextBox> 至 <asp:TextBox  CssClass="mh_date" ID="txtjs" runat="server"  ></asp:TextBox>

                    </td>

                    <td>
                        <asp:Literal ID="ltlMsg" runat="server"></asp:Literal>
                    
                        <asp:Button ID="btnDataClear"  runat="server" Text="数据清理" OnClick="btnDataClear_Click" />
                        <span  style="color:red">注:(清理的数据将不可恢复，请谨慎操作！)</span>
                         <asp:Button ID="btnHid" runat="server" OnClick="btnHid_Click" Width="0px"  Height="0px"/>
                         <asp:HiddenField ID="hid" runat="server"  />
                  

                    </td>
                </tr>
              
            </table>
    
    
       


</div>



             </div>
         

  
         

        </div>
    </div>
        

        </form>


</asp:Content>
