using Produccion.Datos;
using Produccion.Domino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial2023.Datos.Servicios
{
    public class GestorOrden
    {
        private OrdenDao dao;

        public GestorOrden()
        {
            this.dao = new OrdenDao();
        }

        public bool Confirmar(OrdenProduccion orden)
        {
            return dao.Confirmar(orden);
        }
    }
}
