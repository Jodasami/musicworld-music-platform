using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Usuarios
{
	public class CrearUsuario
	{
        [Required(ErrorMessage = "La cedula es obligatoria")]
        [Display(Name = "Cedula")]
        public string NumeroIdentificacion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El genero es obligatorio")]
        [Display(Name = "Genero")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido")]
        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo")]
        [Display(Name = "Tipo de Tarjeta")]
        public string TipoTarjeta { get; set; }

        [Required(ErrorMessage = "El monto minimo es 0")]
        [Display(Name = "Dinero Disponible")]
        public decimal DineroDisponible { get; set; }

        [Required(ErrorMessage = "Debe ingresar un Num. de tarjeta")]
        [RegularExpression(@"^\d{4}-\d{4}-\d{4}-\d{4}$", ErrorMessage = "Formato inválido. Use 0000-0000-0000-0000")]
        [Display(Name = "Numero de Tarjeta")]
        public string NumeroTarjeta { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "Debe de tener un rol")]
        [Display(Name = "Rol")]
        public string Perfil { get; set; }
    }
}