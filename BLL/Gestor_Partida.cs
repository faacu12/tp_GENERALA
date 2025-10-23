using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL
{
    public class Gestor_Partida
    {
        private Partida _partidaActual;
        private Gestor_Turno _gestorTurnos;

        public Gestor_Partida()
        {
            _gestorTurnos = new Gestor_Turno();
        }

        public void IniciarPartida(List<Usuario> usuariosenpartida)
        {
            _partidaActual = new Partida();
            _gestorTurnos.InicializarJugadores(usuariosenpartida);
        }

        public void FinalizarPartida(Usuario ganador = null)
        {
            if (_partidaActual == null) return;

            _partidaActual.FechaFin = DateTime.Now;
            _partidaActual.Ganador = ganador;

            
        }

        public void SumarPuntos(int jugadorIndice, int puntos)
        {
            if (_partidaActual == null) return;

            if (jugadorIndice == 0)
                _partidaActual.PuntajeJugador1 += puntos;
            else if (jugadorIndice == 1)
                _partidaActual.PuntajeJugador2 += puntos;
        }

        public Usuario ObtenerJugadorActual()
        {
            return _gestorTurnos.UsuarioActual;
        }

        public void CambiarTurno()
        {
            _gestorTurnos.Cambiar();
        }

        public Partida ObtenerPartidaActual()
        {
            return _partidaActual;
        }
    }
}

