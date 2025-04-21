using DependencyInjectionMVC_Auth_Fixed.Interfaces;
using DependencyInjectionMVC_Auth_Fixed.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<HelloMessageService>();
builder.Services.AddScoped<AdminMessageService>();

builder.Services.AddScoped<IMessageService>(provider =>
{
    var context = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
    var user = context?.User;
    var isAdmin = user?.IsInRole("Admin") ?? false;

    return isAdmin
        ? provider.GetRequiredService<AdminMessageService>()
        : provider.GetRequiredService<HelloMessageService>();
});

builder.Services.AddAuthentication("MyCookie")
    .AddCookie("MyCookie", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
