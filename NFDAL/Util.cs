
using System.Configuration;

namespace NFDAL
{
    public class Util
    {
        public static string CONNECTION_STRING = ConfigurationManager.AppSettings["NFConnection"].ToString();
    }
}
