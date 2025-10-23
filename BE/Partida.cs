using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Partida
    {
        public int IdPartida { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public Usuario Ganador { get; set; }
        public int PuntajeJugador1 { get; set; }
        public int PuntajeJugador2 { get; set; }
        
        public List<Turno> HistorialTurnos { get; set; }

        public Partida()
        {
            FechaInicio = DateTime.Now;
            HistorialTurnos = new List<Turno>();
        }
    }
}
