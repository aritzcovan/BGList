using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MyBGList.Models;
using MyBGList.DTO;
using System.Linq.Dynamic.Core;
using Microsoft.Data.SqlClient;

namespace MyBGList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BoardGamesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<BoardGamesController> _logger;

        public BoardGamesController(
            ApplicationDbContext context,
            ILogger<BoardGamesController> logger)
        {
            this.context = context;
            this._logger = logger;
        }

        [HttpGet(Name = "GetBoardGames")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public async Task<RestDTO<BoardGame[]>> Get(
            int pageIndex = 0,
            int pageSize = 10,
            string? sortColumn = "Name",
            string? sortOrder = "ASC",
            string? filterQuery = null)
        {
            var query = context.BoardGames.AsQueryable();
            if (!string.IsNullOrEmpty(filterQuery))
                query = query.Where(b => b.Name.Contains(filterQuery));
            var recordCount = await query.CountAsync();

            query = query
                    .OrderBy($"{sortColumn} {sortOrder}")
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize);

            return new RestDTO<BoardGame[]>()
            {
                Data = await query.ToArrayAsync(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "BoardGames",
                            new { pageIndex, pageSize},
                            Request.Scheme)!,
                    "self",
                    "Get"),
                }
            };
        }

        [HttpPost(Name = "UpdateBoardGame")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<BoardGame?>> Post(BoardGameDTO model)
        {
            var boardgame = await context.BoardGames
                                .Where(b => b.Id == model.Id)
                                .FirstOrDefaultAsync();

            if (boardgame != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    boardgame.Name = model.Name;
                if (model.Year.HasValue && model.Year.Value > 0)
                    boardgame.Year = model.Year.Value;
                boardgame.LastModifiedDate = DateTime.Now;
                context.BoardGames.Update(boardgame);
                await context.SaveChangesAsync();
            };
            return new RestDTO<BoardGame?>()
            {
                Data = boardgame,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(Url.Action(null, "BoardGames", model, Request.Scheme)!, "self", "POST"),
                }
            };
        }

        [HttpDelete(Name = "DeleteBoardGame")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<BoardGame?>> Delete(int id)
        {
            var boardgame = await context.BoardGames
                                .Where (b => b.Id == id)
                                .FirstOrDefaultAsync();
            if(boardgame != null)
            {
                context.BoardGames.Remove(boardgame);
                await context.SaveChangesAsync();
            }

            return new RestDTO<BoardGame?>()
            {
                Data = boardgame,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                            null,
                            "BoardGames",
                            id,
                            Request.Scheme)!,
                    "self", "DELETE"),
                }
            };
        }
    }
}