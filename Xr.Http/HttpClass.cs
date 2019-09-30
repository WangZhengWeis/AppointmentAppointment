using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Xr;

namespace Xr.Http
{
    public class HttpClass
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static String loginPost(String url)
        {
            CookieEntity.cookie = new CookieContainer();
            string result = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                System.GC.Collect();    // 请求之前 做一次垃圾回收
                System.Net.ServicePointManager.DefaultConnectionLimit = 20;

                // 打印 请求地址及参数
                Xr.Log4net.LogHelper.Info("请求数据：" + url);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Accept = "*/*";
                request.Method = "POST";
                request.UserAgent = "Mozilla/5.0";
                request.CookieContainer = CookieEntity.cookie;
                request.KeepAlive = false;          // 保持短链接
                request.Timeout = 1 * 60 * 1000;    // 1分钟，以防网络超时

                response = (HttpWebResponse)request.GetResponse();
                string cookieheader = request.CookieContainer.GetCookieHeader(new Uri(url));
                CookieEntity.cookie.SetCookies(new Uri(url), cookieheader);

                stream = response.GetResponseStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                result = reader.ReadToEnd().Trim();

                // 打印响应结果
                Xr.Log4net.LogHelper.Info("响应结果：" + result);
            }
            catch (Exception e)
            {
                result = "{'state': false, 'message':'" + e.Message + "'}";
                if (e.Message.Equals("远程服务器返回错误: (400) 错误的请求。"))
                {
                    throw new Exception(e.Message);
                }
                else if (e.Message.Equals("The remote server returned an error:(405) Method Not Allowed."))
                {
                    throw new Exception(e.Message);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (response != null) response.Close();
                if (request != null) request.Abort();
            }
            return result;
        }

        /// <summary>
        /// post模拟表单提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static String httpPost(String url)
        {
            string result = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                System.GC.Collect();    // 请求之前 做一次垃圾回收
                System.Net.ServicePointManager.DefaultConnectionLimit = 20;

                // 打印 请求地址及参数
                Xr.Log4net.LogHelper.Info("请求数据：" + url);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Accept = "*/*";
                request.Method = "POST";
                request.UserAgent = "Mozilla/5.0";
                request.CookieContainer = CookieEntity.cookie;
                request.KeepAlive = false;          // 保持短链接
                request.Timeout = 1 * 60 * 1000;    // 1分钟，以防网络超时

                response = (HttpWebResponse)request.GetResponse();
                CookieEntity.cookie = request.CookieContainer;

                stream = response.GetResponseStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                result = reader.ReadToEnd().Trim();
                // 打印响应结果
                Xr.Log4net.LogHelper.Info("响应结果：" + result);
                //if (result.Equals("远程服务器返回错误: (404) 未找到。"))
                //{
                //    throw new Exception(result);
                //}
            }
            catch (Exception e)
            {
                result = "{'state': false, 'message':'" + e.Message + "'}";
                if (e.Message.Equals("远程服务器返回错误: (400) 错误的请求。"))
                {
                    throw new Exception(e.Message);
                }
                else if (e.Message.Equals("The remote server returned an error:(405) Method Not Allowed."))
                {
                    throw new Exception(e.Message);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (response != null) response.Close();
                if (request != null) request.Abort();
            }
            return result;
        }

        /// <summary>
        /// post模拟表单提交(参数太长的时候用)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static String httpPost(String url, String postDataStr)
        {
            string result = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                System.GC.Collect();    // 请求之前 做一次垃圾回收
                System.Net.ServicePointManager.DefaultConnectionLimit = 20;

                // 打印 请求地址及参数
                Xr.Log4net.LogHelper.Info("请求数据：" + url + postDataStr);
                request = (HttpWebRequest)WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(postDataStr);
                //request.Accept = "*/*";
                request.Method = "POST";
                //request.UserAgent = "Mozilla/5.0";
                request.CookieContainer = CookieEntity.cookie;
                request.KeepAlive = false;          // 保持短链接
                request.Timeout = 1 * 60 * 1000;    // 1分钟，以防网络超时
                request.ContentType = "application/x-www-form-urlencoded";
                Encoding encoding = Encoding.UTF8;//根据网站的编码自定义
                byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，
                request.ContentLength = postData.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
                requestStream.Close();
                
                response = (HttpWebResponse)request.GetResponse();
                CookieEntity.cookie = request.CookieContainer;

                stream = response.GetResponseStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                result = reader.ReadToEnd().Trim();
                // 打印响应结果
                Xr.Log4net.LogHelper.Info("响应结果：" + result);
                //if (result.Equals("远程服务器返回错误: (404) 未找到。"))
                //{
                //    throw new Exception(result);
                //}
            }
            catch (Exception e)
            {
                result = "{'state': false, 'message':'" + e.Message + "'}";
                if (e.Message.Equals("远程服务器返回错误: (400) 错误的请求。"))
                {
                    throw new Exception(e.Message);
                }
                else if (e.Message.Equals("The remote server returned an error:(405) Method Not Allowed."))
                {
                    throw new Exception(e.Message);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (response != null) response.Close();
                if (request != null) request.Abort();
            }
            return result;
        }

