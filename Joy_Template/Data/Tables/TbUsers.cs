using MVCTemplate.Sources.Repository;
using System.ComponentModel.DataAnnotations;

namespace Joy_Template.Data.Tables {
    public class TbUser : TbBase {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Fathername { get; set; }
        public DateTime BirthDate { get; set; }
        [StringLength(12, MinimumLength = 12)]
        public string Iin { get; set; }
        public string Roles { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class TbRole : TbBase {
        public string Role { get; set; }
    }
}
