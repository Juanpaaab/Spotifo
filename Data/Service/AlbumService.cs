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
    public class AlbumService : IAlbumService
    {
        private readonly SqlConnectionConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AlbumService(SqlConnectionConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AlbumAdd2(Album album, Genero genero)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();
                var EmailUsuario = _httpContextAccessor.HttpContext.User.Identity.Name;

                parameters.Add("Titulo_album", album.Titulo_album, DbType.String);
                parameters.Add("Genero_ID", album.Genero_ID, DbType.Int64);
                parameters.Add("Cover_image", album.Cover_image, DbType.Byte);
                parameters.Add("Fecha_publicacion_album", album.Fecha_publicacion_album, DbType.DateTime);
                parameters.Add("Descripcion", album.Descripcion, DbType.String);
                parameters.Add("Email", EmailUsuario, DbType.String);
                const string query = @"INSERT INTO Album
                    (Titulo_album, 
                    Genero_ID,
                    Cover_image,
                    Fecha_publicacion_album,
                    Descripcion,
                    Usuario_ID) 
                    VALUES (@Titulo_album, 
                    @Genero_ID,
                    @Cover_image,
                    @Fecha_publicacion_album,
                    @Descripcion,
                    (SELECT Id FROM AspNetUsers WHERE Email = @EmailUsuario))";
                await conn.ExecuteAsync(query, new
                {
                    album.Titulo_album,
                    album.Genero_ID,
                    album.Cover_image,
                    album.Fecha_publicacion_album,
                    album.Descripcion,
                    EmailUsuario
                }, commandType: CommandType.Text);
            }
            return true;
        }
        public async Task<bool> AlbumUpdate2(Album album, int _id)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();
                parameters.Add("Titulo_album", album.Titulo_album, DbType.String);
                parameters.Add("Genero_ID", album.Genero_ID, DbType.Int64);
                parameters.Add("Cover_image", album.Cover_image, DbType.Byte);
                parameters.Add("Fecha_publicacion_album", album.Fecha_publicacion_album, DbType.DateTime);
                parameters.Add("Descripcion", album.Descripcion, DbType.String);
                const string query = @"UPDATE Album
                      SET Titulo_album = @Titulo_album,
                          Genero_ID = @Genero_ID,
                          Cover_image = @Cover_image,
                          Fecha_publicacion_album = @Fecha_publicacion_album,
                          Descripcion = @Descripcion
                          WHERE ID_album = @_id";
                await conn.ExecuteAsync(query, new
                {
                    album.Titulo_album,
                    album.Genero_ID,
                    album.Cover_image,
                    album.Fecha_publicacion_album,
                    album.Descripcion,
                    _ID = _id
                }, commandType: CommandType.Text);
            }
            return true;
        }

        public async Task<IEnumerable<Album>> AlbumList(int SaltarFA)
        {
            IEnumerable<Album> albumes;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"SELECT ID_Album, Titulo_album, 
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_Genero = Genero_ID), 
                Fecha_publicacion_album,
                Usuario_ID = (Select Nombre_user from AspNetUsers where Id = Usuario_ID),
                _EmailUser = (SELECT Email from AspNetUsers WHERE Id = Usuario_ID)
                FROM Album
                ORDER BY ID_Album
                OFFSET @SaltarFA ROWS FETCH NEXT 10 ROWS ONLY";

                albumes = await conn.QueryAsync<Album>(query.ToString(), new { SaltarFA = SaltarFA });
            }
            return albumes;
        }

        public async Task<IEnumerable<Album>> AlbumListPura()
        {
            IEnumerable<Album> albumes;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"SELECT ID_Album, Titulo_album, 
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_Genero = Genero_ID), 
                Fecha_publicacion_album,
                Usuario_ID = (Select Nombre_user from AspNetUsers where Id = Usuario_ID),
                _EmailUser = (SELECT Email from AspNetUsers WHERE Id = Usuario_ID)
                FROM Album";

                albumes = await conn.QueryAsync<Album>(query.ToString(), new {  });
            }
            return albumes;
        }

        public async Task<IEnumerable<Album>> AlbumListPersonal()
        {
            IEnumerable<Album> albumes;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var _email = _httpContextAccessor.HttpContext.User.Identity.Name;
                const string query = @"SELECT 
                ID_album,
                Titulo_album, 
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_Genero = Genero_ID), 
                Fecha_publicacion_album FROM Album WHERE Usuario_ID = (SELECT Id FROM AspNetUsers WHERE Email = @_email)";

                albumes = await conn.QueryAsync<Album>(query.ToString(), new { _email = _email });
            }
            return albumes;
        }

        public async Task<Album> AlbumDetails(int _id)
        {
            var conn = new SqlConnection(_configuration.Value);
            const string query = @"SELECT  
                Titulo_album,
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_Genero = Genero_ID),
                Cover_image,
                Fecha_publicacion_album,
                Descripcion,
                _EmailUser = (SELECT Email FROM AspNetUsers WHERE ID = Usuario_ID)
                FROM Album WHERE ID_album = @_ID";
            return await conn.QueryFirstOrDefaultAsync<Album>(query.ToString(), new { _Id = _id });
        }

        public async Task<bool> EliminarAlbum(int _id)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();

                const string query = @"DELETE FROM Cancion  WHERE Album_ID = @_id";
                await conn.ExecuteAsync(query, new
                {
                    _id = _id,
                }, commandType: CommandType.Text);

                const string query1 = @"DELETE FROM Album WHERE ID_album = @_id";
                await conn.ExecuteAsync(query1, new
                {
                    _id = _id,
                }, commandType: CommandType.Text);

            }
            return true;
        }

        public async Task<IEnumerable<Album>> BusquedaGeneroA(Album album)
        {
            IEnumerable<Album> albumnes;
            using (var conn = new SqlConnection(_configuration.Value))
            {
            const string query = @"SELECT
                Id_album,
                Titulo_album,
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_Genero = Genero_ID),
                Fecha_publicacion_album
                FROM Album WHERE Genero_ID = @Genero_ID";
            albumnes = await conn.QueryAsync<Album>(query.ToString(), new { Genero_ID = album.Genero_ID });
            }
            return albumnes;
        }

        public async Task<bool> GustadoAlbum(Album album, int _id, int contMegusta)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();

                const string query = @"UPDATE Album SET Favoritos = @contMegusta WHERE ID_Album = @_id";
                await conn.ExecuteAsync(query, new
                {
                    _id = _id,
                    contMegusta = contMegusta
                }, commandType: CommandType.Text);
            }
            return true;
        }

        public async Task<IEnumerable<Album>> TraerAlbumnesUsuario(Usuario usuario)
        {
            IEnumerable<Album> albumnes;
            using (var conn = new SqlConnection(_configuration.Value))
            {
            const string query = @"SELECT ID_album, Titulo_album, Favoritos, 
            Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_genero = Genero_ID),
            Fecha_publicacion_album FROM Album
            WHERE Usuario_ID = (SELECT Id FROM AspNetUsers WHERE Nombre_user = @Nombre_usuario AND Apellido_user = @Apellido_usuario)";
            albumnes = await conn.QueryAsync<Album>(query.ToString(), new { 
                    Nombre_usuario = usuario.Nombre_usuario,
                    Apellido_usuario = usuario.Apellido_usuario,
                });
            }
            return albumnes;
        }

        public async Task<IEnumerable<Album>> AlbumnesGustados(){
            var EmailUsuario = _httpContextAccessor.HttpContext.User.Identity.Name;
            IEnumerable<Album> albumnes;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"SELECT DISTINCT Id_album, Titulo_album, Nombre_Genero = 
                (SELECT Genero_Nombre FROM Genero WHERE Genero_ID = ID_genero), Fecha_publicacion_album,
                Favoritos
                FROM Album
                JOIN FavoritosAlbum 
                ON Album.ID_Album = FavoritosAlbum.Album_ID
                RIGHT JOIN AspNetUsers
                ON (SELECT Id FROM AspNetUsers where Email = @EmailUsuario) = FavoritosAlbum.User_ID";
                albumnes = await conn.QueryAsync<Album>(query.ToString(), new { EmailUsuario });
            }
            return albumnes;
        }
    }
}
