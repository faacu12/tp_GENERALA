using BE;
using System.Collections.Generic;

namespace BLL
{
    public class Gestor_Partida
    {
        private Partida _partidaActual;
        private Gestor_Turno _gestorTurnos;

        public void IniciarPartida(List<Usuario> usuariosenpartida)
        {
            _partidaActual = new Partida
            {
                PuntajeJugador1 = 0,
                PuntajeJugador2 = 0,
                HistorialTurnos = new List<Turno>()
            };
            _gestorTurnos = new Gestor_Turno();
            _gestorTurnos.InicializarJugadores(usuariosenpartida);
        }

        public void SumarPuntos(int jugadorIndice, int puntos)
        {
            if (_partidaActual == null) return;
            if (jugadorIndice == 0) _partidaActual.PuntajeJugador1 += puntos;
            else if (jugadorIndice == 1) _partidaActual.PuntajeJugador2 += puntos;
        }

        public void FinalizarPartida(Usuario ganador = null)
        {
            if (_partidaActual == null) return;
            _partidaActual.FechaFin = System.DateTime.Now;
            // Si no viene, calcular por puntaje
            _partidaActual.Ganador = ganador ?? CalcularGanadorPorPuntaje();
        }

        public Usuario Ganador()
        {
            if (_partidaActual == null) return null;
            return _partidaActual.Ganador ?? CalcularGanadorPorPuntaje();
        }

        private Usuario CalcularGanadorPorPuntaje()
        {
            if (_gestorTurnos == null || _gestorTurnos.Usuarios == null || _gestorTurnos.Usuarios.Count < 2) return null;

            if (_partidaActual.PuntajeJugador1 > _partidaActual.PuntajeJugador2) return _gestorTurnos.Usuarios[0];
            if (_partidaActual.PuntajeJugador2 > _partidaActual.PuntajeJugador1) return _gestorTurnos.Usuarios[1];
            return null; // empate
        }

        public Usuario ObtenerJugadorActual() => _gestorTurnos?.UsuarioActual;
        public void CambiarTurno() => _gestorTurnos?.Cambiar();
    }
}

