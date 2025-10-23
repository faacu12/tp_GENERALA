using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Dado
    {
        public int Valor { get; set; }
        public bool Retenido { get; set; }

        public Dado()
        {
            Valor = 1;
            Retenido = false;
        }
    }
}
