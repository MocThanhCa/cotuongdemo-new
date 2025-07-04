using cotuongdemotest1.Models.DTOs;
using cotuongdemotest1.Models.Entities;

namespace cotuongdemotest1.Services.Interfaces
{
    public interface IGameService
    {
        (Guid gameId, Guid userId) CreateNewGame(string userName);//ban chat la string
        GameState GetGameState(Guid gameId);
        bool MakeMove(Guid gameId, MoveRequest request);
        bool IsValidMove(Guid gameId, MoveRequest request);
        
        //bool JoinGame(Guid gameId,Guid userId, string userName);// them cai nay de quan ly viec join game
        bool JoinGame(Guid gameId, string userName);
        bool LeaveGame(Guid gameId, Guid userId);
        void CleanupEmptyGames();
        //bool UndoMove(Guid gameId);

    }
}
