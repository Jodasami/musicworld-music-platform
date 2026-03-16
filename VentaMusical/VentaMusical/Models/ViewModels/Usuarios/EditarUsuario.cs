using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Usuarios
{
	public class EditarUsuario : CrearUsuario
	{
        public int IDUsuario { get; set; }
    }
}