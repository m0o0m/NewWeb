using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GS.Web.Manage
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ////TreeView1.SelectedNode.Parent.ChildNodes.Remove(TreeView1.SelectedNode);
            //int node = TreeView1.SelectedNode.Index;
            //TreeView1.Nodes.RemoveAt(node);
            ////
            TreeView1.SelectedNode.Parent.ChildNodes.Remove(TreeView1.SelectedNode);
        }
    }
}