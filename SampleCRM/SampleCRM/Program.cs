using SampleCRM.Common;
using SampleCRM.Contexts;
using SampleCRM.Identity;
using SampleCRM.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using static SampleCRM.Identity.AuthorizationPolicies;
using IdentityRole = SampleCRM.Models.TableModels.IdentityRole;
using IdentityUser = SampleCRM.Models.TableModels.IdentityUser;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info( "―――――――――――――――― GetCurrentClassLogger ――――――――――――――――" );

try
{
    var builder = WebApplication.CreateBuilder( args );

    // MariaDBへの接続設定
    // MariaDB自体はmysqlと接続互換を持って開発が行われているので、接続はmysqlのものを使用して行う。
    var sampleCRMConnectionString = builder.Configuration.GetConnectionString( "SampleCRMConnection" );

    builder.Services.AddDbContext<SampleCRMContext>( options => options
        .UseMySql( sampleCRMConnectionString, ServerVersion.AutoDetect( sampleCRMConnectionString ) )
        .AddInterceptors()
        .EnableSensitiveDataLogging()
        .LogTo( Console.WriteLine, LogLevel.Information ) );

    builder.Services
        .AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<SampleCRMContext>()
        .AddErrorDescriber<JapaneseIdentityErrorDescriber>()
        .AddDefaultTokenProviders();

    builder.Services.AddTransient<UserResolverService>();

    // 認証系の設定
    builder.Services.Configure<IdentityOptions>( options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequireDigit = true;       // 数字必須
            options.Password.RequireLowercase = true;   // 大文字必須
            options.Password.RequireUppercase = true;   // 小文字必須
            options.Password.RequiredLength = 6;        // 6文字以上

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes( 20 );
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;

            options.Stores.SchemaVersion = IdentitySchemaVersions.Version2;
        } );

    // Cookieの設定
    builder.Services.ConfigureApplicationCookie( options =>
    {
        options.AccessDeniedPath = "/Account/AccessDenied";

        // ログインしてから再度ログインを求められるまでの期間（初期値は14日）
        options.ExpireTimeSpan = TimeSpan.FromDays( 14 );
    } );

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    builder.Services.AddAuthorization( options =>
    {
        options.AddPolicy( nameof( GlobalAdministratorOnlyPolicy ), GlobalAdministratorOnlyPolicy );
        options.AddPolicy( nameof( UserAdministratorPolicy ), UserAdministratorPolicy );
        options.AddPolicy( nameof( AnyRolePolicy ), AnyRolePolicy );
        options.AddPolicy( nameof( EditorPolicy ), EditorPolicy );
    } );

    // NLogのセットアップ for DI
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // ここまで設定した内容でアプリケーションの作成
    var app = builder.Build();

    // 初回起動時のSeedデータ投入
    using ( var scope = app.Services.CreateScope() )
    {
        var services = scope.ServiceProvider;
        SeedData.Initialize( services );
    }

    /// Configure the HTTP request pipeline.
    if ( app.Environment.IsDevelopment() || app.Environment.IsEnvironment( "Local" ) )
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler( "/Home/Error" );
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    // 認証ミドルウェアを使う旨宣言
    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}" );

    app.Run();
}
catch ( Exception ex )
{
    logger.Error( ex, $"Stopped program because of exception{Environment.NewLine}" );
    throw;
}
finally
{
    LogManager.Shutdown();
}