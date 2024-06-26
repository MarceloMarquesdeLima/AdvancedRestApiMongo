using AdvancedRestApiMongo.Interfaces;
using AdvancedRestApiMongo.Profies;
using AdvancedRestApiMongo.Services;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>((options)=>
{
    options.GeneralRules = new List<RateLimitRule>()
    {
        new RateLimitRule()
        {
            Endpoint = "*",
            Limit = 10,
            Period = "3m"
        }
    };
});

builder.Services.AddInMemoryRateLimiting();

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddScoped<IUser, UserService>();

builder.Services.AddControllers().AddOData(option=>option.Select().Filter().OrderBy());
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseIpRateLimiting();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
