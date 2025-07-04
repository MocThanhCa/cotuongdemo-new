using cotuongdemotest1.Data;
using cotuongdemotest1.Models.DTOs;
using cotuongdemotest1.Models.Entities;
using cotuongdemotest1.Services.Interfaces;
using System.Text.Json;

namespace cotuongdemotest1.Services
{
    public class GameService : IGameService
    {
        private readonly IMoveValidator _moveValidator;
        //private readonly Dictionary<Guid, GameState> _games;//hàm Dictionary dùng để ánh xạ key-value 
        private readonly ApplicationDbContext _context;

        public GameService(IMoveValidator moveValidator, ApplicationDbContext context)
        {
            _moveValidator = moveValidator;
            //_games = new Dictionary<Guid, GameState>();
            _context = context;
        }

        //public Guid CreateNewGame()
        //{
        //    var gameId = Guid.NewGuid();
        //    //_games[gameId] = new GameState
        //    //{
        //    //    board = new Board(),
        //    //    IsRedTurn = true
        //    //};
        //    var newGame = new GameDb
        //    {
        //        GameId = gameId,
        //        CreatedAt = DateTime.UtcNow,
        //        GameStatus = 0,//dang o trang thai waiting 
        //        Board = JsonSerializer.Serialize(new Board()),//chuyen ban co sang dang chuoi json
        //        IsRedTurn = true,
        //        MoveHistory = "[]",//mang json ban dau rong
        //        GameWinner = ""
        //    };
        //    _context.Games.Add(newGame);
        //    _context.SaveChanges();
        //    return gameId;
        //}
        public (Guid gameId, Guid userId) CreateNewGame(string userName)
        {
            var gameId = Guid.NewGuid();
            var userId = Guid.NewGuid(); // Tự sinh UserId cho người mở bàn

            // Tạo game mới
            var newGame = new GameDb
            {
                GameId = gameId,
                CreatedAt = DateTime.UtcNow,
                GameStatus = 0, // waiting
                IsRedTurn = true,
                MoveHistory = "[]",  // Lịch sử di chuyển rỗng
                Board = JsonSerializer.Serialize(new Board()),
                GameWinner = null
            };

            _context.Games.Add(newGame);

            // Tạo luôn UserGame với người mở bàn
            var newUserGame = new UserGameDb
            {
                GameId = gameId,
                UserId1 = userId, // Gán UserId vừa tạo
                NameUser1 = userName,
                UserId2 = null,
                NameUser2 = null
            };

            _context.UserGames.Add(newUserGame);
            _context.SaveChanges();

            return (gameId, userId); // Trả về cả GameId & UserId
        }



        public GameState GetGameState(Guid gameId)
        {
            //return _games.GetValueOrDefault(gameId);
            var game = _context.Games.FirstOrDefault(x => x.GameId == gameId);
            if (game == null) return null;
            return new GameState
            {
                board = JsonSerializer.Deserialize<Board>(game.Board),
                IsRedTurn = game.IsRedTurn
            };
        }
        //public bool JoinGame(Guid gameId, Guid userId, string userName)
        //{
        //    var game = _context.Games.FirstOrDefault(g => g.GameId == gameId);
        //    if (game == null) return false; // Phòng không tồn tại

        //    var userGame = _context.UserGames.FirstOrDefault(ug => ug.GameId == gameId);

        //    if (userGame == null)
        //    {
        //        // Nếu phòng chưa có ai, tạo mới với user đầu tiên
        //        userGame = new UserGameDb
        //        {
        //            GameId = gameId,
        //            UserId1 = userId,
        //            NameUser1 = userName,
        //            UserId2 = null,
        //            NameUser2 = null
        //        };
        //        _context.UserGames.Add(userGame);
        //    }
        //    else if (userGame.UserId2 == null)
        //    {
        //        // Nếu phòng đã có 1 người, thêm user thứ 2
        //        userGame.UserId2 = userId;
        //        userGame.NameUser2 = userName;

        //        // Cập nhật trạng thái phòng thành "ready"
        //        game.GameStatus = 1;
        //    }
        //    else
        //    {
        //        return false; // Phòng đã đầy
        //    }

