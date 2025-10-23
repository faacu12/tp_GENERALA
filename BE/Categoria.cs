using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Categoria
    {
        public string Nombre { get; set; }
        public int? Puntuacion { get; set; }
        public bool Utilizada { get { return Puntuacion.HasValue; } }
        public string Descripcion { get; set; }
        public TipoCategoria Tipo { get; set; }

        public Categoria()
        {
           
            Puntuacion = null;
        }
    }

    public enum TipoCategoria
    {
        Numero,
        Juego
    }
}

