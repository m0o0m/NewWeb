using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace GS.Web.Manage
{
    public partial class ListOfConfiguration : Public
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            TreeViewBind();
            btndelnode.Attributes.Add("onclick", "return yesno()");
        }



        public void TreeViewBind()
        {
            TreeNode root = new TreeNode { Text = "Casino", Value = "P0", PopulateOnDemand = true };
            root.Collapse();
         
            //TreeNode node = new TreeNode("dasdsa", "1");
            //node.PopulateOnDemand = true;
            //node.Collapse();

            //root.ChildNodes.Add(node);
          

            TreeView1.Nodes.Add(root);
        }

        protected void TreeView1_TreeNodeCollapsed(object sender, TreeNodeEventArgs e)
        {

        }


     

        protected void TreeView1_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
        
              e.Node.ChildNodes.Clear();


              switch (e.Node.Value.Substring(0,1))
              {
                  case"P":
                  TreeNode tnode = new TreeNode();
                  DataSet ds = new BLL.GameTypeInfo().TypeBind(1);
                  DataTable dt = ds.Tables["ds"];

                foreach (DataRow row in dt.Rows)
                {
                    tnode = new TreeNode { Text = "" + row["Texts"].ToString() + "", Value = "T" + row["TypeID"].ToString() + "", PopulateOnDemand = true };
                    e.Node.ChildNodes.Add(tnode);
                }
                      break;
                  case"T":
                      TreeNode knode = new TreeNode();
                      DataSet dsk = new BLL.GameKindInfo().KindBind(Convert.ToInt32(e.Node.Value.Substring(1)), 1);
                      DataTable dtk = dsk.Tables["ds"];
                      foreach (DataRow krow in dtk.Rows)
                      {
                          knode = new TreeNode { Text = "" + krow["Texts"].ToString() + "", Value = "K" + krow["KindID"].ToString() + "", PopulateOnDemand = true };
                          e.Node.ChildNodes.Add(knode);
                      }
                      break;

                  case"K":
                      TreeNode snode = new TreeNode();
                      DataSet dss = new BLL.GameServerInfo().ServerBind(Convert.ToInt32(e.Node.Value.Substring(1)), 1);
                      DataTable dts = dss.Tables["ds"];
                      foreach (DataRow srow in dts.Rows)
                      {
                          snode = new TreeNode { Text = "" + srow["Texts"].ToString() + "", Value = "S" + srow["ServerID"].ToString() + "", PopulateOnDemand = true };
                          e.Node.ChildNodes.Add(snode);
                      }
                      break;

              }
               
                
            
        }

        protected void btnAddGameSpecies_Click(object sender, EventArgs e)
        {
            if (  TreeView1.SelectedValue=="" || TreeView1.SelectedValue.Substring(0, 1) != "P")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('请选择Casino节点')</script>");
            }
            else
            {
                TreeView1.SelectedNode.Collapse();
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>showTipsWindown('游戏种类', 'GameSpecies.aspx', 400, 250);</script>");
            }
        }

        protected void btnAddGameTypeServe_Click(object sender, EventArgs e)
        {
            if (TreeView1.SelectedValue == "" || TreeView1.SelectedValue.Substring(0, 1) != "K")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('请选择正确的游戏类型')</script>");
            }
            else
            {
                TreeView1.SelectedNode.Collapse();
                TreeView1.SelectedNode.PopulateOnDemand = true;
                string kid = TreeView1.SelectedValue.Substring(1);
                string tid = TreeView1.SelectedNode.Parent.Value.Substring(1);
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>showTipsWindown('游戏服务站点', 'GameServer.aspx?kid="+kid+"&tid="+tid+"', 400, 250);</script>");
            }
        }

        protected void btndelnode_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeView1.SelectedValue == "" || TreeView1.SelectedValue == "P0")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('请选择正确的节点')</script>");
                }
                else
                {
                    string flag = TreeView1.SelectedValue.Substring(0, 1);

                    int did = Convert.ToInt32(TreeView1.SelectedValue.Substring(1));

                    switch (flag)
                    {
                        case"T":
                            if (TreeView1.SelectedNode.ChildNodes.Count == 0)
                            {
                                TreeView1.SelectedNode.Parent.Collapse();
                                new BLL.GameTypeInfo().Delete(did);
                                TreeView1.SelectedNode.Parent.Selected = true;
                            }
                            else
                            {
                                TreeView1.SelectedNode.Parent.Collapse();
                                new BLL.GameServerInfo().Delete(" TypeID=" + did + "");
                                new BLL.GameKindInfo().Delete(" TypeID=" + did + "");
                                new BLL.GameTypeInfo().Delete(did);

                                TreeView1.SelectedNode.Parent.Selected = true;
                            }

                            break;
                        case"K":
                            if (TreeView1.SelectedNode.ChildNodes.Count == 0)
                            {
                                TreeView1.SelectedNode.Parent.Collapse();
                                new BLL.GameKindInfo().Delete(did);
                                TreeView1.SelectedNode.Parent.Selected = true;
                            }
                            else
                            {
                                TreeView1.SelectedNode.Parent.Collapse();
                                new BLL.GameServerInfo().Delete(" KindID=" + did + "");
                                new BLL.GameKindInfo().Delete(did);
                               
                                TreeView1.SelectedNode.Parent.Selected = true;
                            }

                            break;
                        case"S":
                            if (TreeView1.SelectedNode.ChildNodes.Count == 0)
                            {
                                TreeView1.SelectedNode.Parent.Collapse();
                                new BLL.GameServerInfo().Delete(did);
                                TreeView1.SelectedNode.Parent.Selected = true;
                            }
                            break;
                    }

                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }


      

        protected void btnnodepz_Click(object sender, EventArgs e)
        {
            if (TreeView1.SelectedValue != "" && TreeView1.SelectedValue != "P0")
            {
            string flag = TreeView1.SelectedValue.Substring(0, 1);
            string id = TreeView1.SelectedValue.Substring(1);

            switch (flag)
            {
                    
                case"T":
                    TreeView1.SelectedNode.Parent.Collapse();
                    TreeView1.SelectedNode.Parent.Selected = true;
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>showTipsWindown('游戏种类', 'GameSpecies.aspx?tid="+id+"', 400, 250);</script>");

                    break;
                case"K":

                      TreeView1.SelectedNode.Parent.Collapse();
                    TreeView1.SelectedNode.Parent.Selected = true;

                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>showTipsWindown('游戏类型', 'GameType.aspx?kid=" + id + "', 410, 500);</script>");
                    break;
                case"S":
                    TreeView1.SelectedNode.Parent.Collapse();
                    TreeView1.SelectedNode.Parent.Selected = true;
                    string kid = TreeView1.SelectedValue.Substring(1);
                    string tid = TreeView1.SelectedNode.Parent.Value.Substring(1);
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>showTipsWindown('游戏服务站点', 'GameServer.aspx?sid=" + id + "&kid=" + kid + "&tid=" + tid + "', 400, 250);</script>");

                    break;

            }
            }else{
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('选择正确的节点')</script>");
            }
        }

        protected void btnAddGameType_Click(object sender, EventArgs e)
        {
            if (TreeView1.SelectedValue == "" || TreeView1.SelectedValue.Substring(0, 1) != "T")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('请选择正确的游戏种类')</script>");
            }
            else
            {
                TreeView1.SelectedNode.Collapse();
                TreeView1.SelectedNode.PopulateOnDemand = true;
                string tid = TreeView1.SelectedValue.Substring(1);
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>showTipsWindown('游戏服务站点', 'GameType.aspx?tid=" + tid + "', 410, 500);</script>");


            }
        }

        protected void btnenable_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>showTipsWindown('游戏启用', 'EnableGame.aspx', 410, 800);</script>");

        }

        protected void btndisable_Click(object sender, EventArgs e)
        {
            bool result=false;
            if (TreeView1.SelectedValue.Substring(0, 1) != "K")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('请选择正确的游戏类型')</script>");
            }
            else
            {
                int KindID =Convert.ToInt32(TreeView1.SelectedValue.Substring(1));
                int TypeID=Convert.ToInt32(TreeView1.SelectedNode.Parent.Value.Substring(1));
           
                result = new BLL.GameKindInfo().isEnaableGame(0, KindID);
                 if (TreeView1.SelectedNode.ChildNodes.Count > 0)
                 {
                     result = new BLL.GameServerInfo().isEnableGame(KindID, TypeID, 0);
                 }
                 TreeNode node = TreeView1.SelectedNode.Parent;
                 node.ChildNodes.Remove(TreeView1.SelectedNode);
                 
               
                 if (result)
                 {
                    
                     Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('禁用成功')</script>");
                 }
                 else
                 {
                     Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('禁用失败')</script>");
                 }
            }
        }

        //protected void btnSubSpecies_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        Model.GameTypeInfo gt = new Model.GameTypeInfo();
        //        gt.TypeName = txtTypeName.Text.Trim();
        //        gt.SortID = Convert.ToInt32(txtSortID.Text.Trim());
        //        gt.ImageID = Convert.ToInt32(dropImgID.SelectedValue);
        //        gt.Enable = true;
        //        int result = new BLL.GameTypeInfo().Add(gt);
        //        if (result > 0)
        //        {
        //            Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('保存成功')</script>");
        //        }
        //        else
        //        {
        //            Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('保存失败')</script>");
        //        }
        //    }
        //    catch (Exception)
        //    {
                
        //        throw;
        //    }
        //}


        //public void TextNull()
        //{

          
        //    foreach (Control c in Panel1.Controls)
        //    {

        //        if (c is TextBox)
        //            {
        //                ((TextBox)c).Text = "";
        //            }
                
        //    }
        //}
    }
}