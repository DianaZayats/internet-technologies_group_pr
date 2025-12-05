using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SlayLib.Data;
using SlayLib.Models;
using SlayLib.Repositories;
using SlayLib.Interfaces;
using Microsoft.Extensions.Options; 
using Mit.Models.Configurations;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Runtime.InteropServices;
using WebAppCore.Authorization;
using Microsoft.AspNetCore.Localization;
using WebAppCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

var mitConfig = builder.Configuration.Get<MitConfiguration>();

if (mitConfig == null)
    throw new InvalidOperationException("MitConfiguration is not set in configuration.");

builder.Services.AddSingleton(mitConfig);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    connectionString = builder.Configuration.GetConnectionString("LinuxDockerConnection");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("WebAppCore")));

builder.Services.AddScoped<IMitRepository, SlaySqlServerRepository>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure application cookie (login path, etc.)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Реєстрація обробників авторизації
builder.Services.AddScoped<IAuthorizationHandler, IsResourceOwnerHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsRecipeOwnerHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MinimumWorkingHoursHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ForumAccessHandler>();

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    // Policy for users that have IsVerifiedClient claim
    options.AddPolicy("VerifiedClientOnly", policy =>
        policy.RequireClaim("IsVerifiedClient", "true"));

    // Політика для редагування ресурсу: тільки власник може редагувати
    options.AddPolicy("CanEditResource", policy =>
        policy.Requirements.Add(new IsResourceOwnerRequirement()));

    // Політика для доступу до Premium сторінки: мінімум 100 робочих годин
    options.AddPolicy("CanAccessPremium", policy =>
        policy.Requirements.Add(new MinimumWorkingHoursRequirement(100)));

    options.AddPolicy("ForumAccessPolicy", policy =>
       policy.AddRequirements(new ForumAccessRequirement()));
});

// MVC controllers: by default require authenticated user
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})
.AddViewLocalization()
.AddDataAnnotationsLocalization(options =>
{
    
    options.DataAnnotationLocalizerProvider = (type, factory) =>
    {
     
        return factory.Create(typeof(WebAppCore.Resources.ValidationMessages));
    };
});
builder.Services.AddRazorPages();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var key = context.User.Identity?.IsAuthenticated == true
            ? context.User.Identity.Name ?? "authenticated"
            : "anonymous";

        if (context.User.Identity?.IsAuthenticated == true)
        {
            return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 120,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            });
        }
        else
        {
            return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 40,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            });
        }
    });

    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        context.HttpContext.Response.ContentType = "text/html; charset=utf-8";

        var executor = context.HttpContext.RequestServices.GetRequiredService<Microsoft.AspNetCore.Mvc.Infrastructure.IActionResultExecutor<Microsoft.AspNetCore.Mvc.ViewResult>>();
        var viewResult = new Microsoft.AspNetCore.Mvc.ViewResult
        {
            ViewName = "/Views/Shared/TooManyRequests.cshtml"
        };

        await executor.ExecuteAsync(
            new Microsoft.AspNetCore.Mvc.ActionContext(
                context.HttpContext,
                context.HttpContext.GetRouteData() ?? new Microsoft.AspNetCore.Routing.RouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
            ),
            viewResult
        );
    };
});

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

var supportedCultureNames = new[] { "en-US", "uk-UA", "cs-CZ" };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultureInfos = supportedCultureNames
        .Select(name => new CultureInfo(name))
        .ToList();

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = cultureInfos;
    options.SupportedUICultures = cultureInfos;

    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringCultureProvider(supportedCultureNames),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});

var app = builder.Build();

// Seed database with initial recipes
await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await SeedData.SeedRecipesAsync(context, userManager);
}

var localizationOptions = app.Services
    .GetRequiredService<IOptions<RequestLocalizationOptions>>()
    .Value;
app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRateLimiter();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

