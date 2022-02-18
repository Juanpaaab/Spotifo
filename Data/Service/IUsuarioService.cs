using Spotifo.Data.Model;
using System.Threading.Tasks;
using Spotifo.Areas.Identity.Data;
using System.Collections.Generic;

namespace Spotifo.Data.Service
{
    public interface IUsuarioService
    {
        Task<bool> UsuarioInsert(Usuario usuario);
        Task<bool> UsuarioUpdate(Usuario usuario, string _id);
        Task<Usuario> UsuarioDetails(string _email);
        Task<IEnumerable<Usuario>> GetUsuarios();
        Task<Usuario> UsuarioDetailsUnico(string _id);
        Task<bool> EliminarUsuario(string _id);
        Task<IEnumerable<Usuario>> BusquedaNacionalidad(Usuario usuario);
        
    }
}