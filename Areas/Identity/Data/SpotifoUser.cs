using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

// Se creo esta clase ya que IdentityUser que es la que tiene el modelo de Users de AspNet Core no se puede modificar
// Por lo cual se creo esta heredando la otra pero añadiendo las cosas extras.

namespace Spotifo.Areas.Identity.Data
{
    public class SpotifoUser : IdentityUser
    {
        [PersonalData]
        public string Nombre_user { get; set; }

        [PersonalData]
        public string Apellido_user { get; set; }

        [PersonalData]
        public string Biografia_user { get; set; }

        [PersonalData]
        public string Nacionalidad { get; set; }

        [PersonalData]
        public byte[] ImagePerfil { get; set; }

    }
}
