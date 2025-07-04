namespace cotuongdemotest1.Models.DTOs
{
    public class MoveRequest
    {
        public int fromX { get; set; }//DTOs là nơi truyền dữ liệu giữa các tầng
        public int fromY { get; set; }
        public int toX { get; set; }
        public int toY { get; set; }
    }
}