        //    _context.SaveChanges();
        //    return true;
        //}
        public bool JoinGame(Guid gameId, string userName)
        {
            var game = _context.Games.FirstOrDefault(g => g.GameId == gameId);
            if (game == null) return false;

            var userGame = _context.UserGames.FirstOrDefault(ug => ug.GameId == gameId);

            if (userGame == null)
            {
                // Nếu phòng chưa có ai, tạo mới với userName là người chơi đầu tiên
                userGame = new UserGameDb
                {
                    GameId = gameId,
                    UserId1 = Guid.NewGuid(), // Tự động tạo userId
                    NameUser1 = userName,
                    UserId2 = null,
                    NameUser2 = null
                };
                _context.UserGames.Add(userGame);
            }
            else if (userGame.UserId2 == null)
            {
                // Nếu phòng đã có 1 người, thêm người chơi thứ 2
                userGame.UserId2 = Guid.NewGuid(); // Tự động tạo userId
                userGame.NameUser2 = userName;

                // Cập nhật trạng thái game thành "ready"
                game.GameStatus = 1;
            }
            else
            {
                // Nếu phòng đã đủ 2 người, từ chối tham gia
                return false;
            }

            _context.SaveChanges();
            return true;
        }

        public bool MakeMove(Guid gameId, MoveRequest request)
        {
            var game = _context.Games.FirstOrDefault(g => g.GameId == gameId);
            if (game == null) return false;

            var gameState = new GameState
            {
                board = JsonSerializer.Deserialize<Board>(game.Board),
                IsRedTurn = game.IsRedTurn
            };

            var piece = gameState.board.GetCell(request.fromX, request.fromY);
            if (piece == null || piece.PieceType == ChessPieceType.None || piece.IsRed != gameState.IsRedTurn)
                return false;

            if (!_moveValidator.ValidateMove(
                piece.PieceType, piece.IsRed,
                request.fromX, request.fromY, request.toX, request.toY,
                gameState.board))
                return false;

            // Cập nhật trạng thái bàn cờ và lượt chơi
            gameState.board.MovePiece(request.fromX, request.fromY, request.toX, request.toY);
            gameState.IsRedTurn = !gameState.IsRedTurn;

            // Lưu vào database
            game.Board = JsonSerializer.Serialize(gameState.board);
            game.IsRedTurn = gameState.IsRedTurn;

            // Cập nhật lịch sử di chuyển
            var moveHistory = JsonSerializer.Deserialize<List<MoveRequest>>(game.MoveHistory);
            moveHistory.Add(request);
            game.MoveHistory = JsonSerializer.Serialize(moveHistory);

            _context.SaveChanges();
            return true;
        }

        public bool IsValidMove(Guid gameId, MoveRequest request)
        {
            var game = _context.Games.FirstOrDefault(g => g.GameId == gameId);
            if (game == null) return false;

            var gameState = new GameState
            {
                board = JsonSerializer.Deserialize<Board>(game.Board),
                IsRedTurn = game.IsRedTurn
            };

            var piece = gameState.board.GetCell(request.fromX, request.fromY);
            if (piece == null || piece.PieceType == ChessPieceType.None || piece.IsRed != gameState.IsRedTurn)
                return false;

            return _moveValidator.ValidateMove(
                piece.PieceType, piece.IsRed,
                request.fromX, request.fromY, request.toX, request.toY,
                gameState.board);
        }
        public bool LeaveGame(Guid gameId, Guid userId)
        {
            var userGame = _context.UserGames.FirstOrDefault(ug => ug.GameId == gameId);
            if (userGame == null) return false;

            bool isUpdated = false;

            // Nếu userId1 rời đi
            if (userGame.UserId1 == userId && userGame.UserId1 != null)
            {
                userGame.UserId1 = null;
                userGame.NameUser1 = null;
                isUpdated = true;
            }
            // Nếu userId2 rời đi
            else if (userGame.UserId2 == userId && userGame.UserId2 != null)
            {
                userGame.UserId2 = null;
                userGame.NameUser2 = null;
                isUpdated = true;
            }

            if (!isUpdated) return false;

            _context.SaveChanges();
            return true;
        }

        public void CleanupEmptyGames()
        {
            var emptyGames = _context.UserGames
                .Where(g => g.UserId1 == null && g.UserId2 == null) 
                .ToList();
            foreach (var game in emptyGames)
            {
                var gameEntity = _context.Games.FirstOrDefault(g => g.GameId == game.GameId);
                if(gameEntity != null)
                {
                    _context.Games.Remove(gameEntity);
                }
                _context.UserGames.Remove(game);
            }
            _context.SaveChanges();
        }
    }
}
