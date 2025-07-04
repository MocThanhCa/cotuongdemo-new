using cotuongdemotest1.Models.Entities;
using cotuongdemotest1.Services.Interfaces;

namespace cotuongdemotest1.Services
{
    public class MoveValidator : IMoveValidator
    {
        public bool ValidateMove(ChessPieceType pieceType, bool isRed, int fromX, int fromY, int toX, int toY, Board board)
        {
            if (ValidateKingFaceToFace(fromX, fromY, toX, toY, board)) return false;
            
            switch (pieceType)
            {
                case ChessPieceType.Xe:
                    return ValidateXeMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Ma:
                    return ValidateMaMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Tinh:
                    return ValidateTinhMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Si:
                    return ValidateSiMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Tuong:
                    return ValidateTuongMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Phao:
                    return ValidatePhaoMove(fromX, fromY, toX, toY, board);
                case ChessPieceType.Tot:
                    return ValidateTotMove(fromX, fromY, toX, toY, board);
                default:
                    return false;
            }
        }
        private bool ValidateKingFaceToFace(int fromX, int fromY, int toX, int toY, Board board)
        {
            Cell movedPiece = board.GetCell(fromX, fromY);
            if (movedPiece.PieceType != ChessPieceType.Tuong)
                return false;

            // Tìm vị trí Tướng đối phương
            int enemyKingX = -1, enemyKingY = -1;
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    var cell = board.GetCell(x, y);
                    if (cell.PieceType == ChessPieceType.Tuong && cell.IsRed != movedPiece.IsRed)
                    {
                        enemyKingX = x;
                        enemyKingY = y;
                        break;
                    }
                }
            }

            if (enemyKingX == -1) return false;

            // Kiểm tra nếu hai Tướng trên cùng một cột
            if (toY == enemyKingY)
            {
                for (int x = Math.Min(toX, enemyKingX) + 1; x < Math.Max(toX, enemyKingX); x++)
                {
                    if (board.GetCell(x, toY).PieceType != ChessPieceType.None)
                        return false;
                }
                return true;
            }

