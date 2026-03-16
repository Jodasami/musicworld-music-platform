using System.Linq;
using VentaMusical.Models.ViewModels.DetallesVentas;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels.Usuarios;

namespace VentaMusical.Helpers
{
    public static class VentaMapper
    {
        public static VentaMusical.Models.ViewModels.Ventas.Venta Mapear(Ventas ventaEntity)
        {
            return new VentaMusical.Models.ViewModels.Ventas.Venta
            {
                NumeroFactura = ventaEntity.NumeroFactura,
                FechaCompra = ventaEntity.FechaCompra,
                TipoPago = ventaEntity.TipoPago,
                TotalFinal = ventaEntity.TotalFinal,
                Usuarios = new Usuario
                {
                    Nombre = ventaEntity.Usuarios?.Nombre,
                    Correo = ventaEntity.Usuarios?.CorreoElectronico
                },
                Detalle = ventaEntity.DetalleVenta?.Select(d => new ListarDetallesVentas
                {
                    IDDetalle = d.IDDetalle,
                    NumeroFactura = d.NumeroFactura,
                    CodigoCancion = d.CodigoCancion,
                    NombreCancion = d.NombreCancion,
                    PrecioUnitario = d.PrecioUnitario,
                    Cantidad = d.Cantidad,                   
                }).ToList()
            };
        }
    }
}
