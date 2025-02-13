using Azure;
using BusinessAccessLayer.Abstraction;
using BusinessAccessLayer.Implementation;
using DataAccessLayer.DbServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ModelAccessLayer.Models;
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
builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<IAdminServices, AdminServices>();
builder.Services.AddScoped<IJyotishServices, JyotishServices>();
builder.Services.AddScoped<IPendingJyotishServices, PendingJyotishServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<RazorpayService>();
builder.Services.AddScoped<IRazorPayServices, RazorPayServices>();
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
    options.AddPolicy("DynamicCorsPolicy", builder =>
    {
        builder.SetIsOriginAllowed((host) => true)   // Allow any origin dynamically
               .AllowAnyMethod()                    // Allow any HTTP method (GET, POST, etc.)
               .AllowAnyHeader()                    // Allow any headers
               .AllowCredentials();                 // Allow credentials (cookies, HTTP authentication, etc.)
    });
});
// Register the background service as scoped (not singleton)
builder.Services.AddHostedService<DatabaseBackgroundService>();
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
    app.UseDeveloperExceptionPage();
}
app.UseCors("DynamicCorsPolicy");
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
app.MapHub<CallHub>("/callhub");

app.MapControllers();
app.Run();


// Background Service to update the database
public class DatabaseBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DatabaseBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Get current date and time
                DateTime currentDate = DateTime.Now;

                // Calculate the time for the next day at midnight (or any time you want)
                DateTime nextRunTime = currentDate.Date.AddDays(1);  // This sets the next run to be at midnight
                TimeSpan timeToNextRun = nextRunTime - currentDate;

                // Wait until it's time for the next run (next day)
                await Task.Delay(timeToNextRun, stoppingToken);
                using (var scope = _scopeFactory.CreateScope())
                {
                    // Resolve the scoped service (MyDbContext) within the scope
                    var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    var res = _dbContext.PurchaseAdvertisement
      .Where(e => e.status)
      .Include(e => e.advertisement)
      .ToList();

                List<PurchaseAdvertisement> pa = new List<PurchaseAdvertisement>();
                if (res.Count > 0)
                {
                    foreach (var item in res)
                    {
                            var startDate = item.StartDate;
                        // Calculate the endDate based on Plantype
                        var endDate = item.advertisement.Plantype.ToLower() switch
                        {
                            "week" => item.StartDate.AddDays(item.advertisement.Duration * 7), // Adding weeks
                            "month" => item.StartDate.AddMonths(item.advertisement.Duration), // Adding months
                            _ => item.StartDate.AddDays(item.advertisement.Duration) // Default to adding days
                        };

                        // Compare StartDate and endDate to current date
                        if (startDate.Date == DateTime.Now.Date)
                        {
                            item.activeStatus = true;
                            pa.Add(item);
                        }
                        else if (endDate.Date < DateTime.Now.Date)
                        {
                            item.status = false;
                            pa.Add(item);
                        }
                    }
                }

                if (pa.Count > 0)
                {
                    // Only update if there are items to update
                    _dbContext.PurchaseAdvertisement.UpdateRange(pa);
                    await _dbContext.SaveChangesAsync(); // Don't forget to save the changes
                }

                    var planData = _dbContext.PackageManager.Where(e => e.Status && e.ExpiryDate<DateTime.Now.Date).ToList();
                    if (planData.Count > 0)
                    {
                        foreach(var item in planData)
                        {
                            item.Status = false;
                        }
                        _dbContext.PackageManager.UpdateRange(planData);
                        await _dbContext.SaveChangesAsync();
                    }
                    
            }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error occurred: {ex.Message}");
            }

           
        }
    }
}
