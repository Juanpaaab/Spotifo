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
    public class UsuarioService : IUsuarioService
    {
        //Connecction Sql Server
        private readonly SqlConnectionConfiguration _configuration;

        public UsuarioService(SqlConnectionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> UsuarioInsert(Usuario usuario)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();
                parameters.Add("Nombre_usuario", usuario.Nombre_usuario, DbType.String);
                parameters.Add("Apellido_usuario", usuario.Apellido_usuario, DbType.String);
                parameters.Add("Email", usuario.Email, DbType.String);
                parameters.Add("Password", usuario.Password, DbType.String);
                parameters.Add("Descripcion", usuario.Descripcion, DbType.String);

                if (usuario.Nombre_usuario != ""
                    && usuario.Apellido_usuario != ""
                    && usuario.Email != ""
                    && usuario.Password != ""
                    && usuario.Descripcion != "")
                {
                    const string query = @"INSERT INTO Usuario
                    (Nombre_usuario, 
                    Apellido_usuario,
                    Email,
                    Password,
                    Descripcion) 
                    VALUES (@Nombre_usuario, 
                    @Apellido_usuario,
                    @Email,
                    @Password,
                    @Descripcion)";
                    await conn.ExecuteAsync(query, new
                    {
                        usuario.Nombre_usuario,
                        usuario.Apellido_usuario,
                        usuario.Email,
                        usuario.Password,
                        usuario.Descripcion
                    }, commandType: CommandType.Text);
                }
            }
            return true;
        }
        public async Task<Usuario> UsuarioDetails(string _email)
        {

            var conn = new SqlConnection(_configuration.Value);
            const string query = @"SELECT Nombre_usuario = Nombre_user, 
            Apellido_usuario = Apellido_user, 
            Email, 
            Descripcion = Biografia_user,
            Nacionalidad,
            Cancion_Num = (SELECT COUNT(Id_cancion) FROM Cancion WHERE Usuario_ID = (SELECT ID FROM AspNetUsers WHERE Email = @_email))
            FROM AspNetUsers WHERE Email= @_email";
            return await conn.QueryFirstOrDefaultAsync<Usuario>(query.ToString(), new { _Email = _email });
        }

        public async Task<IEnumerable<Usuario>> GetUsuarios()
        {
            IEnumerable<Usuario> usuarios;
            using (var conn = new SqlConnection(_configuration.Value))
            {
                const string query = @"SELECT Id_usuario = ID, Nombre_usuario = Nombre_user, Apellido_usuario = Apellido_user, Email FROM AspNetUsers";

                usuarios = await conn.QueryAsync<Usuario>(query.ToString(), new { });
            }
            return usuarios;
        }

        public async Task<bool> EliminarUsuario(string _id)
        {
            using (var conn = new SqlConnection(_configuration.Value))
            {
                var parameters = new DynamicParameters();

                

                const string query2 = @"DELETE FROM Cancion WHERE Usuario_ID = @_id";
                await conn.ExecuteAsync(query2, new
                {
                    _id = _id,
                }, commandType: CommandType.Text);

                const string query3 = @"DELETE FROM Album WHERE Usuario_ID = @_id";
                await conn.ExecuteAsync(query3, new
                {
                    _id = _id,
                }, commandType: CommandType.Text);

                const string query1 = @"DELETE FROM AspNetUsers WHERE ID = @_id";
                await conn.ExecuteAsync(query1, new
                {
                    _id = _id,
                }, commandType: CommandType.Text);
            }
            return true;
        }

         public async Task<Usuario> UsuarioDetailsUnico(string _id)
        {
            var conn = new SqlConnection(_configuration.Value);
            const string query = @"SELECT Id_usuario = ID, 
                Nombre_usuario = Nombre_user, 
                Apellido_usuario = Apellido_user, 
                Descripcion = Biografia_user,
                Email FROM AspNetUsers WHERE ID = @_id";
            return await conn.QueryFirstOrDefaultAsync<Usuario>(query.ToString(), new { _Id = _id });
            
        }

        public async Task<bool> UsuarioUpdate(Usuario usuario, string _id)
          {
              using (var conn = new SqlConnection(_configuration.Value))
              {
                  var parameters = new DynamicParameters();
                  parameters.Add("Nombre_usuario", usuario.Nombre_usuario, DbType.String);
                  parameters.Add("Apellido_usuario", usuario.Apellido_usuario, DbType.String);
                  parameters.Add("Email", usuario.Email, DbType.String);
                  parameters.Add("Descripcion", usuario.Descripcion, DbType.String);
                  const string query = @"UPDATE AspNetUsers
                      SET Nombre_user = @Nombre_usuario,
                          Apellido_user = @Apellido_usuario,
                          Email = @Email,
                          Biografia_user = @Descripcion
                          WHERE ID = @_ID";
                  await conn.ExecuteAsync(query, new { 
                      _id = _id,
                      usuario.Nombre_usuario, 
                      usuario.Apellido_usuario, 
                      usuario.Email, 
                      usuario.Descripcion
                    }, commandType: CommandType.Text);
              }
              return true;
          }

        public async Task<IEnumerable<Usuario>> BusquedaNacionalidad(Usuario usuario)
        {
            IEnumerable<Usuario> usuarios;
            using (var conn = new SqlConnection(_configuration.Value))
            {
            const string query = @"SELECT
                Nombre_usuario = Nombre_user,
                Apellido_usuario = Apellido_user,
                Descripcion = Biografia_user,
                Nacionalidad,
                Email 
                FROM AspNetUsers WHERE Nacionalidad = @Nacionalidad ";
            usuarios = await conn.QueryAsync<Usuario>(query.ToString(), new { Nacionalidad = usuario.Nacionalidad });
            }
            return usuarios;
        }

    }
}
