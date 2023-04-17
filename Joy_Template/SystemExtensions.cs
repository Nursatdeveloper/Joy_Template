using Joy_Template.Data.Tables;
using Joy_Template.Sources.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MVCTemplate.Data;
using MVCTemplate.Sources.Repository;

namespace Joy_Template {
    public static class SystemExtensions {
        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection, Func<IServiceCollection, IServiceCollection> services) {
            return services(serviceCollection);
        }

        public static IServiceCollection AddJoyDbContext(this IServiceCollection services, string connectionString) {
            //services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            services.AddDbContextFactory<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            return services;
        }

        public static IServiceCollection AddSystemMonitor(this IServiceCollection services) {
            services.AddScoped<ISystemMonitor, SystemMonitor>();
            services.AddScoped<IRepositoryProvider, RepositoryProvider>();
            return services;
        }

        public static IServiceCollection AddTable<TbModel>(this IServiceCollection services) where TbModel : TbBase {
            services.AddScoped<IRepositoryBase<ApplicationDbContext, TbModel>, RepositoryBase<ApplicationDbContext, TbModel>>();
            return services;
        }

        public static IServiceCollection AddJoyAuthentication(this IServiceCollection services) {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(options =>
                   {
                       options.LoginPath = "/User/Login";
                   });
            return services;
        }

    }



}
