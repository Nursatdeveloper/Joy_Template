using Joy.Database;
using Joy_Template;
using Joy_Template.Data.Tables;
using Joy_Template.UiComponents.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MVCTemplate.Data;

internal class Program {
    private static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<IHtmlHelperFactory, HtmlHelperFactory>();

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

public class Page {
    public string Url { get; set; }
    public Args RouteArgs { get; set; }
    public Page(string? url) {
        Url = url;
    }
}

public class Args {
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Section {
    public List<Page> Pages { get; set; }
    public Section(List<Page> pages) {
        Pages = pages;
    }

    public void AddPage(Page page) {
        Pages.Add(page);
    }
}

public class AppSection: Section {
    public AppSection() : base(new List<Page>()) {
        Pages.AddRange(new[] {
            new Page("/ru/url"),
            new Page("/ru/hello"),
            new Page("/ru/{Id}/{Name}")
        });
    }


}
