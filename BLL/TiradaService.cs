using BE;
using System.Collections.Generic;

namespace BLL
{
    public class TiradaService
    {
        private DadoService _dadoService = new DadoService();

        public Tirada CrearNuevaTirada(int cantidadDados = 5)
        {
            Tirada tirada = new Tirada();
            
            for (int i = 0; i < cantidadDados; i++)
            {
                tirada.Dados.Add(new Dado());
            }
            
            return tirada;
        }

        public void LanzarDados(Tirada tirada)
        {
            if (tirada.NumeroLanzamientos < tirada.MaximoLanzamientos)
            {
                foreach (var dado in tirada.Dados)
                {
                    _dadoService.TirarDado(dado);
                }
                tirada.NumeroLanzamientos++;
            }
        }
        public void RetenerDado(Tirada tirada, int indice, bool retener)
        {
            if (indice >= 0 && indice < tirada.Dados.Count)
            {
                _dadoService.CambiarRetencion(tirada.Dados[indice], retener);
            }
        }

        public void ReiniciarTirada(Tirada tirada)
        {
            tirada.NumeroLanzamientos = 0;
            foreach (var dado in tirada.Dados)
            {
                dado.Retenido = false;
            }
        }

        public int[] ObtenerValoresDados(Tirada tirada)
        {
            int[] valores = new int[tirada.Dados.Count];
            for (int i = 0; i < tirada.Dados.Count; i++)
            {
                valores[i] = tirada.Dados[i].Valor;
            }
            return valores;
        }

        public int ObtenerLanzamientosRestantes(Tirada tirada)
        {
            return tirada.MaximoLanzamientos - tirada.NumeroLanzamientos;
        }
    }
}