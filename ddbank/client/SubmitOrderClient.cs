using System;
using System.Collections.Generic;
using ddbank_tradeapi_sdk_csharp.ddbank.config;
using ddbank_tradeapi_sdk_csharp.ddbank.util;

namespace ddbank_tradeapi_sdk_csharp.ddbank.client
{
    class SubmitOrderClient
    {
        //public static void Main(string[] args)
        //{
        //    //参数
        //    Dictionary<string, string> reqParams = new Dictionary<string, string>();
        //    reqParams.Add("stoneIds", "5d82faf131d5a71d1e379216,5d82ea3b31d5a71d1e35d16a");
        //    reqParams.Add("brandName","品牌商test");
        //    reqParams.Add("storeName","店铺名称test");
        //    reqParams.Add("storeAddr","店铺地址test");
        //    reqParams.Add("storeType","2");//直营(1)加盟(2)
        //    reqParams.Add("customName","客户姓名test");
        //    reqParams.Add("brandOrder","NO201909010001");
        //    reqParams.Add("deliverDate", "2019-09-01");
	//    reqParams.Add("contactName", "某某某");
	//    reqParams.Add("contactPhone", "13666666666");
        //
        //    Submit(Configuration.API_KEY, Configuration.API_SECRET, reqParams);
        //}

        public static void Submit(string apiKey, string apiSecret, Dictionary<string, string> param)
        {
            //API Secret 进行SHA1加密
            string secret = SignUtil.SHA1_Encrypt(apiSecret);
            //API请求方式（大写）
            string reqMethod = "POST";
            //API请求地址(domain + uri)
            string reqUrl = Configuration.API_ENDPOINT + Configuration.URI_STOCK_SUBMIT;
            //POST参数key排序及URL参数化
            Dictionary<string, string> reqParamsAsc = SignUtil.AsciiDictionary(param);
            //URL参数化
            string postParams = SignUtil.ToRequestParam(reqParamsAsc);
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
            string signStr = SignUtil.Sign(reqMethod + reqUrl + nonce + timestamp + postParams, secret);
            Console.WriteLine("signStr:" + signStr);
            headers.Add("X-CA-SIGNATURE", signStr);
            //post请求并调用
            string result = HttpUtil.Post(reqUrl, param, headers);

            Console.WriteLine("result:" + result);
            Console.ReadKey();
        }
    }
}
