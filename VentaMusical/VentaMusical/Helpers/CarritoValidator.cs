using System.Collections.Generic;
using System.Linq;
using VentaMusical.Models.ViewModels.Carritos;
using VentaMusical.Models.ViewModels.Usuarios;


namespace VentaMusical.Models.ViewModels.CarritoValidator
{
    public class ResultadoValidacionCarrito
    {
        public bool EsValido { get; set; }
        public string MensajeError { get; set; }
    }

    public class CarritoValidator
    {
        public static ResultadoValidacionCarrito Validar(List<CarritoItem> carrito, Usuario
            usuario, VentaMusicalEntities db)
        {
            if (carrito == null || !carrito.Any())
                return new ResultadoValidacionCarrito { EsValido = false, MensajeError = "El carrito está vacío." };

            foreach (var item in carrito)
            {
                var cancion = db.Canciones.Find(item.CodigoCancion);
                if (cancion == null || cancion.CantidadDisponible < item.Cantidad)
                    return new ResultadoValidacionCarrito { EsValido = false, MensajeError = $"Stock insuficiente para {item.NombreCancion}." };
            }

            decimal total = carrito.Sum(i => i.SubTotal) * 1.13m;
            if (usuario.DineroDisponible < total)
                return new ResultadoValidacionCarrito { EsValido = false, MensajeError = "Saldo insuficiente para realizar la compra." };

            return new ResultadoValidacionCarrito { EsValido = true };
        }
    }
}