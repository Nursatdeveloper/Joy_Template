using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joy.Database {
    public static class Extensions {
        public static IServiceCollection AddJoyDatabase(this IServiceCollection services, Func<JoyDatabaseOptions, ConnectionString> func) {
            var connectionString = func(new JoyDatabaseOptions());
            if(connectionString == null) {
                throw new ArgumentNullException("Connection string is null!");
            }

            //init db

            return services;
        }
    }

    public record ConnectionString(string connectionString);

    public class JoyDatabaseOptions {
        private string _connectionString;

        private JoyDatabaseOptions(string connectionString) { 
            _connectionString = connectionString;
        }

        public JoyDatabaseOptions() { }

        public ConnectionString ConnectionString(string connectionString) {
            return new ConnectionString(connectionString);
        }

    }
}
