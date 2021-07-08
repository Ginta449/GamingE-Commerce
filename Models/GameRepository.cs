using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GamingE_Commerce.Data;

namespace GamingE_Commerce.Models
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public GameRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Game> AllGames
        {
            get
            {
                return _appDbContext.Game.Include(g => g.Platform);
            }
        }

        //public IEnumerable<Game> AllGames => throw new NotImplementedException();

        public Game GetGameById(int Id)
        {
            return _appDbContext.Game.Include(g => g.Platform).FirstOrDefault(g => g.Id == Id);

            //_appDbContext.Games.FirstOrDefault(g => g.Id == Id);
        }
    }
}
