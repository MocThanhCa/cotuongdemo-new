namespace cotuongdemotest1.Models.Entities
{
    public class Cell
    {
        public ChessPieceType PieceType { get; set; }
        public bool IsRed { get; private set; } //true - do, false - den
        public Cell(ChessPieceType pieceType = ChessPieceType.None, bool isRed = false)
        {
            PieceType = pieceType;
            IsRed = isRed;
        }
    }
}
