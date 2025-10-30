using BE;
using DAL;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Gestor_Partida
    {
        private Partida _partidaActual;
        private Gestor_Turno _gestorTurnos;
        private XmlBitacora xmlbitacora;

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
            var turno = new Turno
            {
                JugadorActual = ObtenerJugadorActual(),
                NumeroTurno = (_partidaActual.HistorialTurnos?.Count ?? 0) + 1
            };
            _partidaActual.HistorialTurnos.Add(turno);
        }

        public void FinalizarPartida(Usuario ganador = null)
        {
            if (_partidaActual == null) return;
            _partidaActual.FechaFin = System.DateTime.Now;
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
            return null; 
        }
        public DataTable ConsultarTurnos()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NroTurno", typeof(int));
            dt.Columns.Add("Jugador", typeof(string));

            if (_partidaActual?.HistorialTurnos != null)
            {
                foreach (Turno t in _partidaActual.HistorialTurnos)
                {
                    dt.Rows.Add(t.NumeroTurno, t.JugadorActual?.Nombre);
                }
            }
            return dt;
        }
        public Usuario ObtenerJugadorActual() => _gestorTurnos?.UsuarioActual;
        public void CambiarTurno()
        {
            _gestorTurnos?.Cambiar();
        }
    }
}

