using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Canciones
{
	public class EditarCancion : CrearCancion
	{
        public int CodigoCancion { get; set; }
    }
}