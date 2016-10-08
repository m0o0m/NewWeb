<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Site1.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="GS.Web.Manage.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="content-header">
    <div id="breadcrumb"> <a href="#" title="Go to Home" class="tip-bottom"><i class="icon-home"></i> 首页</a> <a href="#" class="current">Error</a> </div>
    <h1>Error </h1>
  </div>
  <div class="container-fluid">
    <div class="row-fluid">
      <div class="span12">
        <div class="widget-box">
          <div class="widget-title"> <span class="icon"> <i class="icon-info-sign"></i> </span>
            <h5>Error</h5>
          </div>
          <div class="widget-content">
            <div class="error_ex">
              <h1>提示</h1>
              <h3>您没权限操作该模块！</h3>
              <p>You can't change permissions operation module</p>
           <%--   <a class="btn btn-warning btn-big"  href="index.html">Back to Home</a> </div>--%>
          </div>
        </div>
      </div>
    </div>
  </div>
    </div>
</asp:Content>
