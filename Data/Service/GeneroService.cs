using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Spotifo.Data.Model;
using Microsoft.AspNetCore.Http;

namespace Spotifo.Data.Service
{
    public class GeneroService : IGeneroService
    {
        private readonly SqlConnectionConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GeneroService(SqlConnectionConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Genero>> GetGeneros()
        {
            IEnumerable<Genero> generos;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"SELECT * FROM Genero";
                generos = await conn.QueryAsync<Genero>(query.ToString(), new { });
            }
            return generos;
        }
    }
}
