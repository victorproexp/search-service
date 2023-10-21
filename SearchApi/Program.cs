using System.Diagnostics;

using SearchApi;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:5271");
                      });
});

var app = builder.Build();
//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

IDatabase database = new Database();

ISearchLogic searchLogic = SearchFactory.CreateSearchLogic(database);
ISearchLogic normalizedSearchLogic = SearchFactory.CreateNormalizedSearchLogic(database);

app.MapGet("/search", (string query, int? maxAmount) => {
    return searchLogic.Search(query.Split(","), maxAmount ?? 10);
});

app.MapGet("/nsearch", (string query, int? maxAmount) => {
    return normalizedSearchLogic.Search(query.Split(","), maxAmount ?? 10);
});

app.MapGet("/version", () => {
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
});

app.Run();