            return false;
        }
        //logic di chuyển của Xe
        private bool ValidateXeMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            if (targetCell.PieceType != ChessPieceType.None && targetCell.IsRed == sourceCell.IsRed)
                return false;
            // Xe chỉ đi thẳng theo hàng ngang hoặc dọc
            if (fromX != toX && fromY != toY) return false;

            if (fromY == toY)//di chuyen doc
            {
                int start = Math.Min(fromX, toX);
                int end = Math.Max(fromX, toX);
                for (int x = start + 1; x <= end; x++)//kiem tra xem co quan nao chan khong
                {
                    if (targetCell.IsRed != sourceCell.IsRed)
                        return true;
                }
            }
            else // Di chuyển ngang
            {
                int start = Math.Min(fromY, toY);
                int end = Math.Max(fromY, toY);
                for (int y = start + 1; y < end; y++)
                {
                    if (targetCell.IsRed != sourceCell.IsRed)
                        return true;
                }
            }
            return true;
        }
        //Logic di chuyển của pháo
        private bool ValidatePhaoMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            // Pháo đi thẳng như xe
            if (fromX != toX && fromY != toY) return false;

            int pieceCount = 0;
            if (fromY == toY) // Di chuyển dọc
            {
                int start = Math.Min(fromX, toX);
                int end = Math.Max(fromX, toX);
                for (int x = start + 1; x < end; x++)
                {
                    if (board.GetCell(x, fromY).PieceType != ChessPieceType.None) pieceCount++;
                }
            }
            else // Di chuyển ngang
            {
                int start = Math.Min(fromY, toY);
                int end = Math.Max(fromY, toY);
                for (int y = start + 1; y < end; y++)
                {
                    if (board.GetCell(fromX, y).PieceType != ChessPieceType.None) pieceCount++;
                }
            }
            if (targetCell.IsRed != sourceCell.IsRed && pieceCount == 1 && targetCell.PieceType != ChessPieceType.None)
            {
                return true;
            }
            return board.GetCell(toX, toY).PieceType != ChessPieceType.None ? pieceCount == 1 : pieceCount == 0;
        }
        //Logic di chuyển của Mã
        private bool ValidateMaMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            int deltaX = Math.Abs(fromX - toX);
            int deltaY = Math.Abs(fromY - toY);
            //kiem tra dich den hop le theo phuong thuc di chuyen
            if (!((deltaX == 1 && deltaY == 2) || (deltaX == 2 && deltaY == 1)))
                return false;
            //kiem tra chan
            if (deltaX == 2)//theo chieu doc
            {
                if (fromX > toX)
                {
                    int blockX = fromX - 1;
                    if (board.GetCell(blockX, fromY).PieceType != ChessPieceType.None) return false;
                }
                else
                {
                    int blockX = fromX + 1;
                    if (board.GetCell(blockX, fromY).PieceType != ChessPieceType.None) return false;
                }
            }
            else//theo chieu ngang
            {
                if (fromY > toY)
                {
                    int blockY = fromY - 1;
                    if (board.GetCell(fromX, blockY).PieceType != ChessPieceType.None) return false;
                }
                else
                {
                    int blockY = fromY + 1;
                    if (board.GetCell(fromX, blockY).PieceType != ChessPieceType.None) return false;
                }
            }
            if (targetCell.PieceType != ChessPieceType.None)
            {
                if (targetCell.IsRed != sourceCell.IsRed) return true;
                else return false;
            }
            return true;
        }
        //Logic di chuyển cho tượng / tịnh 
        private bool ValidateTinhMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            int deltaX = Math.Abs(fromX - toX);
            int deltaY = Math.Abs(fromY - toY);
            //kiem tra dich den hop le theo phuong thuc di chuyen
            if (!(deltaX == 2 && deltaY == 2))
                return false;
            if (fromX == 4 && toX > 4) return false;//khong duoc sang song
            if (fromX == 5 && toX < 5) return false;

            //kiem tra chan
            if (toX > fromX)
            {
                if (toY > fromY)
                {
                    if (board.GetCell(fromX + 1, fromY + 1).PieceType != ChessPieceType.None) return false;
                }
                else
                {
                    if (board.GetCell(fromX + 1, fromY - 1).PieceType != ChessPieceType.None) return false;
                }
            }
            else
            {
                if (toY > fromY)
                {
                    if (board.GetCell(fromX - 1, fromY + 1).PieceType != ChessPieceType.None) return false;
                }
                else
                {
                    if (board.GetCell(fromX - 1, fromY - 1).PieceType != ChessPieceType.None) return false;
                }
            }
            //kiem tra dich
            if (targetCell.PieceType != ChessPieceType.None)
            {
                if (targetCell.IsRed != sourceCell.IsRed) return true;
                else return false;
            }
            return true;
        }
        //Logic di chuyển cho Sĩ
        private bool ValidateSiMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            int deltaX = Math.Abs(fromX - toX);
            int deltaY = Math.Abs(fromY - toY);
            if (!(toY <= 5 && toY >= 3 && deltaX == 1 && deltaY == 1) && (toX <= 2 || toX >= 7))
                return false;

            if (targetCell.PieceType != ChessPieceType.None)
            {
                if (targetCell.IsRed != sourceCell.IsRed) return true;
                else return false;
            }
            return true;
        }
        //Logic di chuyển của tướng
        //private bool ValidateTuongMove(int fromX, int fromY, int toX, int toY, Board board)
        //{
        //    var sourceCell = board.GetCell(fromX, fromY);
        //    var targetCell = board.GetCell(toX, toY);
        //    int deltaX = Math.Abs(fromX - toX);
        //    int deltaY = Math.Abs(fromY - toY);
        //    if (!(toY <= 5 && toY >= 3 && (toX <= 2 || toX >= 7) && ((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1))))
        //        return false;
        //    if (targetCell.PieceType != ChessPieceType.None)
        //    {
        //        if (targetCell.IsRed != sourceCell.IsRed) return true;
        //        else return false;
        //    }
        //    return true;
        //}
        private bool ValidateTuongMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            int deltaX = Math.Abs(fromX - toX);
            int deltaY = Math.Abs(fromY - toY);

            // Kiểm tra xem nước đi có hợp lệ không (Tướng chỉ đi trong cung)
            if (!(toY <= 5 && toY >= 3 && (toX <= 2 || toX >= 7) && ((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1))))
                return false;

            // Kiểm tra nếu di chuyển đến ô có quân đồng minh
            if (targetCell.PieceType != ChessPieceType.None && targetCell.IsRed == sourceCell.IsRed)
                return false;

            // Thử thực hiện nước đi tạm thời
            board.MovePiece(fromX, fromY,toX, toY); // Di chuyển Tướng đến vị trí mới
            //board.MovePiece(fromX, fromY, new Cell()); // Xóa vị trí cũ

            // Kiểm tra nếu Tướng bị chiếu
            bool isChecked = IsKingInCheck(toX, toY, sourceCell.IsRed, board);

            // Hoàn tác nước đi
            board.MovePiece(toX, toY, fromX, fromY);

            return !isChecked;
        }
        // Hàm kiểm tra nếu Tướng bị chiếu sau khi di chuyển
        private bool IsKingInCheck(int kingX, int kingY, bool isRed, Board board)
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    var cell = board.GetCell(x, y);
                    if (cell.PieceType != ChessPieceType.None && cell.IsRed != isRed)
                    {
                        if (ValidateMove(cell.PieceType, cell.IsRed, x, y, kingX, kingY, board))
                            return true;
                    }
                }
            }
            return false;
        }
        //Logic di chuyển của Tốt

        private bool ValidateTotMove(int fromX, int fromY, int toX, int toY, Board board)
        {
            var sourceCell = board.GetCell(fromX, fromY);
            var targetCell = board.GetCell(toX, toY);
            int deltaXDo = toX - fromX;
            int deltaXDen = fromX - toX;
            //int deltaX = Math.Abs(fromX - toX);//sai o day
            int deltaY = Math.Abs(fromY - toY);
            if (sourceCell.IsRed)
            {
                if (toX > 4)
                {
                    if (!(deltaXDo == 1 && deltaY == 0) || (deltaXDo == 0 && deltaY == 1)) return false;
                }
                else
                {
                    if (!(deltaXDo == 1 && deltaY == 0)) return false;
                }
            }
            else
            {
                if (toX < 5)//xet ban co huong con lai
                {
                    if (!((deltaXDen == 1 && deltaY == 0) || (deltaXDen == 0 && deltaY == 1))) return false;
                }
                else
                {
                    if (!(deltaXDen == 1 && deltaY == 0)) return false;
                }
            }
            if (targetCell.PieceType != ChessPieceType.None)
            {
                if (targetCell.IsRed != sourceCell.IsRed) return true;
                else return false;
            }
            return true;
        }
    }
}
