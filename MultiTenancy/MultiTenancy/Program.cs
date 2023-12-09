using Microsoft.EntityFrameworkCore;
using MultiTenancy.Data;
using MultiTenancy.Services;
using MultiTenancy.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITenantServices,TenantServices>();
builder.Services.AddScoped<IProductServices,ProductServices>();
builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection(nameof(TenantSettings)));
TenantSettings options = new();
builder.Configuration.GetSection(nameof(TenantSettings)).Bind(options);

var dpProvider = options.Defults.DbProvider;
if(dpProvider.ToLower() == "mssql")
{
    builder.Services.AddDbContext<AppDbcontext>(s => s.UseSqlServer());
}
foreach(var tenant in options.Tenants)
{
    var connectionString=tenant.ConnectionString??options.Defults.ConnectionString;
    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var dbContext=scope.ServiceProvider.GetRequiredService<AppDbcontext>();
    dbContext.Database.SetConnectionString(connectionString);
    if(dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
}


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
