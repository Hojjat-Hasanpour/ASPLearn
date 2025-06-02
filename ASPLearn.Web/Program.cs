using ASPLearn.Core.Services;
using ASPLearn.Core.Services.Interfaces;
using ASPLearn.DataLayer.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.Convertors;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ASPLearnContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ASPLearnConnection")));
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
    /*options.Cookie.Name = "ASPLearnCookie";
	options.Cookie.HttpOnly = true;
	options.SlidingExpiration = true;
	options.ReturnUrlParameter = "ReturnUrl";
	options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
	{
		OnRedirectToLogin = context =>
		{
			context.Response.Redirect(context.RedirectUri);
			return Task.CompletedTask;
		}
	};*/
});
builder.Services.AddRazorPages();

builder.Services.AddTransient<IUserService, UserService>(); // IoC AddTransient: for each request, a new instance is created
builder.Services.AddTransient<IWalletService, WalletService>(); // IoC
builder.Services.AddTransient<IAdminService, AdminService>(); // IoC
builder.Services.AddTransient<ICourseService, CourseService>(); //IoC
builder.Services.AddTransient<IOrderService, OrderService>(); //Inversion of control
builder.Services.AddTransient<IViewRenderService, RenderViewToString>(); // IoC
builder.Services.AddTransient<IForumService, ForumService>(); //IoC

//builder.Services.Configure<FormOptions>(options =>
//{
//	options.MultipartBodyLengthLimit = 104857600; // 100 MB
//}); // for upload file size limit for other platforms

var app = builder.Build();

//builder.Services.AddMvc();

//builder.Services.AddRe
//app.MapGet("/", () => "Hello World!");
//app.UseMvcWithDefaultRoute();

app.Use(async (context, next) =>
{
    await next.Invoke();
    if (context.Response.StatusCode == 404)
    {
        context.Response.Redirect("/Home/Error404");
    }
});

app.Use(async (context, next) =>
{
    if (context.Request.Path.Value!.ToString().ToLower().StartsWith("/coursefilesonline"))
    {
        var callingUrl = context.Request.Headers["Referer"].ToString();
        if (callingUrl != "" && callingUrl.Contains("localhost:7007")) //For localhost, otherwise domain url
            await next.Invoke();
        else
            context.Response.Redirect("/Login");
    }
    else
    {
        await next.Invoke();
    }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.MapAreaControllerRoute(
    name: "MyAreaUserPanel",
    areaName: "UserPanel",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");



//app.UseEndpoints(endpoints =>
//{
//	endpoints.MapAreaControllerRoute(
//		name: "MyAreaUserPanel",
//		areaName: "UserPanel",
//		pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
//	endpoints.MapControllerRoute(
//		name: "default",
//		pattern: "{controller=Home}/{action=Index}/{id?}");
//});

app.Run();
