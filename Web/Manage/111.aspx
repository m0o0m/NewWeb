<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="111.aspx.cs" Inherits="GS.Web.Manage._111" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

      <script src="js/zDialog.js"></script>
    <script src="js/zDrag.js"></script>
    <script type="text/javascript">

        function open3(title, url) {
            var diag = new Dialog();
            diag.Width = 380;
            diag.Height = 200;
            diag.Title = title;
            diag.URL = url;
            diag.show();
        }
      </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    打死点解啊设立登记as的
    大苏打

    <input type="button" value="sdad" onclick="open3('dasd','www.baidu.com')" />


</asp:Content>
