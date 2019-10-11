using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace ddbank_tradeapi_sdk_csharp.ddbank.util
{
    class HttpUtil
    {
        //HTTP请求(GET)
        public static string Get(string url, Dictionary<string, string> dic)
        {
            string result = "";
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            //Headers
            if (dic.Count > 0)
            {
                int i = 0;
                foreach (var item in dic)
                {
                    if (i >= 0)
                        req.Headers[item.Key] = item.Value;
                    i++;
                }
            }
            //添加参数 
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容 
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }

        //HTTP请求(POST)
        public static string Post(string url, object obj_model, Dictionary<string, string> headers = null)
        {
            string param = JsonConvert.SerializeObject(obj_model);
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            if (headers != null && headers.Count != 0)
            {
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(param);
            request.ContentLength = payload.Length;
            string strValue = "";
            try
            {
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();
                string StrDate = "";
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate;
                }
            }
            catch (Exception e)
            {
                strValue = e.Message;
            }
            return strValue;
        }
    }
}
