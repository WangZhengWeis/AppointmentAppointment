using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xr.Http.RestSharp;

namespace Xr.RtCall.Model
{
    /// <summary>
    /// RestSharp帮助类
    /// </summary>
   public static class RestSharpHelper
    {
       /// <summary>
        /// RestSharp异步请求
       /// </summary>
       /// <typeparam name="T">List<string></typeparam>
       /// <param name="Url">请求地址</param>
       /// <param name="prament">请求参数</param>
        /// <param name="methoh">请求方式(GET, POST, PUT, HEAD, OPTIONS, DELETE)</param>
       /// <param name="callback">回调函数</param>
       public static void ReturnResult<T>(string Url, Dictionary<string, string> prament,Method methoh, Action<IRestResponse<T>> callback)where T:new()
       {
           var client = new RestSharpClient(AppContext.AppConfig.serverUrl+Url);
           var Params = "";
           if (prament.Count != 0)
           {
               Params = "?" + string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
           }
           Log4net.LogHelper.Info("请求地址：" + AppContext.AppConfig.serverUrl + Url + Params);
           client.ExecuteAsync<T>(new RestRequest(Params, methoh), callback);
       }
       /// <summary>
       /// RestSharp同步请求
       /// </summary>
       /// <param name="Url">接口基地址</param>
       /// <param name="prament">请求参数</param>
       /// <param name="method">请求方式</param>
       /// <returns></returns>
       public static string ReturnResult(string Url, string prament, Method method)
       {
           var client = new RestSharpClient(Url);
           var result = client.Execute(new RestRequest("?" + prament, method));
           return result.Content;
       }
       /// <summary>
       ///  RestSharp同步泛型请求
       /// </summary>
       /// <typeparam name="T">类型</typeparam>
       /// <param name="Url">地址</param>
       /// <param name="prament">参数</param>
       /// <param name="methoh">方式</param>
       /// <returns></returns>
       public static List<string> ReturnResults<T>(string Url, string prament, Method methoh) where T : new()
       {
           var client = new RestSharpClient(Url);
           var result = client.Execute<List<string>>(new RestRequest("?"+prament, methoh));
           return result;
       }
       public static string RetureResultes(string Url, string prament,Method method)
       {
           var client = new RestSharpClient(Url);
           var content = "";
           client.ExecuteAsync<List<string>>(new RestRequest("?" + prament, method), result =>
           {
               content = result.Content;
           });
           return content;
       }
   }
}
