using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public abstract class MAPPER<T>
    {
        internal Acceso acceso;
        public abstract int Agregar(T objeto);
        public abstract int Eliminar(T objeto);
        public abstract int Modificar(T objeto);
        public abstract List<T> Listar();

    }
}
