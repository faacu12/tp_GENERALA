using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Tablero
    {
        public Usuario Jugador { get; set; }
        public List<Categoria> Categorias { get; set; }

        public Tablero(Usuario jugador)
        {
            Jugador = jugador;
            Categorias = new List<Categoria>();
        }
    }
}
