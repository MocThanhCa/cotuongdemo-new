namespace cotuongdemotest1.Models.Entities
{
    public class Board
    {
        public Cell[][] Cells { get; set; }

        public Board()
        {
            Cells = new Cell[10][];
            for (int i = 0; i < 10; i++)
            {
                Cells[i] = new Cell[9];
            }
            InitializeBoard();
        }

        public void InitializeBoard()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    Cells[x][y] = new Cell(); // Mặc định không có quân cờ
                }
            }

            // Khởi tạo các quân cờ cho bên đỏ (true)
            Cells[0][0] = new Cell(ChessPieceType.Xe, true);
            Cells[0][1] = new Cell(ChessPieceType.Ma, true);
            Cells[0][2] = new Cell(ChessPieceType.Tinh, true);
            Cells[0][3] = new Cell(ChessPieceType.Si, true);
            Cells[0][4] = new Cell(ChessPieceType.Tuong, true);
            Cells[0][5] = new Cell(ChessPieceType.Si, true);
            Cells[0][6] = new Cell(ChessPieceType.Tinh, true);
            Cells[0][7] = new Cell(ChessPieceType.Ma, true);
            Cells[0][8] = new Cell(ChessPieceType.Xe, true);

            Cells[2][1] = new Cell(ChessPieceType.Phao, true);
            Cells[2][7] = new Cell(ChessPieceType.Phao, true);

            Cells[3][0] = new Cell(ChessPieceType.Tot, true);
            Cells[3][2] = new Cell(ChessPieceType.Tot, true);
            Cells[3][4] = new Cell(ChessPieceType.Tot, true);
            Cells[3][6] = new Cell(ChessPieceType.Tot, true);
            Cells[3][8] = new Cell(ChessPieceType.Tot, true);

            // Khởi tạo quân cờ cho bên đen (false)
            Cells[9][0] = new Cell(ChessPieceType.Xe, false);
            Cells[9][1] = new Cell(ChessPieceType.Ma, false);
            Cells[9][2] = new Cell(ChessPieceType.Tinh, false);
            Cells[9][3] = new Cell(ChessPieceType.Si, false);
            Cells[9][4] = new Cell(ChessPieceType.Tuong, false);
            Cells[9][5] = new Cell(ChessPieceType.Si, false);
            Cells[9][6] = new Cell(ChessPieceType.Tinh, false);
            Cells[9][7] = new Cell(ChessPieceType.Ma, false);
            Cells[9][8] = new Cell(ChessPieceType.Xe, false);

            Cells[7][1] = new Cell(ChessPieceType.Phao, false);
            Cells[7][7] = new Cell(ChessPieceType.Phao, false);

            Cells[6][0] = new Cell(ChessPieceType.Tot, false);
            Cells[6][2] = new Cell(ChessPieceType.Tot, false);
            Cells[6][4] = new Cell(ChessPieceType.Tot, false);
            Cells[6][6] = new Cell(ChessPieceType.Tot, false);
            Cells[6][8] = new Cell(ChessPieceType.Tot, false);
        }

        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < 10 && y < 9;
        }

        public Cell GetCell(int x, int y)
        {
            return IsValidPosition(x, y) ? Cells[x][y] : null;
        }

        public ChessPieceType RemovePiece(int x, int y)
        {
            if (IsValidPosition(x, y) && Cells[x][y].PieceType != ChessPieceType.None)
            {
                ChessPieceType capturedPiece = Cells[x][y].PieceType;
                Cells[x][y] = new Cell();
                return capturedPiece;
            }
            return ChessPieceType.None;
        }

        public void MovePiece(int fromX, int fromY, int toX, int toY)
        {
            if (IsValidPosition(fromX, fromY) && IsValidPosition(toX, toY))
            {
                if (Cells[toX][toY].PieceType != ChessPieceType.None)
                {
                    RemovePiece(toX, toY);
                }

                Cells[toX][toY] = Cells[fromX][fromY];
                Cells[fromX][fromY] = new Cell();
            }
        }
    }
}
