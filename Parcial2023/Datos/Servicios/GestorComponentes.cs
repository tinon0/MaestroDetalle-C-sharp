using Produccion.Domino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial2023.Datos.Servicios
{
    public class GestorComponentes
    {
        private ComponenteDao dao;

        public GestorComponentes()
        {
            this.dao = new ComponenteDao();
        }

        public List<Componente> Consultar()
        {
            return dao.Consultar();
        }
    }
}
