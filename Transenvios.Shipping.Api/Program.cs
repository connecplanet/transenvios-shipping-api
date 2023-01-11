using Microsoft.EntityFrameworkCore;
using Transenvios.Shipping.Api.Domains.CatalogService;
using Transenvios.Shipping.Api.Domains.DriverService;
using Transenvios.Shipping.Api.Infraestructure;
using Transenvios.Shipping.Api.Domains.ClientService;
using Transenvios.Shipping.Api.Domains.RoutesService;
using Transenvios.Shipping.Api.Domains.ShipmentOrderService;
using Transenvios.Shipping.Api.Domains.UserService;
using Transenvios.Shipping.Api.Mediators.CatalogService;
using Transenvios.Shipping.Api.Mediators.ClientService;
using Transenvios.Shipping.Api.Mediators.DriverService;
using Transenvios.Shipping.Api.Mediators.RoutesService;
using Transenvios.Shipping.Api.Mediators.ShipmentOrderService;
using Transenvios.Shipping.Api.Mediators.UserService;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

// Add services to the container.
{
    var services = builder.Services;

    // Update ASPNETCORE_ENVIRONMENT={Development} to use MySQL
    if (env.IsEnvironment("NonProd"))
    {
        services.AddDbContext<IDbContext, SqliteDataContext>(ServiceLifetime.Transient); // SqlLite
    }
    else if (env.IsStaging())
    {
        services.AddDbContext<DataContext>(ServiceLifetime.Transient); // MsSQL
    }
    else
    {
        services.AddDbContext<IDbContext, MySqlDataContext>(ServiceLifetime.Transient); // MySQL
    }

    services.AddCors();
    services.AddControllers();

    // configure automapper with all automapper profiles from this assembly
    services.AddAutoMapper(typeof(Program));

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
    // configure DI for application services
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IPasswordReset, PasswordMediator>();;
    services.AddScoped<UserProcessor>();
    services.AddScoped<IRegisterUser, UserMediator>();
    services.AddTransient<IGetAuthorizeUser, UserMediator>();
    services.AddScoped<IGetUser, UserMediator>();
    services.AddScoped<IUpdateUser, UserMediator>();
    services.AddScoped<IRemoveUser, UserMediator>();
    services.AddScoped<ShipmentOrderProcessor>();
    services.AddScoped<IDriverStorage, DriverMediator>();
    services.AddScoped<DriverProcessor>();
    services.AddScoped<ICatalogStorage<City>, CityMediator>();
    services.AddScoped<ICatalogQuery<ShipmentRoute>, ShipmentRouteMediator>();
    services.AddScoped<ICatalogQuery<IdType>, IdTypeMediator>();
    services.AddScoped<ICatalogQuery<Country>, CountryMediator>();
    services.AddScoped<ClientProcessor>();
    services.AddTransient<IClientStorage, ClientMediator>();
    services.AddScoped<IOrderChargesCalculator, ShipmentOrderMediator>();
    services.AddScoped<IOrderStorage, ShipmentOrderMediator>();
    services.AddScoped<CityProcessor>();
    
    services.AddScoped<RoutesProcessor>();
    services.AddScoped<IRouteStorage, RouteMediator>();
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<IDbContext>();
    dataContext.Migrate(builder.Environment);
}

// Configure the HTTP request pipeline.
{
    // global cors policy
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    if (app.Environment.IsDevelopment() || env.IsEnvironment("NonProd") || env.IsEnvironment("QA"))
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
