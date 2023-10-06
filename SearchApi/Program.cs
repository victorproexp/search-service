using System.Diagnostics;

using ConsoleSearch;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("https://localhost:7209");
                      });
});

var app = builder.Build();
app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/search", (string query, int? maxAmount) => {
    return SearchFactory.CreateSearchLogic().Search(query.Split(","), maxAmount ?? 10);
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
