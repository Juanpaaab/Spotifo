using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Spotifo.Data.Model;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Spotifo.Data.Service
{
    public class FavoritosAlbumService : IFavoritosAlbumService
    {
        //Connecction Sql Server
        private readonly SqlConnectionConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FavoritosAlbumService(SqlConnectionConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<bool> AlbumGustado(FavoritosAlbum FavoritosAlbum, int _id)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();
                var EmailUsuario = _httpContextAccessor.HttpContext.User.Identity.Name;

                parameters.Add("Album_ID", FavoritosAlbum.Album_ID, DbType.Int64);
                parameters.Add("User_ID", FavoritosAlbum.User_ID, DbType.String);

                const string query = @"INSERT INTO FavoritosAlbum
                    (Album_ID, 
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

        public async Task<FavoritosAlbum> GetFA(int _id){
            var EmailUsuario = _httpContextAccessor.HttpContext.User.Identity.Name;
            var conn = new SqlConnection(_configuration.Value);
            const string query = @"SELECT *
            FROM FavoritosAlbum WHERE Album_ID = @_Id 
            AND User_ID = (SELECT Id FROM AspNetUsers WHERE Email = @EmailUsuario)";
            return await conn.QueryFirstOrDefaultAsync<FavoritosAlbum>(query.ToString(), new { _Id = _id, EmailUsuario });
        }

        
    }
}