using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotifo.Data.Model
{
    public class Album
    {
        public int ID_album { get; set; }
        public string Titulo_album { get; set; }
        public string Genero_ID { get; set; }
        public byte[] Cover_image { get; set; }
        public DateTime Fecha_publicacion_album { get; set; }
        public string Descripcion { get; set; }
        public string Usuario_ID { get; set; }
        public string _EmailUser { get; set;}
        public string Nombre_Genero { get; set; }
        public int Favoritos { get; set; }
    }
}
