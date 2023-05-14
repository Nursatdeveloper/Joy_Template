using Joy.Database.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joy.Database.Core {
    public class JoyTable {
        public FieldBase[] Fields { get; set; }
        public PrimaryKey PrimaryKey { get; set; }

    }

    public record PrimaryKey(FieldBase primaryKeyField);
}
