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
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        // Allow any origin, but don't allow credentials
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAnyOrigin");
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


app.MapControllers();
app.Run();

