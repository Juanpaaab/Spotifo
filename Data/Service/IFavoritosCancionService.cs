using Spotifo.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spotifo.Data.Service
{
    public interface IFavoritosCancionService
    {
        Task<bool> CancionGustado(FavoritosCancion FavoritosCancion, int _id);
        Task<FavoritosCancion> GetFA(int _id);
    }
}