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
    public class IAPProductDAL
    {
        static string xmlFilePath = Utils.ServerPath("~/App_Data/IAPProduct.xml");
        static string xmlFilePathForIOS = Utils.ServerPath("~/App_Data/IAPProduct_ios.xml");

        internal static IAPProduct GetModelByID(string ID)
        {

            XElement xes = XElement.Load(xmlFilePath);
 
            //查询指定名称的元素

            IEnumerable<IAPProduct> elements = from item in xes.Elements("item")
                                               where item.Attribute("product_id").Value == ID
                                               select new IAPProduct
                                               {
                                                   product_id = item.Attribute("product_id") == null ? (string)null : item.Attribute("product_id").Value,
                                                   goods = item.Attribute("goods") == null ? 0 : Convert.ToInt32(item.Attribute("goods").Value),
                                                   goodsType = item.Attribute("goodsType") == null ? 0 : Convert.ToInt32(item.Attribute("goodsType").Value),
                                                   price = item.Attribute("price") == null ? 0 : Convert.ToDecimal(item.Attribute("price").Value),
                                                   attach_5b = item.Attribute("attach_5b") == null ? 0 : Convert.ToInt32(item.Attribute("attach_5b").Value),
                                                   attach_chip = item.Attribute("attach_chip") == null ? 0 : Convert.ToInt32(item.Attribute("attach_chip").Value),
                                                   productname = item.Attribute("productname") == null ? (string)null : item.Attribute("productname").Value,
                                                   attach_props = item.Attribute("attach_props") == null ? (string)null : item.Attribute("attach_props").Value
                                               };



            return elements.FirstOrDefault();
        
        }
        internal static IAPProduct GetModelByIDForIOS(string ID)
        {

            XElement xes = XElement.Load(xmlFilePathForIOS);

            //查询指定名称的元素

            IEnumerable<IAPProduct> elements = from item in xes.Elements("item")
                                               where item.Attribute("product_id").Value == ID
                                               select new IAPProduct
                                               {
                                                   product_id = item.Attribute("product_id") == null ? (string)null : item.Attribute("product_id").Value,
                                                   goods = item.Attribute("goods") == null ? 0 : Convert.ToInt32(item.Attribute("goods").Value),
                                                   goodsType = item.Attribute("goodsType") == null ? 0 : Convert.ToInt32(item.Attribute("goodsType").Value),
                                                   price = item.Attribute("price") == null ? 0 : Convert.ToDecimal(item.Attribute("price").Value),
                                                   attach_5b = item.Attribute("attach_5b") == null ? 0 : Convert.ToInt32(item.Attribute("attach_5b").Value),
                                                   attach_chip = item.Attribute("attach_chip") == null ? 0 : Convert.ToInt32(item.Attribute("attach_chip").Value),
                                                   productname = item.Attribute("productname") == null ? (string)null : item.Attribute("productname").Value
                                               };



            return elements.FirstOrDefault();

        }


    }
}
