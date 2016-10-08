using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace GS.Web.Manage
{
    public partial class EnableGame : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            TreeViewBind(); 

            
        }



        public void TreeViewBind()
        {
            TreeNode root = new TreeNode("根节点", "P0");

            DataSet ds = new BLL.GameTypeInfo().TypeBind(-1);
            DataTable dt = ds.Tables["ds"];
            foreach (DataRow row in dt.Rows)
            {
                TreeNode node = new TreeNode(row["Texts"].ToString(), "T"+row["TypeID"].ToString());
                RecursiveTree(node);
                node.CollapseAll();
                root.ChildNodes.Add(node);
                
            }
            TreeView1.Nodes.Add(root);

        }


        /// <summary>
        /// 递归树
        /// </summary>
        /// <param name="node"></param>
        public void RecursiveTree(TreeNode node)
        {
            if (node.Value.Substring(0, 1) == "T")
            {
                DataSet dst = new BLL.GameKindInfo().KindBind(Convert.ToInt32(node.Value.Substring(1)), 0);
                DataTable dtt = dst.Tables["ds"];
                foreach (DataRow trow in dtt.Rows)
                {
                    TreeNode Tnd = new TreeNode(trow["Texts"].ToString(), "K" + trow["KindID"]);
                    Tnd.ShowCheckBox = true;
                    node.ChildNodes.Add(Tnd);

                    RecursiveTree(Tnd);
                }
            }
            else if (node.Value.Substring(0, 1) == "K")
            {
                DataSet dsk = new BLL.GameServerInfo().ServerBind(Convert.ToInt32(node.Value.Substring(1)), 0);
                DataTable dtk = dsk.Tables["ds"];
                foreach (DataRow krow in dtk.Rows)
                {
                    TreeNode stnd = new TreeNode(krow["Texts"].ToString(), "S" + krow["ServerID"]);
                    node.ChildNodes.Add(stnd);
                }

            }


        }

        protected void btnenable_Click(object sender, EventArgs e)
        {
            try
            {
                 bool result=false;
                 for (int i = 0; i < TreeView1.CheckedNodes.Count; i++)
                 {
                     
                         int KindID = Convert.ToInt32(TreeView1.CheckedNodes[i].Value.Substring(1));
                         int TypeID = Convert.ToInt32(TreeView1.CheckedNodes[i].Parent.Value.Substring(1));
                         int enab = 1;
                         result = new BLL.GameKindInfo().isEnaableGame(enab, KindID);
                         if (TreeView1.CheckedNodes[i].ChildNodes.Count > 0)
                         {
                             result = new BLL.GameServerInfo().isEnableGame(KindID, TypeID, enab);
                         }
                         TreeNode node = TreeView1.CheckedNodes[i].Parent;
                         node.ChildNodes.Remove(TreeView1.CheckedNodes[i]);
                         i--;
                    
                 }
                if (result)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('启用成功')</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('启用失败')</script>");
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}