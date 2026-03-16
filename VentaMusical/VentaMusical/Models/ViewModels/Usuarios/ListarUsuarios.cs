using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Usuarios
{
	public class ListarUsuarios : Usuario
	{
        [Display(Name = "ID")]
        public int ID { get; set; }


        [Display(Name = "Cedula")]
        public string NumeroIdentificacion { get; set; }


        [Display(Name = "Nombre")]
        public string Nombre { get; set; }


        [Display(Name = "Apellido")]
        public string Apellido { get; set; }


        [Display(Name = "Genero")]
        public string Genero { get; set; }


        [Display(Name = "Correo")]
        public string Correo { get; set; }


        [Display(Name = "Tipo de Tarjeta")]
        public string TipoTarjeta { get; set; }


        [Display(Name = "Dinero Disponible")]
        public decimal DineroDisponible { get; set; }


        [Display(Name = "Numero de Tarjeta")]
        public string NumeroTarjeta { get; set; }


        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Display(Name = "Rol")]
        public string Perfil { get; set; }
    }
}