using ColinhoDaCa.IoC;
using ColinhoDaCaApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog
    builder.Host.UseSerilog((context, configuration) =>
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code)
            .WriteTo.File(
                new Serilog.Formatting.Json.JsonFormatter(),
                "logs/colinho-api-.log",
                rollingInterval: RollingInterval.Day)
            .Enrich.WithProperty("Application", "ColinhoDaCaApi")
            .Enrich.FromLogContext());

    builder.Services.RegistraDependencias(builder.Configuration);

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddControllers();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:8080",
                    "https://colinho-da-ca-site.vercel.app"
                )
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    var jwtSecret = builder.Configuration["Jwt:Secret"];
    var key = Encoding.ASCII.GetBytes(jwtSecret);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();

    app.UseHttpsRedirection();

    app.UseCors("AllowFrontend");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    //Log.Fatal(ex, $"Aplica��o finalizada de forma inesperada: {ex.Message}");
}
finally
{
    Log.CloseAndFlush();
}