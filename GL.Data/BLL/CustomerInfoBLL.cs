using GL.Common;
using GL.Data.DAL;
using GL.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace GL.Data.BLL
{
    public class CustomerInfoBLL
    {
        public static PagedList<CustomerInfo> GetListByPage(int page, int id)
        {
            PagerQuery pq = new PagerQuery();
            pq.CurrentPage = page;
            pq.PageSize = 10;
            pq.RecordCount = DAL.PagedListDAL<CustomerInfo>.GetRecordCount(@"select count(0) from CustomerInfo");
            pq.Sql = string.Format(@"select * from CustomerInfo order by CustomerID desc limit {0}, {1}", pq.StartRowNumber, pq.PageSize);

            PagedList<CustomerInfo> obj = new PagedList<CustomerInfo>(DAL.PagedListDAL<CustomerInfo>.GetListByPage(pq), pq.CurrentPage, pq.PageSize, pq.RecordCount);
            return obj;
        }





        public static CustomerInfo GetModel(CustomerInfo model)
        {
            return CustomerInfoDAL.GetModel(model);
        }

        public static int UpdateState(CustomerInfo model)
        {

            return CustomerInfoDAL.UpdateState(model);
        }

        public static CustomerInfo GetModelByID(CustomerInfo model)
        {
            CustomerInfo cust = CustomerInfoDAL.GetModelByID(model);
            return cust;
        }



        public static int Delete(CustomerInfo model)
        {
            return CustomerInfoDAL.Delete(model);
        }

        public static int Add(CustomerInfo model)
        {
            return CustomerInfoDAL.Add(model);
        }

        public static int Update(CustomerInfo model)
        {
            return CustomerInfoDAL.Update(model);
        }

    }
}
