using Joy.Database;
using Joy_Template;
using Joy_Template.Controllers;
using Joy_Template.Data.Tables;
using Joy_Template.UiComponents.Base;
using Joy_Template.UiComponents.Version2;
using Joy_Template.Wizard_2._0;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MVCTemplate.Data;

internal class Program {
    private static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IJoyUi, JoyUi>();
        builder.Services.AddTransient<IHtmlHelperFactory, HtmlHelperFactory>();
        builder.Services.AddTransient<IHtmlHelperFactory<AppModel>, HtmlHelperFactory<AppModel>>();
        builder.Services.AddTransient<IHtmlHelperFactory<MyModel>, HtmlHelperFactory<MyModel>>();

        var connectionString = builder.Configuration.GetConnectionString("devConnection");

        builder.Services.AddJoyDatabase(options => options.ConnectionString(connectionString));

        builder.Services.ConfigureServices(services => services
            .AddJoyAuthentication()
            .AddJoyDbContext(connectionString)
            .AddSystemMonitor()
            .AddTable<TbUser>()
        );

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if(!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();


        app.UseRouting();


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");


        app.Run();
    }
}


