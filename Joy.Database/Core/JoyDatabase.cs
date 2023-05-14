using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joy.Database.Core {
    public class JoyDatabase {
        public JoySchema[] Schemas { get; set; }
        public string ConnectionString { get; set; }

        public JoyDatabase(string connectionString) {
            ConnectionString = connectionString;
        }
    }
}
