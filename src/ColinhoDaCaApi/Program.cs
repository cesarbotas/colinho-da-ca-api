using ColinhoDaCa.IoC;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.RegistraDependencias(builder.Configuration);

    builder.Services.AddControllers();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins(
                "http://localhost:8080",
                "https://colinho-da-ca-site.vercel.app/"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
    });

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

    app.UseCors("AllowFrontend");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    //Log.Fatal(ex, $"Aplicação finalizada de forma inesperada: {ex.Message}");
}
finally
{
    //Log.CloseAndFlush();
}