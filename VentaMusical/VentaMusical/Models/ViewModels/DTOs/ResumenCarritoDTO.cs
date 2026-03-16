using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.DTOs
{
    public class ResumenCarritoDTO
    {
        public List<ItemCarritoDTO> Productos { get; set; }
        public string TipoPago { get; set; }
        public decimal Recargo { get; set; }
        public decimal TotalFinal { get; set; }
    }
    public class ItemCarritoDTO
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}