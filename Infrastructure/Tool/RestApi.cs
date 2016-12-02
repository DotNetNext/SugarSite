using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using SyntacticSugar;
using Infrastructure.DbModel;
using Infrastructure.Pub;

namespace Infrastructure.Tool
{
    public class RestApi<T> where T : class, new()
    {
        public T Get(string url, object pars)
        {
            var type = Method.GET;
            IRestResponse<T> reval = GetApiInfo(url, pars, type);
            return reval.Data;
        }
        public T Post(string url, object pars)
        {
            var type = Method.POST;
            IRestResponse<T> reval = GetApiInfo(url, pars, type);
            return reval.Data;
        }
        public T Delete(string url, object pars)
        {
            var type = Method.DELETE;
            IRestResponse<T> reval = GetApiInfo(url, pars, type);
            return reval.Data;
        }
        public T Put(string url, object pars)
        {
            var type = Method.PUT;
            IRestResponse<T> reval = GetApiInfo(url, pars, type);
            return reval.Data;
        }

        private static IRestResponse<T> GetApiInfo(string url, object pars, Method type)
        {
            var request = new RestRequest(type);
            if (pars != null)
                request.AddObject(pars);
            var cm = CacheManager<UserInfo>.GetInstance();
            string uniqueKey = PubGet.GetUserKey;
            if (cm.ContainsKey(uniqueKey)) {
                request.AddCookie(PubConst.UserUniqueKey, CookiesManager<string>.GetInstance()[PubConst.UserUniqueKey]);
            }
            var client = new RestClient(RequestInfo.HttpDomain + url);
            client.CookieContainer = new System.Net.CookieContainer();
            IRestResponse<T> reval = client.Execute<T>(request);
            if (reval.ErrorException != null)
            {
                PubMethod.WirteExp(new Exception(reval.Content));
                throw new Exception("请求出错");
            }
            return reval;
        }
    }
}