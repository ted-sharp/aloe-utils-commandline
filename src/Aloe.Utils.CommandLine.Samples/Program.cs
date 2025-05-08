using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Aloe.Utils.CommandLine;

public static class AppConfig
{
    public static readonly List<string> FlagArgs =
    [
        "--standalone",
        "--debug",
        "--verbose",
    ];

    public static readonly List<string> ShortArgs =
    [
        "-u",
        "-p",
        "-c",
    ];

    public static readonly Dictionary<string, string> Aliases = new()
    {
        { "--standalone", "AppSettings:IsStandalone" },
        { "--debug", "AppSettings:IsDebug" },
        { "-u", "AppSettings:Username" },
        { "-p", "AppSettings:Password" },
    };

    public static IConfigurationRoot CreateConfigurationRoot(string[] args)
    {
        var processedArgs = ArgsHelper.PreprocessArgs(
            args,
            FlagArgs,
            ShortArgs);

        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets<Program>(optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(processedArgs, Aliases);

        return builder.Build();
    }
}

public class AppSettings
{
    public bool IsStandalone { get; set; }
    public bool IsDebug { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // 設定の追加
        builder.Configuration.AddConfiguration(AppConfig.CreateConfigurationRoot(args));

        // サービスの登録
        builder.Services.AddSingleton<App>();
        builder.Services.Configure<AppSettings>(
            builder.Configuration.GetSection("AppSettings"));

        var host = builder.Build();
        var app = host.Services.GetRequiredService<App>();
        return app.Run();
    }
}

public class App
{
    private readonly IConfiguration _configuration;
    private readonly AppSettings _appSettings;

    public App(IConfiguration configuration, IOptions<AppSettings> appSettings)
    {
        this._configuration = configuration;
        this._appSettings = appSettings.Value;
    }

    public int Run()
    {
        Console.WriteLine("アプリケーションの設定:");
        Console.WriteLine($"スタンドアロンモード: {this._appSettings.IsStandalone}");
        Console.WriteLine($"デバッグモード: {this._appSettings.IsDebug}");
        Console.WriteLine($"ユーザー名: {this._appSettings.Username}");
        Console.WriteLine($"パスワード: {this._appSettings.Password}");

        return 0;
    }
}
