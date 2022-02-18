using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Spotifo.Data.Model;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Spotifo.Data.Service
{
    public class CancionService : ICancionService
    {
        //Connecction Sql Server
        private readonly SqlConnectionConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CancionService(SqlConnectionConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<bool> CancionInsert(Cancion cancion, Album album, Usuario usuario, Genero genero)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();
                var EmailUsuario = _httpContextAccessor.HttpContext.User.Identity.Name;

                parameters.Add("Nombre_cancion", cancion.Nombre_cancion, DbType.String);
                parameters.Add("Genero_ID", cancion.Genero_ID, DbType.Int64);
                parameters.Add("Fecha_publicacion_cancion", cancion.Fecha_publicacion_cancion, DbType.Date);
                parameters.Add("URL", cancion.URL, DbType.String);
                parameters.Add("Extension", cancion.Extension, DbType.String);
                parameters.Add("Titulo_album", album.Titulo_album, DbType.String);
                parameters.Add("Email", EmailUsuario, DbType.String);

                const string query = @"INSERT INTO Cancion
                    (Nombre_cancion, 
                    Genero_ID,
                    Fecha_publicacion_cancion,
                    URL,
                    Extension,
                    Album_ID,
                    Usuario_ID) 
                    VALUES (@Nombre_cancion, 
                    @Genero_ID,
                    @Fecha_publicacion_cancion,
                    @URL, 
                    @Extension,
                    (SELECT ID_album FROM Album WHERE Titulo_Album = @Titulo_Album),
                    (SELECT Id FROM AspNetUsers WHERE Email = @EmailUsuario) )";
                
                await conn.ExecuteAsync(query, new
                {
                    cancion.Nombre_cancion,
                    cancion.Genero_ID,
                    cancion.Fecha_publicacion_cancion,
                    cancion.URL,
                    cancion.Extension,
                    album.Titulo_album,
                    EmailUsuario
                }, commandType: CommandType.Text);
            }
            return true;
        }
        public async Task<IEnumerable<Cancion>> CancionList(int SaltarFC)
        {
            IEnumerable<Cancion> canciones;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"SELECT ID_cancion, Nombre_cancion, Vistas,
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_Genero = Genero_ID),
                URL, 
                Usuario_ID = (Select Nombre_user from AspNetUsers where Id = Usuario_ID ),
                User_Titulo_Album = (SELECT Titulo_Album FROM Album WHERE ID_Album = Album_ID),
                _EmailUser = (SELECT Email FROM AspNetUsers WHERE Id = Usuario_ID),
                Fecha_publicacion_cancion, Album_ID FROM Cancion
                ORDER BY ID_cancion
                OFFSET @SaltarFC ROWS FETCH NEXT 10 ROWS ONLY";

                canciones = await conn.QueryAsync<Cancion>(query.ToString(), new { SaltarFC = SaltarFC });
            }
            return canciones;
        }
        public async Task<IEnumerable<Cancion>> CancionListPura()
        {
            IEnumerable<Cancion> canciones;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"SELECT ID_cancion, Nombre_cancion, Vistas,
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_Genero = Genero_ID),
                URL, 
                Usuario_ID = (Select Nombre_user from AspNetUsers where Id = Usuario_ID ),
                User_Titulo_Album = (SELECT Titulo_Album FROM Album WHERE ID_Album = Album_ID),
                _EmailUser = (SELECT Email FROM AspNetUsers WHERE Id = Usuario_ID),
                Fecha_publicacion_cancion, Album_ID FROM Cancion";

                canciones = await conn.QueryAsync<Cancion>(query.ToString(), new { });
            }
            return canciones;
        }


        public async Task<bool> CancionUpdate2(Cancion cancion, Album album, Usuario usuario, int _id)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();
                parameters.Add("Nombre_cancion", cancion.Nombre_cancion, DbType.String);
                parameters.Add("Genero_ID", cancion.Genero_ID, DbType.Int64);
                parameters.Add("Fecha_publicacion_cancion", cancion.Fecha_publicacion_cancion, DbType.Date);
                parameters.Add("URL", cancion.URL, DbType.String);
                parameters.Add("Titulo_album", album.Titulo_album, DbType.String);
                parameters.Add("Extension", cancion.Extension, DbType.String);

                const string query = @"UPDATE Cancion 
                          SET Nombre_cancion = @Nombre_cancion,
                          Genero_ID = @Genero_ID,
                          Fecha_publicacion_cancion = @Fecha_publicacion_cancion,
                          URL = @URL,
                          Extension = @Extension,
                          Album_ID = (SELECT ID_album FROM Album WHERE Titulo_Album = @Titulo_Album)
                          WHERE ID_cancion = @_Id";
                await conn.ExecuteAsync(query, new
                {
                    cancion.Nombre_cancion,
                    cancion.Genero_ID,
                    cancion.Fecha_publicacion_cancion,
                    cancion.URL,
                    album.Titulo_album,
                    usuario.Nombre_usuario,
                    cancion.Extension,
                    _Id = _id
                }, commandType: CommandType.Text);
            }
            return true;
        }
        public async Task<Cancion> CancionDetails(int _id)
        {
            var conn = new SqlConnection(_configuration.Value);
            const string query = @"SELECT  
                Nombre_cancion,
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_Genero = Genero_ID),
                URL,
                Extension,
                Fecha_publicacion_cancion,
                Favoritos,
                Vistas,
                Usuario_ID = (SELECT Nombre_user FROM AspNetUsers WHERE Id = Usuario_ID ), 
                User_Titulo_Album = (Select Titulo_album from Album where ID_Album = Album_ID)                
                FROM Cancion WHERE ID_cancion = @_ID";

            return await conn.QueryFirstOrDefaultAsync<Cancion>(query.ToString(), new { _Id = _id });
        }

        public async Task<IEnumerable<Cancion>> CancionListPersonal()
        {
            IEnumerable<Cancion> canciones;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var _email = _httpContextAccessor.HttpContext.User.Identity.Name;
                const string query = @"SELECT 
                ID_cancion,
                Nombre_cancion, 
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_Genero = Genero_ID), 
                URL,
                Fecha_publicacion_cancion FROM Cancion WHERE Usuario_ID = (SELECT ID FROM AspNetUsers WHERE Email = @_email)";

                canciones = await conn.QueryAsync<Cancion>(query.ToString(), new { _email = _email });
            }
            return canciones;
        }

        public async Task<bool> GustadoCancion(Cancion cancion, int _id, int contMegusta)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();

                const string query = @"UPDATE Cancion SET Favoritos = @contMegusta WHERE ID_cancion = @_id";
                await conn.ExecuteAsync(query, new
                {
                    _id = _id,
                    contMegusta = contMegusta
                }, commandType: CommandType.Text);
            }
            return true;
        }

        public async Task<bool> VistasCancion(Cancion cancion, int _id, int contVistas)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();

                const string query = @"UPDATE Cancion SET Vistas = @contVistas WHERE ID_cancion = @_id";
                await conn.ExecuteAsync(query, new
                {
                    _id = _id,
                    contVistas = contVistas
                }, commandType: CommandType.Text);
            }
            return true;
        }

        public async Task<bool> EliminarCancion(int _id)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();

                const string query = @"DELETE FROM Cancion WHERE ID_cancion = @_id";
                await conn.ExecuteAsync(query, new
                {
                    _id = _id,
                }, commandType: CommandType.Text);
            }
            return true;
        }

        public async Task<IEnumerable<Cancion>> BusquedaGeneroC(Cancion cancion)
        {
            IEnumerable<Cancion> canciones;
            using (var conn = new SqlConnection(_configuration.Value))
            {
            const string query = @"SELECT
                Id_cancion,
                Nombre_cancion, 
                URL,
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_Genero = Genero_ID),
                Fecha_publicacion_cancion
                FROM Cancion WHERE Genero_ID = @Genero_ID";
            canciones = await conn.QueryAsync<Cancion>(query.ToString(), new { Genero_ID = cancion.Genero_ID});
            }
            return canciones;
        }


        public async Task<IEnumerable<Cancion>> GenerarLAleatoria(int genero, int Formulario)
        {
            IEnumerable<Cancion> canciones;
            using (var conn = new SqlConnection(_configuration.Value))
            {
            const string query = @"SELECT TOP (@Cantidad)
                ID_cancion, 
                Nombre_cancion, 
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_genero = Genero_ID),
                Fecha_publicacion_cancion,
                URL,
                Extension 
                FROM Cancion 
                WHERE Genero_ID = @Genero_ID 
                ORDER BY NEWID()";
            canciones = await conn.QueryAsync<Cancion>(query.ToString(), new { Cantidad = Formulario,
            Genero_ID = genero });
            }
            return canciones;
        }

        public async Task<IEnumerable<Cancion>> GenerarLista(Cancion cancion)
        {
            IEnumerable<Cancion> canciones;
            using (var conn = new SqlConnection(_configuration.Value))
            {
            const string query = @"SELECT TOP (@Cantidad)
                ID_cancion, 
                Nombre_cancion, 
                Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_genero = Genero_ID),
                Fecha_publicacion_cancion,
                URL,
                Extension 
                FROM Cancion 
                ORDER BY NEWID()";
            canciones = await conn.QueryAsync<Cancion>(query.ToString(), new { cantidad = cancion.cantidad });
            }
            return canciones;
        }

        public async Task<IEnumerable<Cancion>> TraerCancionesUsuario(Usuario usuario)
        {
            IEnumerable<Cancion> canciones;
            using (var conn = new SqlConnection(_configuration.Value))
            {
            const string query = @"SELECT ID_cancion, Nombre_cancion, Favoritos, Vistas, 
            Nombre_Genero = (SELECT Genero_Nombre FROM Genero WHERE ID_genero = Genero_ID),
            Fecha_publicacion_cancion FROM Cancion 
            WHERE Usuario_ID = (SELECT Id FROM AspNetUsers WHERE Nombre_user = @Nombre_usuario AND Apellido_user = @Apellido_usuario)";
            canciones = await conn.QueryAsync<Cancion>(query.ToString(), new { 
                    Nombre_usuario = usuario.Nombre_usuario,
                    Apellido_usuario = usuario.Apellido_usuario,
                });
            }
            return canciones;
        }

        public async Task<IEnumerable<Cancion>> CancionesFamosas()
        {
            IEnumerable<Cancion> canciones;
            using (var conn = new SqlConnection(_configuration.Value))
            {
            const string query = @"SELECT TOP 10 ID_cancion, Nombre_cancion, 
            Fecha_publicacion_cancion,
            Vistas,
            URL FROM Cancion WHERE Vistas > 0 ORDER BY Vistas DESC";
            canciones = await conn.QueryAsync<Cancion>(query.ToString(), new { });
            }
            return canciones;
        }

        public async Task<IEnumerable<Cancion>> CancionesGustados(){
            var EmailUsuario = _httpContextAccessor.HttpContext.User.Identity.Name;
            IEnumerable<Cancion> canciones;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"SELECT DISTINCT Id_cancion, Nombre_cancion, Nombre_Genero = 
                (SELECT Genero_Nombre FROM Genero WHERE Genero_ID = ID_genero), Fecha_publicacion_cancion,
                Favoritos
                FROM Cancion
                JOIN FavoritosCancion
                ON Cancion.ID_cancion = FavoritosCancion.Cancion_ID
                RIGHT JOIN AspNetUsers
                ON (SELECT Id FROM AspNetUsers where Email = @EmailUsuario) = FavoritosCancion.User_ID";
                canciones = await conn.QueryAsync<Cancion>(query.ToString(), new { EmailUsuario });
            }
            return canciones;
        }
    }
}
