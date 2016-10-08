wangzhaoyang：bsuperman@126.com

注意：标题的class=top，脚本中会用到不要修改，如果要修正则搜索脚本中用到的地方替换吧
<TR class=top height=25>
  <TD noWrap>组织名称</TD>
  <TD width=70>简称</TD>
  <TD width=70>联系人</TD>
  <TD width=100>联系电话</TD>
  <TD width=100>传真</TD>
</TR>

注意：标题的class=bg1，脚本中会用到不要修改，如果要修正则搜索脚本中用到的地方替换吧
pid=""表示此节点为顶层节点
textIndex=表示此tr文字显示的位置，顶层节点为0然后次节点依次+20pt


<TR class=bg1 id=TR_0 pid="" textIndex=0 >
  <TD style="TEXT-INDENT: 0pt; TEXT-ALIGN: left">
	  <IMG id=IMG_0 style="CURSOR: hand" onclick="javascript:showHiddenNode(this, 'TR_0')" 
	  src="images/dtree/nolines_minus.gif" align=absMiddle border=0>
	  <A id="A_0" href="#">南京移动</A>
  </TD>
  <TD>移动不动</TD>
  <TD>不动明王</TD>
  <TD>10086</TD>
  <TD>fax1086</TD>
</TR>

<TR class=bg1 id=TR_0_0 pid="TR_0" textIndex=20 >
  <TD style="TEXT-INDENT: 20pt; TEXT-ALIGN: left">
	<IMG id=IMG_0_0 style="CURSOR: hand" onclick="javascript:showHiddenNode(this, 'TR_0_0')" 
	src="images/dtree/nolines_plus.gif" align=absMiddle border=0> 
	<A id="A_0_0" href="#">城区分公司</A>
  </TD>
  <TD>城区</TD>
  <TD>王朝阳</TD>
  <TD>13333333</TD>
  <TD>fox</TD>
</TR>

