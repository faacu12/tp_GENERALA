using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Tirada
    {
        public List<Dado> Dados { get; set; }
        public int NumeroLanzamientos { get; set; }
        public int MaximoLanzamientos { get; set; }

        public Tirada()
        {
            Dados = new List<Dado>();
            NumeroLanzamientos = 0;
            MaximoLanzamientos = 3;
        }

        public int[] Valores()
        {
            return Dados.Select(d => d.Valor).ToArray();
        }
    }
}
