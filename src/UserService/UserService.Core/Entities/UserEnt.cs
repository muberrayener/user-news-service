using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.UserService.Core.Entities
{
    [Table("users")]

    public class UserEnt:BaseEnt
    {
        public required string name { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
        public string role { get; set; }
    }
}
