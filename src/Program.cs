using Api.Infra.Database;
using Api.Shared.Helpers;
using Api.Shared.Bases;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var connectionString = builder.Configuration.GetConnectionString("IredeMulti");

builder.Services
    .AddDbContext<AppDbContext>(options =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
           mySqlOptions => mySqlOptions.MigrationsHistoryTable("__NotificationMigrations"));
    });

builder.Services.AddScoped<PaginationParams>();

builder.Services.AddScoped<PaginationHeaderFilter>();

builder.Services.AddAttributedServices();

builder.Services.AddControllers();

var swaggerConfig = builder.Configuration.GetSection("Swagger");

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(swaggerConfig["Version"], new OpenApiInfo
    {
        Title = swaggerConfig["Title"],
        Description = swaggerConfig["Description"],
        Version = swaggerConfig["Version"]
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => builder
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .WithMethods("POST", "GET", "PUT", "DELETE")
            .WithExposedHeaders("X-Total-Count", "X-Total-Pages", "X-Current-Page", "X-Page-Size")
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error applying migrations: {ex.Message}");
        }
    }
}

app.UseCors();

app.MapControllers();

await app.RunAsync();