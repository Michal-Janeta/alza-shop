using AlzaShop.Api;
using AlzaShop.Core.Commands;
using AlzaShop.Core.Database;
using AlzaShop.Core.Product;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

builder.Services.AddDbContext<AlzaShopDbContext>(options => options.UseSqlServer("name=ConnectionStrings:AlzaShop", b => b.MigrationsAssembly("AlzaShop.Core")));
builder.Services.AddScoped(typeof(AlzaShopDbContext), typeof(AlzaShopDbContext));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ProductQueryHandler).Assembly);
     cfg.AddOpenBehavior(typeof(LoggingMediatorPipeline<,>));
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Installer.Register(builder.Services);

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<AlzaShopDbContext>();
    context?.Database.Migrate();
}

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
