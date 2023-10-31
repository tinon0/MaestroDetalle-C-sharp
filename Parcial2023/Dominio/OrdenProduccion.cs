using Parcial2023.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produccion.Domino
{
    public class OrdenProduccion
    {
        public DateTime Fecha { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public string Estado { get; set; }
        public List<DetalleOrden> Detalle { get; set; }

        public OrdenProduccion(DateTime fecha, string modelo, int cant, string estado, List<DetalleOrden> detalles)
        {
            this.Fecha = fecha;
            this.Modelo = modelo;
            this.Estado = estado;
            this.Detalle = detalles;
            this.Cantidad = cant;
        }
        public OrdenProduccion()
        {
            Fecha = DateTime.Now;
            Modelo= string.Empty;
            Estado= string.Empty;
            Cantidad = 0;
            Detalle= new List<DetalleOrden>();
        }

        public void Agregar(DetalleOrden detalle)
        {
            Detalle.Add(detalle);
        }
        public void Quitar(int id)
        {
            Detalle.RemoveAt(id);
        }
    }
}
