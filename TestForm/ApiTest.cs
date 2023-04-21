using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{
    public partial class ApiTest : Form
    {
        public ApiTest()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            InitializeComponent();
        }

        private void ApiTest_Load(object sender, EventArgs e)
        {
            //test1();// GET방식 서버연결 상태체크 잘됨
            test2();// <-이놈이 문제
        }


        public void test1()
        {
            string url = "https://apidev.cosmo.or.kr:8443/cup/v2/healthChecks";

            try
            {
                string clientId = "yJCPWAVYNjQpl5yy1fHfUk56df4Rgdox";
                string clientSecret = "MSSiUHM+tH8C6sVtPXjEnDSoQopp";
                string method = "GET";

                UriBuilder uriBuilder = new UriBuilder(url);
                string path = uriBuilder.Path;
                string query = (!string.IsNullOrEmpty(uriBuilder.Query) && uriBuilder.Query[0].Equals('?')) ? uriBuilder.Query.Substring(1) : uriBuilder.Query;
                string authorization = HmacUtils.Generate(path, query, method, clientId, clientSecret);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.ContentType = "application/json; charset=utf-8";
                request.Headers["Authorization"] = authorization;
                request.Timeout = 30 * 1000;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    Stream respGetStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(respGetStream);
                    string result = reader.ReadToEnd();

                    Console.WriteLine(result);
                }
            }
            catch (Exception err)
            {
                throw;
            }
        }

        public void test2()
        {
            string url = "https://apidev.cosmo.or.kr:8443/cup/v2/pos/received";

            try
            {
                string clientId = "yJCPWAVYNjQpl5yy1fHfUk56df4Rgdox";
                string clientSecret = "MSSiUHM+tH8C6sVtPXjEnDSoQopp";
                string method = "POST";

                //인증값 생성
                UriBuilder uriBuilder = new UriBuilder(url);
                string path = uriBuilder.Path;
                string query = (!string.IsNullOrEmpty(uriBuilder.Query) && uriBuilder.Query[0].Equals('?')) ? uriBuilder.Query.Substring(1) : uriBuilder.Query;
                string authorization = HmacUtils.Generate(path, query, method, clientId, clientSecret);


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.ContentType = "application/json";
                request.Headers["Authorization"] = authorization;
                request.Timeout = 30 * 1000;
                //request.KeepAlive = false;
                //request.ProtocolVersion = HttpVersion.Version11;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                request.Referer = "http://www.google.com/";

                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    JObject json = new JObject(new JProperty("box_id", "123"));
                    writer.Write(json.ToString());
                }

                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string result = String.Empty;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseGetStream = response.GetResponseStream())
                    {
                        using(StreamReader reader = new StreamReader(responseGetStream))
                        {
                            result = reader.ReadToEnd();

                        }
                    }
                }
                JObject jObject = JObject.Parse(result);
            }
            catch (Exception err)
            {
                throw;
            }
        }

        public class HmacUtils
        {
            private const string ALGORITHM = "HmacSHA256";
            public static string Generate(string path,
            string query,
            string method,
            string accessKey,
            string secretKey)
            {
                string datetime = DateTime.Now.ToUniversalTime().ToString("yyMMddTHHmmssZ");
                string message = String.Format("{0}{1}{2}{3}", datetime, method, path, query);
                Encoding encoding = new UTF8Encoding();
                string signature = ByteToString(new HMACSHA256(encoding.GetBytes(secretKey)).ComputeHash(encoding.GetBytes(message)));
                return String.Format("CEA algorithm={0}, access-key={1}, signed-date={2}, signature={3}", ALGORITHM, accessKey, datetime, signature);
            }
            private static string ByteToString(byte[] buff)
            {
                string sbinary = "";
                for (int i = 0; i < buff.Length; i++)
                {
                    sbinary += buff[i].ToString("x2");
                }
                return sbinary;
            }
        }


    }
}
