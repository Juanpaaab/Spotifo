using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotifo.Data.Model
{
    public class Usuario
    {
        public string ID_usuario { get; set; }
        public string Nombre_usuario { get; set; }
        public string Apellido_usuario { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Descripcion { get; set; }
        public string Cancion_Num { get; set; }
        public string Nacionalidad {get; set; }
    }
}
