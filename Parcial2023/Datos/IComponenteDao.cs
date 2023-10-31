using Produccion.Domino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial2023.Datos
{
    public interface IComponenteDao
    {
        List<Componente> Consultar();
    }
}
