using System.Configuration;

namespace data
{
    public static class DBHelper
    {
        public static string ConnectionString
        {
            get
            {
                var connectionStr = ConfigurationManager.ConnectionStrings["BHCSConnection"].ConnectionString;
                return connectionStr;
            }
        }
    }
}
