using Backend_BankingApp.Utilities.Facade;
using Backend_BankingApp.Utilities.Flyweight;
using Backend_BankingApp.Utilities.Observer;
using Backend_BankingApp.Utilities.Proxy;
using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
     options.AddPolicy("AllowAll", policy =>
     {
          policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
     });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("scs")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<ServiceFlyweightFactory>();
builder.Services.AddTransient<RequestService>();
builder.Services.AddSingleton<Backend_BankingApp.Utilities.Observer.WebSocketManager>();
builder.Services.AddTransient<WebSocketService>();
builder.Services.AddTransient<UserService>();


builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddTransient<IUserService>(serviceProvider =>
{
     var realUserService = serviceProvider.GetRequiredService<UserService>(); 
     var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>(); 
     var logger = serviceProvider.GetRequiredService<ILogger<UserService>>();
     return new UserServiceProxy(realUserService, httpContextAccessor, logger); 
});

builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();
var webSocketOptions = new WebSocketOptions
{
     KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);


app.Use(async (context, next) =>
{
     if (context.Request.Path == "/api/websocket/connect")
     {
          if (context.WebSockets.IsWebSocketRequest)
          {
               var userId = context.Request.Query["userId"].ToString();

               if (string.IsNullOrEmpty(userId))
               {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("User ID is required.");
                    return;
               }

               var webSocket = await context.WebSockets.AcceptWebSocketAsync();
               var webSocketManager = context.RequestServices.GetRequiredService<Backend_BankingApp.Utilities.Observer.WebSocketManager>();

               await webSocketManager.Attach(userId, webSocket);

               var handler = new WebSocketHandler(webSocket, userId, webSocketManager);
               await handler.HandleAsync(CancellationToken.None);
          }
          else
          {
               context.Response.StatusCode = 400;
          }
     }
     else
     {
          await next();
     }
});
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
}


app.UseCors("AllowAll"); 

app.UseAuthorization();

app.MapControllers();

app.Run();