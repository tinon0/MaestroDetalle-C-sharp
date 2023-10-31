using Produccion.Datos;
using Produccion.Domino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial2023.Datos
{
    public class OrdenDao : IOrdenDao
    {
        public bool Confirmar(OrdenProduccion orden)
        {
            return DBHelper.GetInstancia().Confirmar(orden);
        }
    }
}
