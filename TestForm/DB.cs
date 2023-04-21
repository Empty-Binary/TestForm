using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForm
{
    public class SalesPrice
    {        
        public string affiliate_id;
        public string pay_type;
        public string pay_price;
    }

    public class DB
    {
        public static List<SalesPrice> GetSales()
        {
            try
            {
                List<SalesPrice> salesPrice_list = new List<SalesPrice>();
                //using (SqlConnection conn = new SqlConnection("server = 182.162.90.215; uid = unospay; pwd =dnshtmvpdl0223!; database =unospay;"))
                //{
                //    string sql = "select * from Sales_price with(nolock) where affiliate_id = 'test_demo2' and  CONVERT(CHAR(8), timestamp, 112) = '20221102'";

                //    conn.Open();
                //    SqlCommand cmd = new SqlCommand(sql, conn);
                //    SqlDataReader rdr = cmd.ExecuteReader();
                //    while (rdr.Read())
                //    {
                //        SalesPrice salesPrice = new SalesPrice();
                //        salesPrice.pay_type = Convert.ToString(rdr["pay_type"]);
                //        salesPrice.pay_price = Convert.ToString(rdr["pay_price"]);
                //        salesPrice_list.Add(salesPrice);
                //    }
                //}
                Console.WriteLine(salesPrice_list);

                return salesPrice_list;
            }
            catch (Exception e)
            {
                return new List<SalesPrice>();
            }
        }
    }
}
