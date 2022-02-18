using Spotifo.Data.Model;
using System.Threading.Tasks;
using Spotifo.Areas.Identity.Data;
using System.Collections.Generic;

namespace Spotifo.Data.Service
{
    public interface ICancionService
    {
        Task<bool> CancionInsert(Cancion cancion, Album album, Usuario usuario, Genero genero);
        Task<bool> CancionUpdate2(Cancion cancion, Album album, Usuario usuario, int id);
        Task<IEnumerable<Cancion>> CancionList(int SaltarFC);
        Task<IEnumerable<Cancion>> CancionListPura();
        Task<Cancion> CancionDetails(int id);
        Task<IEnumerable<Cancion>> CancionListPersonal();
        Task<bool> GustadoCancion(Cancion cancion, int _id, int contMegusta);
        Task<bool> VistasCancion(Cancion cancion, int _id, int contVistas);
        Task<bool> EliminarCancion(int _id);
        Task<IEnumerable<Cancion>> BusquedaGeneroC(Cancion cancion);
        Task<IEnumerable<Cancion>> GenerarLAleatoria(int genero, int Formulario);
        Task<IEnumerable<Cancion>> GenerarLista(Cancion cancion);
        Task<IEnumerable<Cancion>> TraerCancionesUsuario(Usuario usuario);
        Task<IEnumerable<Cancion>> CancionesFamosas();
        Task<IEnumerable<Cancion>> CancionesGustados();
    }
}