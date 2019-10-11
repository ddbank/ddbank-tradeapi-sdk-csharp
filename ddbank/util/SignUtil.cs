using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ddbank_tradeapi_sdk_csharp.ddbank.util
{
    class SignUtil
    {
        //将集合key以ascii码从小到大排序
        public static Dictionary<string, string> AsciiDictionary(Dictionary<string, string> sArray)
        {
            Dictionary<string, string> asciiDic = new Dictionary<string, string>();
            string[] arrKeys = sArray.Keys.ToArray();
            Array.Sort(arrKeys, string.CompareOrdinal);
            foreach (var key in arrKeys)
            {
                string value = sArray[key];
                asciiDic.Add(key, value);
            }
            return asciiDic;
        }

        //获取Unix时间戳
        public static string GetTimeStamp()
        {
            System.DateTime time = System.DateTime.Now;
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }

        private static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        //URL参数化，value进行URLEncode编码(UTF-8)
        public static string ToRequestParam(Dictionary<string, string> param)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in param)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append("&");
                }
                try
                {
                    stringBuilder.Append(kvp.Key).Append("=").Append(UrlEncode(kvp.Value, UTF8Encoding.UTF8));
                }
                catch (Exception ignored)
                {
                    //TODO
                }
            }
            return stringBuilder.ToString();
        }

        //DDBank API签名
        public static string Sign(string encryptText, string encryptKey)
        {
            //第1次明文消息进行base64编码。
            string base64Message = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(encryptText));
            //HMACSHA1加密
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = System.Text.Encoding.UTF8.GetBytes(encryptKey);
            byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(base64Message);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            //第2次HMAC_SHA1签名的二进制结果进行base64编码，得到最终签名。
            return Convert.ToBase64String(hashBytes);
        }

        //SHA1加密
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }

        //URLEncode(UTF-8)，输出与Java相同的URLEncode编码
        public static string UrlEncode(string str, Encoding encoding)
        {
            if (encoding == null)
            {
                encoding = UTF8Encoding.UTF8;
            }
            byte[] bytes = encoding.GetBytes(str);
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                char ch = (char)bytes[i];
                if (ch == ' ')
                {
                    num++;
                }
                else if (!IsUrlSafeChar(ch))
                {
                    //非url安全字符
                    num2++;
                }
            }

            if (num == 0 && num2 == 0)
            {
                //不包含空格和特殊字符
                return str;
            }
            //包含特殊字符，每个特殊字符转为3个字符，所以长度+2x
            byte[] buffer = new byte[bytes.Length + (num2 * 2)];
            int num3 = 0;
            for (int j = 0; j < bytes.Length; j++)
            {
                byte num6 = bytes[j];
                char ch2 = (char)num6;
                if (IsUrlSafeChar(ch2))
                {
                    buffer[num3++] = num6;
                }
                else if (ch2 == ' ')
                {
                    //0x2B代表 ascii码中的+，url编码时候会把空格编写为+
                    buffer[num3++] = 0x2B;
                }
                else
                {
                    //特殊符号转换
                    buffer[num3++] = 0x25;  //代表%
                    //8位向右移动四位后与1111按位与，即保留高前四位 ，比如/为2f，则结果保留了2
                    buffer[num3++] = (byte)IntToHex((num6 >> 4) & 15);
                    //8位，与00001111按位与，即保留后四位，比如/为2f，则结果保留了f
                    buffer[num3++] = (byte)IntToHex(num6 & 15);
                }
            }
            return encoding.GetString(buffer);
        }

        private static bool IsUrlSafeChar(char ch)
        {
            if ((((ch < 'a') || (ch > 'z')) && ((ch < 'A') || (ch > 'Z'))) && ((ch < '0') || (ch > '9')))
            {
                switch (ch)
                {
                    case '(':
                    case ')':
                    case '*':
                    case '-':
                    case '.':
                    case '!':
                        break;  //安全字符

                    case '+':
                    case ',':
                        return false;  //非安全字符
                    default:   //非安全字符
                        if (ch != '_')
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        //n为0-f 
        private static char IntToHex(int n)
        {
            if (n <= 9)
            {
                //0x30十进制是48 对应ASCII码是0  
                return (char)(n + 0x30);
            }
            //0x41十进制是 65对应ASCII码是A
            return (char)((n - 10) + 0x41);
        }

    }
}
