using System.Diagnostics;
using System.Text;

namespace SearchApi
{
    public class Utils
    {
        public static string CreateCacheKey(SearchParameters parameters)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(parameters.IsNormalized ? "N_" : "NN_");
            stringBuilder.Append(parameters.Query);
            stringBuilder.Append(':');
            stringBuilder.Append(parameters.MaxAmount);

            return stringBuilder.ToString();
        }
        
        static public Dictionary<string, string> GetApiVersion()
        {
            var properties = new Dictionary<string, string>
            {
                { "service", "Search API" }
            };
            var ver = FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion;
            if (ver != null)
            {
                properties.Add("version", ver);
            }  
            var hostName = System.Net.Dns.GetHostName();
            var ips = System.Net.Dns.GetHostAddressesAsync(hostName).GetAwaiter().GetResult();
            var ipa = ips.First().MapToIPv4().ToString();
            properties.Add("ip-address", ipa);
            return properties;
        }
    }
}
