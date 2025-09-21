using Asp.Versioning;
using Common.Logging;
using EventBus.Messages.Common;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application.Extensions;
using Ordering.Application.Handlers;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;
using Ordering.Infrastructure.Repositories;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Serilog Configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();

//Register Application Services.
builder.Services.AddApplicationServices();

builder.Services.AddInfraServices(builder.Configuration);

// MessageConsumer Class Registration.
builder.Services.AddScoped<BasketOrderingConsumer>();

builder.Services.AddScoped<BasketOrderingConsumerV2>();


//Register Infrastructure Services.
//builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(CheckoutOrderCommandHandler).Assembly
};
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Ordering.API", Version = "v1" }); });

// MassTransit Registration.
builder.Services.AddMassTransit(config => {

    //Mark this as consumer.
    config.AddConsumer<BasketOrderingConsumer>();
    config.AddConsumer<BasketOrderingConsumerV2>();
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        //Provide the queue name with the consumer settings.
        cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketOrderingConsumer>(ctx);
        });

        //V2 Version
        cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueueV2, c =>
        {
            c.ConfigureConsumer<BasketOrderingConsumerV2>(ctx);
        });
    });
});

builder.Services.AddMassTransitHostedService();

//builder.Services.AddDbContext<OrderContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OrderingConnectionString")));
//builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
//builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

app.MigrateDatabase<OrderContext>((context, services) => {

    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();    
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
