using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{
    public partial class FormAlimTalk : Form
    {
        public FormAlimTalk()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<SalesPrice> saelsList = DB.GetSales();

            string data = "매장명:우노스페이\r\n\r\n";
            data += "[매출정보]\r\n";
            data += "집계일자 : 2022-11-03 00:00~24:00\r\n";            

            int totalPrice = 0;
            foreach(SalesPrice sales in saelsList)
            {
                if(sales.pay_type == "1")
                {

                }
                else if(sales.pay_type == "2")
                {

                }
                else if (sales.pay_type == "3")
                {

                }

                totalPrice += Convert.ToInt32(sales.pay_price);
            }

            data += "매출총액 : " + String.Format("{0:#,###}", totalPrice) + "원\r\n";

            sendAlimtalk(data);
        }


        private void sendAlimtalk(string data)
        {
            try
            {
                String callUrl = "http://221.139.14.189/API/alimtalk_api";
                string from = "01045937914";
                string to = "01063095259";
                string key = "ISCTQGBMVE31227";
                string template_code = "SJT_045030";

                String postData = String.Format("api_key={0}&template_code={1}&variable={2}&callback={3}&dstaddr={4}&next_type={5}&send_reserve={6}&send_reserve_date={7}"
                        , key, template_code, data, from, to, "1", "0", "");

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl);
                // 인코딩 UTF-8
                byte[] sendData = UTF8Encoding.UTF8.GetBytes(postData);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = sendData.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(sendData, 0, sendData.Length);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                string returnStr = streamReader.ReadToEnd();
                streamReader.Close();
                httpWebResponse.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("err");
            }
        }
    }
}
