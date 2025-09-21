using Common.Logging;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Repositories;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Serilog Configuration
builder.Host.UseSerilog(Logging.ConfigureLogger);

//Register AutoMapper.
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//Register MediatR.
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(CreateDiscountCommandHandler).Assembly
};

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

//Application Services.
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddGrpc();

var app = builder.Build();

// Migrate the database.
app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}


app.UseRouting();

//app.UseAuthorization();

app.UseEndpoints(epoints =>
{
    epoints.MapGrpcService<DiscountService>();
    epoints.MapGet("/", async context => 
    {
        await context.Response.WriteAsync("Communication with gRpc endpoints must be made through a gRPC client.");  
    });

});





app.Run();


