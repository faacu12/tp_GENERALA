using BE;
using System;
using System.Collections.Generic;

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
            _partidaActual.Ganador = ganador ?? CalcularGanadorPorPuntaje();
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

        public Usuario Ganador()
        {
            if (_partidaActual == null) return null;
            if (_partidaActual.Ganador != null) return _partidaActual.Ganador;

            return CalcularGanadorPorPuntaje();
        }

        private Usuario CalcularGanadorPorPuntaje()
        {
            if (_partidaActual == null || _gestorTurnos == null || _gestorTurnos.Usuarios == null || _gestorTurnos.Usuarios.Count < 2)
                return null;

            if (_partidaActual.PuntajeJugador1 > _partidaActual.PuntajeJugador2)
                return _gestorTurnos.Usuarios[0];

            if (_partidaActual.PuntajeJugador2 > _partidaActual.PuntajeJugador1)
                return _gestorTurnos.Usuarios[1];

            // Empate
            return null;
        }
    }
}

