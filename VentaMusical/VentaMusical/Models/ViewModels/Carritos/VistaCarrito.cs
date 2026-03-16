using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VentaMusical.Models.ViewModels.Usuarios;

namespace VentaMusical.Models.ViewModels.Carritos
{
    public class VistaCarrito
    {
        public List<CarritoItem> Carrito { get; set; }
        public ResumenFactura Resumen { get; set; }
        public Usuario Usuario { get; set; }
        public VaciarCarrito VistaVaciaInfo { get; set; }
    }
}