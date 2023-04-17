using Joy_Template;
using Joy_Template.Data.Tables;
using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVCTemplate.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IHtmlHelperFactory, HtmlHelperFactory>();

var connectionString = builder.Configuration.GetConnectionString("devConnection");

builder.Services.ConfigureServices(services => services
    .AddJoyAuthentication()
    .AddJoyDbContext(connectionString)
    .AddSystemMonitor()
    .AddTable<TbUser>()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
