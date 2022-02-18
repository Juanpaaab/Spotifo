using Spotifo.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spotifo.Data.Service
{
    public interface IFavoritosAlbumService
    {
        Task<bool> AlbumGustado(FavoritosAlbum FavoritosAlbum, int _id);
        Task<FavoritosAlbum> GetFA(int _id);
    }
}