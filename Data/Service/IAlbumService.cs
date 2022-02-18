using Spotifo.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spotifo.Data.Service
{
    public interface IAlbumService
    {
        Task<bool> AlbumAdd2(Album album, Genero genero);
        Task<bool> AlbumUpdate2(Album album, int _id);
        Task<IEnumerable<Album>> AlbumList(int SaltarFA);
        Task<IEnumerable<Album>> AlbumListPura();
        Task<IEnumerable<Album>> AlbumListPersonal();
        Task<Album> AlbumDetails(int _id);
        Task<bool> EliminarAlbum(int id);
        Task<IEnumerable<Album>> BusquedaGeneroA(Album album);
        Task<IEnumerable<Album>> TraerAlbumnesUsuario(Usuario usuario);
        Task<bool> GustadoAlbum(Album album, int _id, int contMegusta);
        Task<IEnumerable<Album>> AlbumnesGustados();
    }
}