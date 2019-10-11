using System;
using System.Collections.Generic;
using System.Text;
using ddbank_tradeapi_sdk_csharp.ddbank.config;
using ddbank_tradeapi_sdk_csharp.ddbank.util;

namespace ddbank_tradeapi_sdk_csharp.ddbank.client
{
    class QueryStockClient
    {
        public static void Main(string[] args)
        {
            //参数
            Dictionary<string, string> reqParams = new Dictionary<string, string>();
            reqParams.Add("pageNum", "1");
            reqParams.Add("pageSize", "10");
            reqParams.Add("customCode", Configuration.CUSTOM_CODE);
            Query(Configuration.API_KEY, Configuration.API_SECRET, reqParams);
        }

        public static void Query(string apiKey, string apiSecret, Dictionary<string, string> param)
        {
            //API Secret 进行SHA1加密
            string secret = SignUtil.SHA1_Encrypt(apiSecret);
            //API请求方式
            string reqMethod = "GET";
            //API请求地址(domain + uri)
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(Configuration.API_ENDPOINT).Append(Configuration.URI_STOCK_QUERY);
            //参数key排序
            Dictionary<string, string> reqParamsAsc = SignUtil.AsciiDictionary(param);
            //GET参数URL参数化
            string getParam = SignUtil.ToRequestParam(reqParamsAsc);
            string bodyParam = "";//无
            string url = urlBuilder.Append("?").Append(getParam).ToString();
            //unix时间戳(13位)
            string timestamp = SignUtil.GetTimeStamp();
            //一次性随机字符串
            string nonce = System.Guid.NewGuid().ToString("N");
            //API请求Headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("X-CA-ACCESSKEY", apiKey);
            headers.Add("X-CA-TIMESTAMP", timestamp);
            headers.Add("X-CA-NONCE", nonce);
            //API请求签名
            string signStr = SignUtil.Sign(reqMethod + url + nonce + timestamp + bodyParam, secret);
            Console.WriteLine("signStr:" + signStr);
            headers.Add("X-CA-SIGNATURE", signStr);
            //post请求并调用
            string result = HttpUtil.Get(url, headers);

            Console.WriteLine("result:" + result);
            Console.ReadKey();
        }

    }
}
