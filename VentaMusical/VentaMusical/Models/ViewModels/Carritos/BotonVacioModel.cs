using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace VentaMusical.Models.ViewModels.Carritos
{
    public class BotonVacioModel
    {
        public string Texto { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public object RutaParametros { get; set; }
        public string ClaseCss { get; set; }
        public string Titulo { get; set; }
        public string URL { get; set; }
    }
}