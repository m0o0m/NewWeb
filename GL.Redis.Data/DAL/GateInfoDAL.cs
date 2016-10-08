using GL.Common;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GL.Data.DAL
{
    public class GateInfoDAL
    {
        static string xmlFilePath = Utils.ServerPath("~/App_Data/Gate.xml");

        internal static IEnumerable<GateInfo> GetModel()
        {

            XElement xes = XElement.Load(xmlFilePath);

            //查询指定名称的元素

            IEnumerable<GateInfo> elements = from item in xes.Elements("GateInfo")
                                             select new GateInfo
                                             {
                                                 ID = item.Attribute("ID") == null ? 0 : Convert.ToInt32(item.Attribute("ID").Value),
                                                 GIP = item.Attribute("GIP") == null ? (string)null : item.Attribute("GIP").Value,
                                                 GPort = item.Attribute("GPort") == null ? (string)null : item.Attribute("GPort").Value,
                                                 ZIP = item.Attribute("ZIP") == null ? (string)null : item.Attribute("ZIP").Value,
                                                 ZPort = item.Attribute("ZPort") == null ? (string)null : item.Attribute("ZPort").Value,
                                                 HIP = item.Attribute("HIP") == null ? (string)null : item.Attribute("HIP").Value,
                                                 HPort = item.Attribute("HPort") == null ? (string)null : item.Attribute("HPort").Value,
                                                 Limit = item.Attribute("Limit") == null ? 0 : Convert.ToInt32(item.Attribute("Limit").Value),
                                                 Description = item.Attribute("Description") == null ? (string)null : item.Attribute("Description").Value,
                                                 Type = item.Attribute("Type") == null ? gateType.内网测试 : (gateType)Convert.ToInt32(item.Attribute("Type").Value),
                                                 Num = 0
                                               };

#if DEBUG
            return elements.Where(x => x.Type == gateType.内网测试);
#endif
#if Release
            return elements.Where(x => x.Type == gateType.正式);
#endif
#if Test
            return elements.Where(x => x.Type == gateType.外网测试);
#endif

        }

        internal static IEnumerable<GateInfo> GetModelByIDArray(IEnumerable<int> ID)
        {
            return GetModel().Where(delegate (GateInfo x) {
                return ID.Contains(x.ID);
            });
        }

        internal static GateInfo GetModelByID(int ID)
        {
            return GetModel().Where(x => x.ID == ID).FirstOrDefault();
        }
    }
}
