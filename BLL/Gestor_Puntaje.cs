using System;

namespace BLL
{
    // Responsabilidad: devolver puntajes base de categorías.
    // - Números: numero * cantidad
    // - Combinaciones: por código/nombre ("e", "full", "dg", etc.)
    public class Gestor_Puntaje
    {
        // Puntos base (ajústalos si tu reglamento usa otros valores)
        public const int PuntosEscalera = 25;
        public const int PuntosFull = 35;
        public const int PuntosPoker = 45;
        public const int PuntosGenerala = 50;
        public const int PuntosDobleGenerala = 100;

        public int CalcularPuntajeNumero(int numero, int cantidad)
        {   
            if (numero < 1 || numero > 6) throw new ArgumentOutOfRangeException(nameof(numero));
            if (cantidad < 0 || cantidad > 5) throw new ArgumentOutOfRangeException(nameof(cantidad));
            return numero * cantidad;
        }

        // Recibe código o nombre: "e"/"escalera", "f"/"full", "p"/"poker", "g"/"generala", "dg"/"doble generala"
        public int CalcularPuntajeCombinacion(string combinacion)
        {
            if (string.IsNullOrWhiteSpace(combinacion)) return 0;
            string key = combinacion.Trim().ToLowerInvariant();

            switch (key)
            {
                case "e":
                case "escalera":
                    return PuntosEscalera;

                case "f":
                case "full":
                    return PuntosFull;

                case "p":
                case "poker":
                case "póker":
                    return PuntosPoker;

                case "g":
                case "generala":
                    return PuntosGenerala;

                case "dg":
                case "doble generala":
                case "doblegenerala":
                    return PuntosDobleGenerala;

                default:
                    return 0;
            }
        }
    }
}