        /// <summary>
        /// post模拟表单提交(参数太长的时候用),可自定义超时时间
        /// </summary>
        /// <param name="url">请求路径</param>
        /// <param name="postDataStr">参数</param>
        /// <param name="minute">超时时间，单位分钟，例如：10就是10分钟</param>
        /// <returns></returns>
        public static String httpPost(String url, String postDataStr, int minute)
        {
            string result = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                System.GC.Collect();    // 请求之前 做一次垃圾回收
                System.Net.ServicePointManager.DefaultConnectionLimit = 20;

                // 打印 请求地址及参数
                Xr.Log4net.LogHelper.Info("请求数据：" + url + postDataStr);
                request = (HttpWebRequest)WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(postDataStr);
                //request.Accept = "*/*";
                request.Method = "POST";
                //request.UserAgent = "Mozilla/5.0";
                request.CookieContainer = CookieEntity.cookie;
                request.KeepAlive = false;          // 保持短链接
                request.Timeout = minute * 60 * 1000;    // 1分钟，以防网络超时
                request.ContentType = "application/x-www-form-urlencoded";
                Encoding encoding = Encoding.UTF8;//根据网站的编码自定义
                byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，
                request.ContentLength = postData.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
                requestStream.Close();

                response = (HttpWebResponse)request.GetResponse();
                CookieEntity.cookie = request.CookieContainer;

                stream = response.GetResponseStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                result = reader.ReadToEnd().Trim();
                // 打印响应结果
                Xr.Log4net.LogHelper.Info("响应结果：" + result);
                //if (result.Equals("远程服务器返回错误: (404) 未找到。"))
                //{
                //    throw new Exception(result);
                //}
            }
            catch (Exception e)
            {
                result = "{'state': false, 'message':'" + e.Message + "'}";
                if (e.Message.Equals("远程服务器返回错误: (400) 错误的请求。"))
                {
                    throw new Exception(e.Message);
                }
                else if (e.Message.Equals("The remote server returned an error:(405) Method Not Allowed."))
                {
                    throw new Exception(e.Message);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (response != null) response.Close();
                if (request != null) request.Abort();
            }
            return result;
        }


        /// <summary>  
        /// 使用Post方法获取字符串结果  
        /// 主要用于文件上传(可携带表单数据)
        /// </summary>  
        /// <param name="url">请求地址</param>  
        /// <param name="formItems">Post表单文件、内容数据</param>  
        /// <returns></returns>  
        public static string PostForm(string url, List<FormItemModel> formItems)
        {
            string result = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                System.GC.Collect();    // 请求之前 做一次垃圾回收
                System.Net.ServicePointManager.DefaultConnectionLimit = 20;

                // 打印 请求地址及参数
                Xr.Log4net.LogHelper.Info("请求数据：" + url);
                request = WebRequest.Create(url) as HttpWebRequest;

                #region 初始化请求对象
                request.Accept = "*/*";
                request.Method = "POST";
                request.UserAgent = "Mozilla/5.0";
                request.CookieContainer = CookieEntity.cookie;
                request.KeepAlive = false;          // 保持短链接
                request.Timeout = 2 * 60 * 1000;    // 2分钟，以防网络超时 或 文件过大

                #endregion

                string boundary = "----" + DateTime.Now.Ticks.ToString("x");//分隔符  
                request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
                //请求流  
                var postStream = new MemoryStream();

                #region 处理Form表单请求内容
                //是否用Form上传文件  
                var formUploadFile = formItems != null && formItems.Count > 0;
                if (formUploadFile)
                {
                    //文件数据模板  
                    string fileFormdataTemplate =
                        "\r\n--" + boundary +
                        "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
                        "\r\nContent-Type: multipart/form-data" +
                        "\r\n\r\n";
                    //文本数据模板  
                    string dataFormdataTemplate =
                        "\r\n--" + boundary +
                        "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                        "\r\n\r\n{1}";
                    foreach (var item in formItems)
                    {
                        string formdata = null;
                        if (item.IsFile)
                        {
                            //上传文件  
                            formdata = string.Format(
                                fileFormdataTemplate,
                                item.Key, //表单键  
                                item.FileName);
                        }
                        else
                        {
                            //上传文本  
                            formdata = string.Format(
                                dataFormdataTemplate,
                                item.Key,
                                item.Value);
                        }

                        //统一处理  
                        byte[] formdataBytes = null;
                        //第一行不需要换行  
                        if (postStream.Length == 0)
                            formdataBytes = Encoding.UTF8.GetBytes(formdata.Substring(2, formdata.Length - 2));
                        else
                            formdataBytes = Encoding.UTF8.GetBytes(formdata);
                        postStream.Write(formdataBytes, 0, formdataBytes.Length);

                        //写入文件内容  
                        if (item.FileContent != null && item.FileContent.Length > 0)
                        {
                            using (var streams = item.FileContent)
                            {
                                byte[] buffer = new byte[1024];
                                int bytesRead = 0;
                                while ((bytesRead = streams.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    postStream.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                    //结尾  
                    var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                    postStream.Write(footer, 0, footer.Length);

                }
                else
                {
                    request.ContentType = "application/json;charset=UTF-8";
                }
                #endregion

                request.ContentLength = postStream.Length;

                #region 输入二进制流
                if (postStream != null)
                {
                    postStream.Position = 0;
                    //直接写入流  
                    Stream requestStream = request.GetRequestStream();

                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);
                    }
                    postStream.Close();//关闭文件访问  
                }
                #endregion

                response = (HttpWebResponse)request.GetResponse();
                CookieEntity.cookie = request.CookieContainer;

                stream = response.GetResponseStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                result = reader.ReadToEnd().Trim();

                // 打印响应结果
                Xr.Log4net.LogHelper.Info("响应结果：" + result);
            }
            catch (Exception e)
            {
                result = e.Message;
                if (e.Message.Equals("远程服务器返回错误: (400) 错误的请求。"))
                {
                    throw new Exception(e.Message);
                }
                else if (e.Message.Equals("The remote server returned an error:(405) Method Not Allowed."))
                {
                    throw new Exception(e.Message);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (response != null) response.Close();
                if (request != null) request.Abort();
            }
            return result; ;
        }
    
    }
}
