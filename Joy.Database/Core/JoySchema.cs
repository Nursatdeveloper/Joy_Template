using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joy.Database.Core {
    public class JoySchema {
        public string SchemaName { get; set; }
        public JoyTable[] Tables { get; set; }
    }
}
