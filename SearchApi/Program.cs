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
builder.Services.AddControllers();
builder.Services.AddSingleton<ISearchAggregator, SearchAggregator>();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/version", () => Utils.GetApiVersion());

app.UseAuthorization();

app.MapControllers();

app.Run();
