using System.ComponentModel.DataAnnotations;

namespace cotuongdemotest1.Models.Entities
{
    public class GameDb
    {
        [Key]
        public Guid GameId { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int GameStatus { get; set; }//0: waiting, 1: playing, 2: end
        public string? Board {  get; set; }
        public bool IsRedTurn { get; set; }
        public string? MoveHistory { get; set; }
        public string? GameWinner { get; set; }//hien thi xem van do ai thang

    }
}
