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

var searchAggregator = new SearchAggregator();

app.MapGet("/search", (string query, int? maxAmount) => 
    searchAggregator.GetSearchResult(query, maxAmount)
);

app.MapGet("/nsearch", (string query, int? maxAmount) => 
    searchAggregator.GetNormalizedSearchResult(query, maxAmount)
);

app.MapGet("/version", () => Utils.GetApiVersion());

app.Run();
