using Azure;
using BusinessAccessLayer.Abstraction;
using BusinessAccessLayer.Implementation;
using DataAccessLayer.DbServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyJyotishGApi.RazorPay;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
builder.Services.Configure<RazorpaySettings>(builder.Configuration.GetSection("Razorpay"));
builder.Services.AddScoped<IAdminServices, AdminServices>();
builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<IJyotishServices, JyotishServices>();
builder.Services.AddScoped<IPendingJyotishServices, PendingJyotishServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<RazorpayService>();
builder.Services.AddScoped<IRazorPayServices,RazorPayServices>();
builder.Services.AddScoped<ICallServices, CallServices>();
builder.Services.AddScoped<IChat, ChatServices>();
builder.Services.AddSignalR();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    
.AddJwtBearer("Scheme1", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Scheme1:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Scheme1:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Scheme1:Key"]))
    };
})
.AddJwtBearer("Scheme2", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Scheme2:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Scheme2:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Scheme2:Key"]))
    };
})
.AddJwtBearer("Scheme3", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Scheme3:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Scheme3:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Scheme3:Key"]))
    };
})
.AddJwtBearer("Scheme4", options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = builder.Configuration["Jwt:Scheme4:Issuer"],
         ValidAudience = builder.Configuration["Jwt:Scheme4:Audience"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Scheme4:Key"]))
     };
 });

// Add authorization policies if needed
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Policy1", policy =>
        policy.RequireAuthenticatedUser().AddAuthenticationSchemes("Scheme1"));
    options.AddPolicy("Policy2", policy =>
        policy.RequireAuthenticatedUser().AddAuthenticationSchemes("Scheme2"));
    options.AddPolicy("Policy3", policy =>
        policy.RequireAuthenticatedUser().AddAuthenticationSchemes("Scheme3"));
    options.AddPolicy("Policy4", policy =>
        policy.RequireAuthenticatedUser().AddAuthenticationSchemes("Scheme4"));
});


/*builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});*/
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseWebSockets();

app.Use(async (context, next) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        await next();
    }
    else
    {
        await next();
    }
});
builder.Services.AddHttpClient();

app.MapGet("/jyotish/{name}", async (string name, HttpClient httpClient) =>
{
    var apiUrlForFeature = "https://localhost:7118/Api/Admin/GetAllFeatures";
    var apiUrlForGetPlan = "https://localhost:7118/Api/Admin/getPlan";

// Call the external API to get posts
var response1 = await httpClient.GetAsync(apiUrlForFeature);
var response2 = await httpClient.GetAsync(apiUrlForGetPlan);

    dynamic features=null;
    dynamic plan=null;
    if (response1.IsSuccessStatusCode)
    {
        // Deserialize the JSON response into a list of Post objects
        var jsonResponse = await response1.Content.ReadAsStringAsync();
         features = JsonSerializer.Deserialize<dynamic>(jsonResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

    } 
    if (response2.IsSuccessStatusCode)
    {
        // Deserialize the JSON response into a list of Post objects
        var jsonResponse = await response2.Content.ReadAsStringAsync();
         plan = JsonSerializer.Deserialize <dynamic> (jsonResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

    }
    
        List<string> list = new List<string>();
        List<string> planList = new List<string>();
    
        if (features != null)
        {
            foreach (var item in features)
            {
                list.Add(item.serviceUrl);
            }
        }
    if (list.Contains($"/jyotish/{name}")) {
        bool checkValidation = false;
        foreach(var data in plan)
        {
            planList.Add(data.url);
        }

        if (planList.Contains($"/jyotish/{name}"))
        {
            return Results.Ok("valid");

        }
        else
        {
            return Results.StatusCode(404);
        }

    }
    else
    {
        return Results.Ok("valid");
    }
});


app.MapControllers();
app.MapHub<CallHub>("/callHub");
app.Run();

