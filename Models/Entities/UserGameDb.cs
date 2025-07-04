using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cotuongdemotest1.Models.Entities
{
    public class UserGameDb
    {
        [Key]
        public Guid GameId { get; set; }
        [ForeignKey("GameId")]
        public GameDb? Game { get; set; }
        public Guid? UserId1 { get; set; }
        public Guid? UserId2 { get; set; }
        public string? NameUser1 { get; set; }
        public string? NameUser2 { get; set; }
    }
}
