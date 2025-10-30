using BE;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class TableroService
    {
        public Tablero CrearTablero(Usuario jugador)
        {
            Tablero tablero = new Tablero(jugador);
            InicializarCategorias(tablero);
            return tablero;
        }

        public void InicializarCategorias(Tablero tablero)
        {
            tablero.Categorias = new List<Categoria>
            {
                // Categorías de números
                new Categoria { Nombre = "1", Descripcion = "Suma de todos los 1", Tipo = TipoCategoria.Numero },
                new Categoria { Nombre = "2", Descripcion = "Suma de todos los 2", Tipo = TipoCategoria.Numero },
                new Categoria { Nombre = "3", Descripcion = "Suma de todos los 3", Tipo = TipoCategoria.Numero },
                new Categoria { Nombre = "4", Descripcion = "Suma de todos los 4", Tipo = TipoCategoria.Numero },
                new Categoria { Nombre = "5", Descripcion = "Suma de todos los 5", Tipo = TipoCategoria.Numero },
                new Categoria { Nombre = "6", Descripcion = "Suma de todos los 6", Tipo = TipoCategoria.Numero },
                
                // Categorías de juego
                new Categoria { Nombre = "Escalera", Descripcion = "Secuencia de 5 números", Tipo = TipoCategoria.Juego },
                new Categoria { Nombre = "Full", Descripcion = "3 de un número y 2 de otro", Tipo = TipoCategoria.Juego },
                new Categoria { Nombre = "Poker", Descripcion = "4 dados iguales", Tipo = TipoCategoria.Juego },
                new Categoria { Nombre = "Generala", Descripcion = "5 dados iguales", Tipo = TipoCategoria.Juego },
                new Categoria { Nombre = "Doble Generala", Descripcion = "Segunda Generala", Tipo = TipoCategoria.Juego }
            };
        }
        public bool AnotarPuntuacion(Tablero tablero, string nombreCategoria, int puntos)
        {
            var categoria = tablero.Categorias.FirstOrDefault(c => c.Nombre == nombreCategoria);
            if (categoria != null && !categoria.Utilizada)
            {
                categoria.Puntuacion = puntos;
                return true;
            }
            return false;
        }

        public bool TodasCategoriasCompletadas(Tablero tablero)
        {
            return !tablero.Categorias.Any(c => !c.Utilizada);
        }
        
        public int CalcularTotalPuntos(Tablero tablero)
        {
            return tablero.Categorias.Sum(c => c.Puntuacion ?? 0);
        }
    }
}