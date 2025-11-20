using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Features;
using Wallet.Helper;
using Wallet.Interfaces;
using Wallet.Models;
using Wallet.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 104857600; // Set limit (e.g., 100MB)
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // Set limit (e.g., 100MB)
});

// Read notary nodes list from settings
builder.Services.Configure<List<NotaryNode>>(builder.Configuration.GetSection("Win:Beta:NotaryNodes"));

// Add services to the container.
builder.Services.AddSingleton<LanguageService>();
builder.Services.AddScoped<TranslationService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddRazorPages();


builder.Services.AddSession();
builder.Services.AddAuthentication("AuthToken")
    .AddCookie("AuthToken", options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "AuthToken";
        //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Adjust as needed
        options.Cookie.SameSite = SameSiteMode.Lax; // Ensure compatibility
        options.ExpireTimeSpan = TimeSpan.FromMinutes(1440);
        options.SlidingExpiration = true;
        options.LoginPath = "/auth/login";
    });
builder.Services.AddHttpClient();
builder.Services.AddScoped<ITsgCoreServiceHelper, TsgCoreServiceHelper>();
builder.Services.AddScoped<ICustomerMapperHelper, CustomerMapperHelper>();
builder.Services.AddScoped<IVLinkServiceHelper, VLinkServiceHelper>();
builder.Services.AddScoped<UserContextService>();
builder.Services.AddScoped<TSGAuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();
builder.Services.AddScoped<IVLinkAccessAvailabilityService, VLinkAccessAvailabilityService>();
builder.Services.AddScoped<IVLinkMapperService, VLinkMapperService>();
builder.Services.AddScoped<IFileAttachmentServiceHelper, FileAttachmentServiceHelper>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IQRCodeReaderService, QRCodeReaderService>();
builder.Services.AddScoped<IVerifyMapperService, VerifyMapperService>();
builder.Services.AddScoped<ITextHelper, TextHelper>();
builder.Services.AddScoped<ICustomerMapperHelper, CustomerMapperHelper>();
builder.Services.AddScoped<IUserMapperHelper, UserMapperHelper>();
builder.Services.AddScoped<IFileAttachmentMapperService, FileAttachmentMapperService>();
builder.Services.AddScoped<IGetVerifiedMapperService, GetVerifiedMapperService>();
builder.Services.AddScoped<IItemNoteMapperService, ItemNoteMapperService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<BalanceHelper>();
builder.Services.AddScoped<AccountStatementHelper>();
builder.Services.AddScoped<FxDealHelper>();
builder.Services.AddScoped<InstantPaymentHelper>();

builder.Services.AddScoped<LiquidationPreferenceHelper>();


builder.Services.AddScoped<CurrencySettingsHelper>();
builder.Services.AddScoped<PasswordHelper>();

builder.Services.AddTransient<Wallet.Pages.Shared.Components.BalanceCardsModel>();
builder.Services.AddTransient<Wallet.Pages.Shared.Components.FxCardsModel>();
builder.Services.AddTransient<Wallet.Pages.Shared.Components.PaymentCardsModel>();


builder.Services.AddDataProtection().SetApplicationName("WKYC").PersistKeysToFileSystem(new DirectoryInfo(@"./KeyStore"));
// builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR(options => options.MaximumReceiveMessageSize = 802400);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2); // Configure request header timeout
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2); // Configure keep-alive timeout
    serverOptions.Limits.MaxRequestBodySize = 524288000; // 500 MB file size limit
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


// Preload languages
using (var scope = app.Services.CreateScope())
{
    var languageService = scope.ServiceProvider.GetRequiredService<LanguageService>();
    await languageService.LoadLanguagesAsync(); // Load languages asynchronously
}

// Custom middleware for refreshing tokens and checking authentication
app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    Console.WriteLine("Middleware for token refresh and authentication check invoked.");
    Console.WriteLine($"Request path: {path}");

    if (!path.StartsWithSegments("/auth/login") && !path.StartsWithSegments("/css") &&
        !path.StartsWithSegments("/images") && !path.StartsWithSegments("/js"))
    {
        Console.WriteLine("Path conditions met, processing authentication.");
        var authService = context.RequestServices.GetRequiredService<ITsgCoreServiceHelper>();
        Console.WriteLine("Attempting to refresh token.");
        bool tokenRefreshed = await authService.RefreshAccessTokenAndRebuildCookie(false);
        if (!tokenRefreshed)
        {
            Console.WriteLine("Token refresh failed, redirecting to login.");
            context.Response.Redirect("/auth/login");
            return;
        }
        Console.WriteLine("Token did not require a refresh.");
        var isAuthenticated = context.User.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated)
        {
            Console.WriteLine("User not authenticated, redirecting to login.");
            context.Response.Redirect("/auth/login");
            return;
        }
        Console.WriteLine($"Username: {context.User.Identity?.Name}");
        Console.WriteLine("User is authenticated.");
        foreach (var claim in context.User.Claims)
        {
            Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
        }
    }
    else
    {
        Console.WriteLine("Path conditions not met, skipping authentication check.");
    }

    await next.Invoke();
});

app.Use(async (context, next) =>
{
    foreach (var cookie in context.Request.Cookies)
    {
        Console.WriteLine($"Cookie BEFORE: {cookie.Key} = {cookie.Value}");
    }

    await next();

    if (context.Response.StatusCode == 401)
    {
        Console.WriteLine("Received 401 Unauthorized response, redirecting to login.");
        context.Response.Redirect("/login");
    }
    foreach (var cookie in context.Request.Cookies)
    {
        Console.WriteLine($"Cookie AFter: {cookie.Key} = {cookie.Value}");
    }

});
// app.UseSession();
app.UseAntiforgery();

app.MapRazorPages();
//app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();

