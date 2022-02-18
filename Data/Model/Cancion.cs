using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotifo.Data.Model
{
    public class Cancion
    {
        public int ID_cancion { get; set; }
        public string Nombre_cancion { get; set; }
        public string Genero_ID { get; set; }
        public DateTime Fecha_publicacion_cancion { get; set; }
        public string URL { get; set; }
        public int Favoritos { get; set; }
        public int Vistas { get; set; }
        public string Usuario_ID { get; set; }
        public int Album_ID { get; set; }
        public string Extension { get; set; }
        public string User_Titulo_Album { get; set; }
        public string _EmailUser { get; set; }
        public string Nombre_Genero { get; set; }
        public int cantidad { get; set; }
    }
}
