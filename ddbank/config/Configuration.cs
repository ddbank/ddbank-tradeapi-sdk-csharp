
namespace ddbank_tradeapi_sdk_csharp.ddbank.config
{
    public class Configuration
    {
        public static string API_KEY = "<Your ApiKey>";

        public static string API_SECRET = "<Your ApiSecret>";

        public static string CUSTOM_CODE = "<Your CustomCode>";

        public static string API_ENDPOINT = "<Your Target Domain>";



        public static string HEADER_KEY = "X-CA-ACCESSKEY";

        public static string HEADER_TS = "X-CA-TIMESTAMP";

        public static string HEADER_NONCE = "X-CA-NONCE";

        public static string HEADER_SIGN = "X-CA-SIGNATURE";

        public static string MEDIA_TYPE_JSON = "application/json; charset=UTF-8";

        public static string URI_STOCK_QUERY = "/api/v1/stock/list";

        public static string URI_STOCK_SUBMIT = "/api/v1/stock/placeOrder";
    }
}
