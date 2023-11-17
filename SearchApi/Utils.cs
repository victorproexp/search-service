using System.Diagnostics;

namespace SearchApi
{
    public class Utils
    {
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
