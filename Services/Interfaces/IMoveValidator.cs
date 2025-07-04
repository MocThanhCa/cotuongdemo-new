using cotuongdemotest1.Models.Entities;

namespace cotuongdemotest1.Services.Interfaces
{
    public interface IMoveValidator
    {
        bool ValidateMove(ChessPieceType pieceType, bool isRed, int fromX, int fromY, int toX, int toY, Board board);
    }
}
