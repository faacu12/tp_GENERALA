using System;
using BE;

namespace BLL
{
    public class Gestor_Puntaje
    {
        public const int PuntosEscalera = 25;
        public const int PuntosFull = 30;
        public const int PuntosPoker = 40;
        public const int PuntosGenerala = 50;
        public const int PuntosDobleGenerala = 100;

        // Público: valida si la tirada cumple la categoría (número o combinación)
        public bool CumpleCategoria(string categoriaNombre, int[] valores, Tablero tablero)
        {
            if (valores == null || valores.Length != 5) return false;

            int n;
            if (int.TryParse((categoriaNombre ?? "").Trim(), out n) && n >= 1 && n <= 6)
            {
                int[] freq = ContarFrecuencias(valores);
                return freq[n] >= 1; // aunque sea tengo q tener un dado de ese numero
            }

            string code = MapearCategoriaACodigo(categoriaNombre);
            bool generalaYaUsada = GeneralaAnotadaConPuntos(tablero);
            return CumpleCombinacionPorCodigo(code, valores, generalaYaUsada);
        }

        
        public int CalcularPuntajeParaCategoria(string categoriaNombre, int[] valores, Tablero tablero)
        {
            if (valores == null || valores.Length != 5) return 0;

            int n;
            if (int.TryParse((categoriaNombre ?? "").Trim(), out n) && n >= 1 && n <= 6)
            {
                int[] freq = ContarFrecuencias(valores);
                int cantidad = freq[n];
                return CalcularPuntajeNumero(n, cantidad);
            }

            string code = MapearCategoriaACodigo(categoriaNombre);
            bool generalaYaUsada = GeneralaAnotadaConPuntos(tablero);
            if (!CumpleCombinacionPorCodigo(code, valores, generalaYaUsada)) return 0;

            return CalcularPuntajeCombinacion(code);
        }

        #region "RETORNAR PUNTOS FINALES"
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
        #endregion

        // Mapea nombre mostrado a código simple para combinaciones
        private string MapearCategoriaACodigo(string nombre)
        {
            string k = (nombre ?? string.Empty).Trim().ToLowerInvariant();
            switch (k)
            {
                case "escalera": return "e";
                case "full": return "f";
                case "poker":
                case "póker": return "p";
                case "generala": return "g";
                case "doble generala": return "dg";
                default: return string.Empty;
            }
        }

        private bool GeneralaAnotadaConPuntos(Tablero tablero)
        {
            if (tablero == null || tablero.Categorias == null) return false;
            for (int i = 0; i < tablero.Categorias.Count; i++)
            {
                BE.Categoria c = tablero.Categorias[i];
                if (c != null && string.Equals(c.Nombre, "Generala", StringComparison.OrdinalIgnoreCase))
                {
                    return c.Puntuacion.HasValue && c.Puntuacion > 0;
                }
            }
            return false;
        }

        private int[] ContarFrecuencias(int[] valores)
        {
            int[] frecuencia = new int[7]; // índices 1..6
            for (int i = 0; i < valores.Length; i++)
            {
                int v = valores[i];
                if (v >= 1 && v <= 6) frecuencia[v]++;
            }
            return frecuencia;
        }

        private bool CumpleCombinacionPorCodigo(string code, int[] valores, bool generalaYaUsada)
        {
            if (string.IsNullOrEmpty(code)) return false;

            // Copia ordenada
            int[] a = new int[valores.Length];
            for (int i = 0; i < valores.Length; i++) a[i] = valores[i];
            Array.Sort(a);

            int[] freq = ContarFrecuencias(a);

            switch (code)
            {
                case "e":
                    // tengo q poder aceptar: 1-2-3-4-5 o 2-3-4-5-6
                    int[] e1 = new int[] { 1, 2, 3, 4, 5 };
                    int[] e2 = new int[] { 2, 3, 4, 5, 6 };
                    return ArraysIguales(a, e1) || ArraysIguales(a, e2);

                case "f":
                    
                    bool hayTres = false, hayDos = false;
                    for (int v = 1; v <= 6; v++)
                    {
                        if (freq[v] == 3) hayTres = true;
                        else if (freq[v] == 2) hayDos = true;
                    }
                    return hayTres && hayDos;

                case "p":
                   
                    for (int v = 1; v <= 6; v++)
                        if (freq[v] == 4) return true;
                    return false;

                case "g":
                    
                    for (int v = 1; v <= 6; v++)
                        if (freq[v] == 5) return true;
                    return false;

                case "dg":
                   
                    if (!generalaYaUsada) return false;
                    for (int v = 1; v <= 6; v++)
                        if (freq[v] == 5) return true;
                    return false;

                default:
                    return false;
            }
        }

        private bool ArraysIguales(int[] a, int[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return false;
            return true;
        }
    }
}