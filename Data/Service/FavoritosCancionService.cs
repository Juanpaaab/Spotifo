using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Spotifo.Data.Model;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Spotifo.Data.Service
{
    public class FavoritosCancionService : IFavoritosCancionService
    {
        //Connecction Sql Server
        private readonly SqlConnectionConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FavoritosCancionService(SqlConnectionConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<bool> CancionGustado(FavoritosCancion FavoritosCancion, int _id)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();
                var EmailUsuario = _httpContextAccessor.HttpContext.User.Identity.Name;

                parameters.Add("Cancion_ID", FavoritosCancion.Cancion_ID, DbType.Int64);
                parameters.Add("User_ID", FavoritosCancion.User_ID, DbType.String);

                const string query = @"INSERT INTO FavoritosCancion
                    (Cancion_ID, 
                    User_ID) 
                    VALUES (@_Id, 
                    (SELECT Id FROM AspNetUsers WHERE Email = @EmailUsuario))";
                
                await conn.ExecuteAsync(query, new
                {
                    _Id = _id,
                    EmailUsuario
                }, commandType: CommandType.Text);
            }
            return true;
        }

        public async Task<FavoritosCancion> GetFA(int _id){
            var EmailUsuario = _httpContextAccessor.HttpContext.User.Identity.Name;
            var conn = new SqlConnection(_configuration.Value);
            const string query = @"SELECT *
            FROM FavoritosCancion WHERE Cancion_ID = @_Id 
            AND User_ID = (SELECT Id FROM AspNetUsers WHERE Email = @EmailUsuario)";
            return await conn.QueryFirstOrDefaultAsync<FavoritosCancion>(query.ToString(), new { _Id = _id, EmailUsuario });
        }


    }
}