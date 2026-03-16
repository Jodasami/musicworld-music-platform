using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Carritos
{
    public class VaciarCarrito
    {
        public string Icono { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public List<BotonVacioModel> Botones { get; set; }
    }    
}
