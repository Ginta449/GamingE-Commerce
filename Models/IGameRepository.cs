using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingE_Commerce.Models
{
    public interface IGameRepository
    {
        IEnumerable<Game> AllGames { get; }
        Game GetGameById(int Id);
    }
}