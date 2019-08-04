namespace GameStore.Models
{
    public class Constant
    {
        public const string Resources = "Resources";

        public const string GAME = "Game";
        public const string CONSOLE = "Console";
        public const string ACCESSORIES = "Accessories";

        public const string Auth_Scheme = "Cookies";
        public const string Admin_Auth_Scheme = "AdminCookies";

        public const string REFERRAL_KEY = "USER_REFERRAL_KEY";

        public static string DomainNane = string.Empty;
        public static string DomainNaneTxt = string.Empty;

        public static string Phone_Number = string.Empty;
        public static string Phone_NumberTxt = string.Empty;


        public static string IgdbUserKey = string.Empty;
        public static string TelrSecretKey = string.Empty;
        public static string TelrAPIKey = string.Empty;
        public static string TelrAuthToken = string.Empty;
        public static string Currency = string.Empty;
        public static int TelrTestMode { get; set; }
        public static int StoreID { get; set; }
        public static int TelrMerchantID { get; set; }

        public static string FetchrLocationId = string.Empty;
        public static string FetchrTokenId = string.Empty;
        public static string FetchrAuthToken = string.Empty;
        public static string FetchrUserName = string.Empty;
        public static string FetchrPassword = string.Empty;

        public static class ImageCategory
        {
            public const string GameImage = "Game";
            public const string PlatformImage = "Platform";
            public const string GameEngineImage = "GameEngine";
            public const string AccessoriesImage = "Accessories";
            public const string ConsoleImage = "Console";

            public static class ImageType
            {
                public const string Logo = "Logo";
                public const string Cover = "Cover";
                public const string ScreenShot = "ScreenShot";
            }

        }

        public static class ImageType
        {               
            public const string Small = "Small";
            public const string Large = "Large";
            public const string Medium = "Medium";
            public const string Thumbnail = "Thumbnail";

            public static class IGdb
            {
                public const string Logo = "t_thumb";
                public const string Small = "t_micro";
                public const string Large = "t_screenshot_big";
                public const string Medium = "t_screenshot_med";
                public const string Thumbnail = "t_cover_big";
            }
        }
    }

    public class Xml_Nodes
    {
        public const string TRANSACTIONS = "transactions";
        public const string TRANSACTION = "transaction";
        public const string ID = "id";
        public const string PREV_ID = "prev_id";
        public const string INIT_ID = "init_id";
        public const string TYPE = "type";
        public const string CLASS = "class";
        public const string AUTH = "auth";
        public const string AMOUNT = "amount";
        public const string CURRENCY = "currency";
        public const string DECRIPTION = "description";
        public const string CARTID = "cartid";
        public const string TEST = "test";
        public const string DATE = "date";
        public const string STORE = "store";
        public const string NAME = "name";
        public const string CARD = "card";
        public const string NUMBER = "number";
        public const string COUNTRY = "country";
        public const string COUNTRY_ISO = "country_iso";
        public const string NOTECOUNT = "notecount";
        public const string NOTES = "notes";
        public const string NOTE = "note";
        public const string TEXT = "text";
        public const string REF = "ref";

        public const string STATUS = "status";
        public const string STATUS_TXT = "statustxt";
        public const string CODE = "code";
        public const string MESSAGE = "message";

        public const string BILLING = "billing";
        public const string TITLE = "title";
        public const string FNAME = "fname";
        public const string SNAME = "sname";
        public const string FULL_NAME = "fullname";
        public const string ADDR1 = "addr1";
        public const string ADDR2 = "addr2";
        public const string ADDR3 = "addr3";
        public const string CITY = "city";
        public const string REGION = "region";
        public const string ZIP = "zip";
        public const string TEL = "tel";
        public const string EMAIL = "email";
        public const string IP = "ip";
    }
}
