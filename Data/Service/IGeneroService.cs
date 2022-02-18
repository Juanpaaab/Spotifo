using Spotifo.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spotifo.Data.Service
{
    public interface IGeneroService
    {
        Task<IEnumerable<Genero>> GetGeneros();
    }
}