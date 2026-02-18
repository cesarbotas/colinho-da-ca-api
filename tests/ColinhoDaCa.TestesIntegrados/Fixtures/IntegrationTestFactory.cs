using ColinhoDaCa.Infra.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Reflection;
using Testcontainers.PostgreSql;

namespace ColinhoDaCa.TestesIntegrados.Fixtures;

public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:15-alpine")
        .WithImage("postgres:15-alpine")
        .WithDatabase("colinho_test")
        .WithUsername("testuser")
        .WithPassword("testpass")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ColinhoDaCaContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<ColinhoDaCaContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        // Executar scripts SQL da pasta database
        await ExecuteDatabaseScriptsAsync();
    }
    
    private async Task ExecuteDatabaseScriptsAsync()
    {
        var connectionString = _dbContainer.GetConnectionString();
        
        // Obter o diretório raiz do projeto
        var currentDirectory = Directory.GetCurrentDirectory();
        var projectRoot = FindProjectRoot(currentDirectory);
        var scriptsPath = Path.Combine(projectRoot, "database", "scripts");
        
        if (!Directory.Exists(scriptsPath))
        {
            throw new DirectoryNotFoundException($"Scripts directory not found: {scriptsPath}");
        }
        
        // Obter todos os arquivos SQL ordenados por nome
        var sqlFiles = Directory.GetFiles(scriptsPath, "*.sql")
            .OrderBy(f => f)
            .ToArray();
        
        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        foreach (var sqlFile in sqlFiles)
        {
            var sqlContent = await File.ReadAllTextAsync(sqlFile);
            
            // Dividir por comandos separados por GO ou ponto e vírgula
            var commands = sqlContent.Split(new[] { "GO", ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(cmd => !string.IsNullOrWhiteSpace(cmd))
                .ToArray();
            
            foreach (var command in commands)
            {
                var trimmedCommand = command.Trim();
                if (!string.IsNullOrWhiteSpace(trimmedCommand))
                {
                    try
                    {
                        using var cmd = new NpgsqlCommand(trimmedCommand, connection);
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        // Log e continuar - alguns comandos podem falhar se já existirem
                        Console.WriteLine($"Warning executing script {Path.GetFileName(sqlFile)}: {ex.Message}");
                    }
                }
            }
        }
        
        // Executar scripts de teste
        await ExecuteTestScriptsAsync(connection);
    }
    
    private string FindProjectRoot(string currentPath)
    {
        var directory = new DirectoryInfo(currentPath);
        
        while (directory != null)
        {
            if (directory.GetDirectories("database").Any())
            {
                return directory.FullName;
            }
            directory = directory.Parent;
        }
        
        throw new DirectoryNotFoundException("Could not find project root with database folder");
    }
    
    private async Task ExecuteTestScriptsAsync(NpgsqlConnection connection)
    {
        var testScriptsPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts");
        
        if (!Directory.Exists(testScriptsPath))
        {
            return; // Não há scripts de teste
        }
        
        var testSqlFiles = Directory.GetFiles(testScriptsPath, "*.sql")
            .OrderBy(f => f)
            .ToArray();
        
        foreach (var sqlFile in testSqlFiles)
        {
            var sqlContent = await File.ReadAllTextAsync(sqlFile);
            
            var commands = sqlContent.Split(new[] { "GO", ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(cmd => !string.IsNullOrWhiteSpace(cmd))
                .ToArray();
            
            foreach (var command in commands)
            {
                var trimmedCommand = command.Trim();
                if (!string.IsNullOrWhiteSpace(trimmedCommand))
                {
                    try
                    {
                        using var cmd = new NpgsqlCommand(trimmedCommand, connection);
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Warning executing test script {Path.GetFileName(sqlFile)}: {ex.Message}");
                    }
                }
            }
        }
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
