using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GL.Pay.YiDong
{

    public class XMLHelp
    {
        private XDocument _document;

        public XDocument Document
        {
            get { return _document; }
            set { _document = value; }
        }
        private string _fPath = "";

        public string FPath
        {
            get { return _fPath; }
            set { _fPath = value; }
        }

        public DataRow GetEntityRow()
        {
            DataTable dtData = new DataTable();
            XElement root = this._document.Root;
            IEnumerable<XElement> elements = root.Elements();

            foreach (XElement item in elements)
            {
                dtData.Columns.Add(item.Name.LocalName);
            }
            DataRow dr = dtData.NewRow();
            int i = 0;
            foreach (XElement item in elements)
            {
                dr[i] = item.Value;
                i = i + 1;
            }
            dtData.Rows.Add(dr);
            return dr;
        }

    }

}
