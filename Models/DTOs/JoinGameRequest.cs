namespace cotuongdemotest1.Models.DTOs
{
    public class JoinGameRequest
    {
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }

}
