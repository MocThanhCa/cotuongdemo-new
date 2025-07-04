using cotuongdemotest1.Models.DTOs;
using cotuongdemotest1.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cotuongdemotest1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)//đây là 1 constructor, khi hàm này được gọi thì phần bên trong sẽ được chạy ngay lập tức
        {
            _gameService = gameService;
        }

        //[HttpGet("new")]
        //public IActionResult CreateNewGame()
        //{
        //    var gameId = _gameService.CreateNewGame();
        //    return Ok(new { GameId = gameId });
        //}
        [HttpPost("new")]
        public IActionResult CreateNewGame([FromBody] JoinGameRequest request)
        {
            var (gameId, userId) = _gameService.CreateNewGame(request.UserName);
            return CreatedAtAction(nameof(GetGameState), new { gameId }, new { GameId = gameId, UserId = userId });
        }



        //[HttpPost("joinGame")]
        //public IActionResult JoinGame(Guid gameId, Guid userId, string userName)
        //{
        //    var success = _gameService.JoinGame(gameId, userId, userName);
        //    if (!success)
        //    {
        //        return BadRequest("Không thể tham gia game.");
        //    }
        //    return Ok("Tham gia game thành công.");
        //}
        [HttpPost("joinGame")]
        public IActionResult JoinGame([FromBody] JoinGameRequest request)
        {
            var success = _gameService.JoinGame(request.GameId, request.UserName);
            if (!success)
            {
                return BadRequest("Không thể tham gia game.");
            }
            return Ok("Tham gia game thành công.");
        }



        [HttpGet("{gameId}/state")]
        public IActionResult GetGameState(Guid gameId)
        {
            var state = _gameService.GetGameState(gameId);
            if (state == null)
                return NotFound();
            return Ok(state);
        }
        [HttpPatch("{gameId}/move")]
        public IActionResult MakeMove(Guid gameId, [FromBody] MoveRequest moveRequest)
        {
            try
            {
                if (moveRequest == null)
                    return BadRequest("Request body is missing or invalid");

                if (!_gameService.MakeMove(gameId, moveRequest))
                    return BadRequest("Invalid move");

                return Ok(_gameService.GetGameState(gameId));
            }
            catch (Exception)
            {
                return BadRequest("Invalid request format");
            }
        }
        [HttpDelete("leaveGame/{gameId}/{userId}")]
        public IActionResult LeaveGame(Guid gameId, Guid userId)
        {
            var success = _gameService.LeaveGame(gameId, userId);
            if (!success)
                return BadRequest("Không thể rời game.");

            // Dọn dẹp các phòng trống
            _gameService.CleanupEmptyGames();

            return Ok("Rời game thành công.");
        }

    }
}
