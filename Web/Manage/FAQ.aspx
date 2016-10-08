<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="FAQ.aspx.cs" Inherits="GS.Web.Manage.FAQ" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <link href="/css/AspNetPage.css" rel="stylesheet" />
    <script type="text/javascript">

        function setHiddenCol(oTable, iCol)//Writed by 
        {
            for (i = 0; i < oTable.rows.length ; i++) {
                var strArr = iCol.toString();
                var getArr = strArr.split(",");
                for (var j = 0; j < getArr.length; j++) {
                    oTable.rows[i].cells[getArr[j]].style.display = oTable.rows[i].cells[getArr[j]].style.display == "none" ? "block" : "none";
                }
            }
        }

       
        function GetSession() {
            var val = '<%=strSession %>';
            if (val != "") {
                var arr = [3, 4]
                setHiddenCol(document.getElementById('tbfaq'), arr)

            }
        }
          
   
      


         



    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
      <form id="form1" runat="server" oninit="form1_Init" >
      <div id="content-header">
    <div id="breadcrumb"> <a href="#" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a><a href="#" class="tip-bottom">知识库</a> <a href="#" class="current">FAQ信息</a> </div>
         <h1>FAQ信息</h1>
  </div>

        <div class="container-fluid">

    <hr>

               <div >标题： <asp:TextBox ID="txtfaqtitle" runat="server"></asp:TextBox>
                &nbsp;类型：  <asp:DropDownList ID="droptype" runat="server">
                            <asp:ListItem Value="0">--请选择--</asp:ListItem>
                            <asp:ListItem Value="1">游戏问题</asp:ListItem>
                            <asp:ListItem Value="2">充值问题</asp:ListItem>
                            <asp:ListItem Value="3">兑换问题</asp:ListItem>
                            <asp:ListItem Value="4">其他问题</asp:ListItem>
                        </asp:DropDownList>  &nbsp;
                <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btn btn-info"  OnClick="btnSearch_Click" /></div>
    <div class="row-fluid">
     <div class="widget-box">
          <div class="widget-title"> <span class="icon"> <i class="icon-th"></i> </span>
            <h5>FAQ信息列表</h5>
          </div>


      


          <div class="widget-content nopadding">
     

         <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
             <HeaderTemplate>

                    <table class="table table-bordered table-striped" id="tbfaq" >
              <thead>
                <tr>
                  <th>FAQ标题</th>
                  <th>FAQ内容</th>
                  <th>FAQ类型</th>
                  <th>创建时间</th>
                  <th>基本操作</th>
                </tr>
              </thead>
             </HeaderTemplate>

             <ItemTemplate>
                    <tbody>
                <tr class="odd gradeX">
                  <td><%# Eval("faqtitle") %></td>
                  <td><%# GetString(Eval("faqcontent").ToString(),20) %></td>
                  <td><%# gettype(Eval("faqtype").ToString()) %></td>
                    <td><%# Eval("operdate") %></td>
                  <td style="text-align:center"> <asp:LinkButton ID="linkdel" CommandArgument='<%# Eval("Id") %>'  OnClientClick="return confirm('确定删除？')" CommandName="Del" CssClass="btn btn-danger btn-mini" runat="server"  Text="删除"></asp:LinkButton>
                      | <asp:LinkButton ID="linkupdate"  CommandArgument='<%# Eval("Id") %>' Text=" 编 辑（查看详细） " CssClass="btn btn-primary btn-mini"  CommandName="Update" runat="server"></asp:LinkButton>

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
         
    <webdiyer:AspNetPager ID="AspNetPager1"  CssClass="paginator" CurrentPageButtonClass="cpb" runat="server" PageSize="10" OnPageChanged="AspNetPager1_PageChanged"   FirstPageText="首页" LastPageText="尾页" NextPageText="下一页" PrevPageText="上一页"></webdiyer:AspNetPager>
        </div>
        </div></div>


       
        </form>
</asp:Content>
