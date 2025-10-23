using BE;
using System;

namespace BLL
{
    public class DadoService
    {
        private Random _random = new Random();

        public void TirarDado(Dado dado)
        {
            if (!dado.Retenido)
            {
                dado.Valor = _random.Next(1, 7);
            }
        }

        public void CambiarRetencion(Dado dado, bool retenido)
        {
            dado.Retenido = retenido;
        }
    }
}