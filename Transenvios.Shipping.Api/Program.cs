using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Domains.UserService.AuthorizationEntity;
using Transenvios.Shipping.Api.Domains.UserService.UserPage;
using Transenvios.Shipping.Api.Infraestructure;
using Transenvios.Shipping.Api.Mediators.ShipmentOrderService.ShipmentOrderPage;
using Transenvios.Shipping.Api.Mediators.UserService.ForgotPasswordPage;
using Transenvios.Shipping.Api.Mediators.UserService.UserPage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    var services = builder.Services;
    var env = builder.Environment;

    // Update ASPNETCORE_ENVIRONMENT={Development} to use MySQL
    if (env.IsProduction() || env.IsDevelopment())
    {

        services.AddDbContext<DataContext, MySqlDataContext>(ServiceLifetime.Transient);
    }
    else if (env.IsStaging())
    {
        services.AddDbContext<DataContext, SqliteDataContext>(ServiceLifetime.Transient);
    }
    else
    {
        services.AddDbContext<DataContext>(ServiceLifetime.Transient);
    }

    services.AddCors();
    services.AddControllers();

    // configure automapper with all automapper profiles from this assembly
    services.AddAutoMapper(typeof(Program));

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    // configure DI for application services
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IPasswordReset, EmailMediator>();;
    services.AddScoped<UserProcessor>();
    services.AddScoped<IRegisterUser, UserMediator>();
    services.AddTransient<IGetAuthorizeUser, UserMediator>();
    services.AddScoped<IGetUser, UserMediator>();
    services.AddScoped<IUpdateUser, UserMediator>();
    services.AddScoped<IRemoveUser, UserMediator>();
    services.AddScoped<ShipmentOrderProcessor>();
    services.AddScoped<ICalculateShipmentCharges, ShipmentOrderMediator>();
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
    DbInitializer.Initialize(dataContext, builder.Environment);
}

// Configure the HTTP request pipeline.
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}
app.Run();
