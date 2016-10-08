<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DialogueInfo.aspx.cs" Inherits="GS.Web.Manage.DialogueInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>无标题文档</title>
<style>
*{margin:0;padding:0;}
body{font:12px Arial, Helvetica,宋体;color:#535353;background-color:#ffffff;}
li{list-style:none;}
.question{width:65%;height:700px;margin:auto;padding:10px;}
.question h1{font:18px/22px 微软雅黑;}
.question .describe{color:#666;line-height:20px;padding:6px 0;}
.question .asker{padding:3px 0;}
.question .answer{padding:10px;}
textarea{border:1px solid #666;border-radius:5px;margin:5px 0;width:50%;height:200px;line-height:20px;}
.question .answers{padding:10px 0;}
.question .answers li{line-height:16px;padding:5px 0;border-bottom:1px solid #f2f1f2;margin-top:5px;}
.question .answers li .infor .time{color:#999;padding-left:10px;}
.question .answers li .infor{padding-bottom:3px;}
</style>
</head>

<body>
    <form id="form1" runat="server">
<div class="question">
 <%--<h1>问题标题</h1>

 <p class="asker">提问者：nick</p>

 <p class="describe">问：<strong>问题详情问题详情问题详情问题详情问题详情问题详情问题详情问题详情,问题描述问题描述问题描述问题描述问题描述,问题描述问题描述问题描述问题描述问题描述,问题描述问题描述问题描述问题描述问题描述，问题描述问题描述问题描述问题描述问题描述。问题描述问题描述问题描述问题描述问题描述，问题描述问题描述问题描述问题描述问题描述。</strong></p>
 <div class="answers">
   <ul>
     <li>
       <p class="infor"><strong>&nbsp;nick:</strong><span class="time">2014-17-17</span></p>
       回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容 回答内容
     </li>
  

   </ul>
 </div>--%>
    <%=list %>

  
 <div class="answer">
   提交回答：<br />
     <asp:TextBox ID="txthdcontent" TextMode="MultiLine" runat="server" Width="500"></asp:TextBox><br />
     <asp:Button ID="btnSub" Height="32" Width="80" runat="server" Text=" 提 交 " OnClick="btnSub_Click" />
    
 </div>
</div>
        </form>
</body>
</html>