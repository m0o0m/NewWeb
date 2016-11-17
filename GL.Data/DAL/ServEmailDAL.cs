using GL.Command.DBUtility;
using GL.Data.Model;
using GL.Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Data.DAL
{
    public class ServEmailDAL
    {
        public static readonly string sqlconnectionString = PubConstant.ConnectionString;

        public static readonly string database1 = PubConstant.GetConnectionString("database1");
        public static readonly string database2 = PubConstant.GetConnectionString("database2");
        public static readonly string database3 = PubConstant.GetConnectionString("database3");
        internal static int Insert(ServEmail servEmail)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"insert into ServEmail(ServEmailTime,ServEmailTitle,ServEmailContent,ServEmailAuthor)values (@ServEmailTime,@ServEmailTitle,@ServEmailContent,@ServEmailAuthor)", servEmail);

                cn.Close();
                
                return i;
            }

        
        
        
        
        }

        internal static int Delete(ServEmail servEmail)
        {

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"delete from ServEmail where ServEmailID = @ServEmailID", servEmail);

                cn.Close();

                return i;
            }
        }



        public static int AddStock(int id, Int64 addStock)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"
update "+ database2 + @".e_userstockgroup set `Value`=`Value`+(@addStock) , UpdateTime = '"+DateTime.Now.ToString()+@"'
where id = @id;
", new { addStock= addStock, id= id });

                cn.Close();

                return i;
            }
        }


        public static int AddStock(string username, Int64 addStock)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"
update "+ database2 + @".e_userstockgroup as b 
LEFT JOIN "+ database2 + @".e_userstock as a on a.groupid = b.id 
set b.`Value`=b.`Value`+(@addStock)
where a.UserName = @username;

", new { addStock = addStock, username = username });

                cn.Close();

                return i;
            }
        }



        public static int AddStockGroup(string groupName, Int64 value)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                int i = cn.Execute(@"
insert into "+ database2 + @".e_userstockgroup(GroupName,Value,LastValue,UpdateTime)
values(@groupName,@value,0,'"+DateTime.Now.ToString()+@"');
", new { groupName = groupName, value = value });

                cn.Close();

                return i;
            }
        }



        //AddStockUser
        public static int AddStockUser(int id, string username)
        {
            string[] users = username.Split(','); //用户输入的参数

            IEnumerable<UserStock> dbUserList = GetUserBindList(id); //数据库存储的用户

            //检测哪些用户是需要添加的

            List<string> resUsersAdd = new List<string>();
            List<string> resUsersDelete = new List<string>();
            for (int i = 0; i < users.Length; i++) {//用户输入的参数，不存在数据库中则添加
                string uItem = users[i].Trim();
                IEnumerable<UserStock> hasExist = dbUserList.Where(m => m.UserName == uItem);
                if(hasExist==null || hasExist.Count() <= 0)//说明数据库里面没有这个值,记录下来 
                {
                    resUsersAdd.Add(uItem);
                }
            }

            foreach (var item in dbUserList)//数据库中有，但是数据的参数没有，则删除
            {
                bool res = users.Contains(item.UserName);
                if (!res) {//说明数据库中有，输入参数没有
                    resUsersDelete.Add(item.UserName);
                }
            }
            string sql = "";


            string insertSql = @"
insert into e_userstock(UserName,groupid)
 ";
            string insertSqlCompa = insertSql;
            for (int ai = 0; ai <resUsersAdd.Count(); ai++)
            {//组建添加sql
                string name = resUsersAdd[ai];
            
                if (ai == resUsersAdd.Count() - 1)//最后一个，不要union all
                {
                    insertSql += @" 
                     select '" + name + "'," + id + @" ; 
                ";
                }
                else {
                    insertSql += @" 
                     select '" + name + "'," + id + @" union all     
                    ";
                }
            }

            if (insertSqlCompa != insertSql) {
                sql += insertSql;
            }



          

            foreach (var deleteitem in resUsersDelete)//组建删除sql
            {
                string deleteSql = @"
delete from  e_userstock where UserName = '"+ deleteitem + @"' ;
 ";
                sql += deleteSql;
            }



            if (!string.IsNullOrEmpty(sql))
            {
                using (var cn = new MySqlConnection(sqlconnectionString))
                {
                    cn.Open();
                    sql += "update "+ database2 + @".e_userstockgroup set UpdateTime='" + DateTime.Now.ToString() + "' where id = " + id + ";";
                    int i = cn.Execute(sql);

                    cn.Close();

                    return i;
                }
            }
            else {
                return 1;
            }


         
        }

        public static UserStock GetModelStock(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                var i = cn.Query<UserStock>(@"
select * from "+ database2 + @".e_userstockgroup  where id = @id
", new {  id = id });

                cn.Close();

                return i.FirstOrDefault();
            }
        }


        public static IEnumerable<UserStock> GetModelStockByUserName(string userName)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                var i = cn.Query<UserStock>(@"
select * from "+ database2 + @".e_userstockgroup as a, "+ database2 + @".e_userstock as b 
WHERE a.id = b.groupid and b.userName = @userName;
", new { userName = userName });

                cn.Close();

                return i;
            }
        }




        public static IEnumerable<UserStock> GetOtherUsers(int id,string userName)
        {
           

            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                var i = cn.Query<UserStock>(@"
select * from "+ database2 + @".e_userstock  where groupid != @id and UserName in ("+userName+@")
", new { id = id  });

                cn.Close();

                return i;
            }
        }




        public static UserStock GetModelStockAll(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                var i = cn.Query<UserStock>(@"
select c.id as GroupID,c.GroupName,c.Value,group_concat(c.UserName separator ',') as UserName
from 
(select b.*,a.UserName from e_userstockgroup as b left join e_userstock as a on a.groupid = b.id
WHERE 1=1 and b.id =  @id
) as c
group by c.id,c.GroupName,c.Value
", new { id = id });

                cn.Close();

                return i.FirstOrDefault();
            }
        }




        /// <summary>
        /// 得到可以绑定的用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IEnumerable< UserStock> GetUserBindList(int id)
        {
            using (var cn = new MySqlConnection(sqlconnectionString))
            {
                cn.Open();

                var i = cn.Query<UserStock>(@"
select * from "+ database2 + @".e_userstock  where groupid = @id
", new { id = id });

                cn.Close();

                return i;
            }
        }


    }
}
