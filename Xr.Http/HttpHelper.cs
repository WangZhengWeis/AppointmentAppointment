using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
/** ***********************************************************************
** 程序集			: HttpHelper
** 作者				: wzw
** 创建时间			: 2019-01-07
** 最后修改者		: wzw
** ***********************************************************************/
namespace Xr.Http
{
    /// <summary>
    /// HttpHelper请求帮助类
    /// </summary>
   public class HttpHelper
    {
        public static string CallRemote(string url, Dictionary<string, string> paramsList, HttpMethod method)
        {
            // System.GC.Collect();//强制回收，回收没有关闭的HTTP请求
            MyWebClient wc = new MyWebClient();
            if (url.StartsWith("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
            }
            if (method == HttpMethod.Get)
            {
                foreach (var pa in paramsList)
                {
                    wc.QueryString.Add(pa.Key, pa.Value);
                }
                byte[] byRemoteInfo;
                string sRemoteInfo = "";
                string result = "";
                try
                {
                    byRemoteInfo = wc.DownloadData(url);
                    sRemoteInfo = System.Text.Encoding.UTF8.GetString(byRemoteInfo);
                    result = sRemoteInfo;
                }
                catch
                {
                    //LogPrint("HTTP Get请求错误信息：" + ex.Message);
                }
                return result;
                //var byRemoteInfo = wc.DownloadData(url);
                //string sRemoteInfo = System.Text.Encoding.UTF8.GetString(byRemoteInfo);
                //return sRemoteInfo;
            }
            else
            {
                System.Collections.Specialized.NameValueCollection par = new System.Collections.Specialized.NameValueCollection();
                foreach (var pa in paramsList)
                {
                    par.Add(pa.Key, pa.Value);
                }
                byte[] byRemoteInfo;
                string sRemoteInfo = "";
                string result = "";
                try
                {
                    byRemoteInfo = wc.UploadValues(url, "post", par);
                    sRemoteInfo = System.Text.Encoding.UTF8.GetString(byRemoteInfo);
                    result = sRemoteInfo;
                }
                catch
                {
                    //ErrorInfor = ex.Message;
                    //LogPrint("HTTP Post请求错误信息：" + ex.Message);
                }
                return result;
            }
        }

        #region 请求证书
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion
    }

   public class MyWebClient : WebClient
   {
       protected override WebRequest GetWebRequest(Uri uri)
       {
           WebRequest w;
           w = base.GetWebRequest(uri);
           w.Timeout = 30 * 1000;
           w.Proxy = null;//去掉这条代码,坑能导致第一次访问很慢
           return w;
       }
   } 
}
