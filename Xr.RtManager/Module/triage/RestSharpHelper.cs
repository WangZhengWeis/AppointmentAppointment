using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xr.Http.RestSharp;

namespace Xr.RtManager.Module.triage
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
        public static void ReturnResult<T>(string Url, Dictionary<string, string> prament, Method methoh, Action<IRestResponse<T>> callback) where T : new()
        {
            var client = new RestSharpClient(AppContext.AppConfig.serverUrl + Url);
            var Params = "";
            if (prament.Count != 0)
            {
                Params = "?" + string.Join("&", prament.Select(x => x.Key + "=" + x.Value).ToArray());
            }
            Log4net.LogHelper.Info("请求地址：" + AppContext.AppConfig.serverUrl + Url + Params);
            client.ExecuteAsync<T>(new RestRequest(Params, methoh), callback);
        }
    }
}
