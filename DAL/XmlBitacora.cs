using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DAL
{
    public class XmlBitacora
    {
        private readonly string _ruta;
        private readonly DataSet _ds = new DataSet("BitacoraDS");
        private static readonly object _sync = new object();

        public XmlBitacora(string ruta = null)
        {
            _ruta = ruta ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bitacora", "Bitacora.xml");
            Directory.CreateDirectory(Path.GetDirectoryName(_ruta));
            Cargar();
        }

        public void Limpiar()
        {
            Cargar();
            DataTable t = _ds.Tables["Bitacora"];
            t.Rows.Clear();
            Guardar();
        }
        // Registrar movimiento de partida (anotar/tachar)
        public void RegistrarMovimiento(string accion, int? usuarioId, string jugadorNombre, string categoria, int puntos, int turno)
        {
            lock (_sync)
            {
                Cargar();
                DataTable t = _ds.Tables["Bitacora"];
                DataRow r = t.NewRow();
                r["FechaHora"] = DateTime.Now;
                r["Tipo"] = "Movimiento";
                r["UsuarioId"] = (object)usuarioId ?? DBNull.Value;
                r["Accion"] = accion ?? string.Empty;               // "Anotar" | "Tachar"
                r["Jugador"] = jugadorNombre ?? string.Empty;
                r["Categoria"] = categoria ?? string.Empty;
                r["Puntos"] = puntos;
                r["Turno"] = turno;
                r["Mensaje"] = string.Empty; // opcional
                t.Rows.Add(r);
                Guardar();
            }
        }

        // Mini consulta: filtros básicos. Devuelve DataTable para bindear en un DataGridView
        public DataTable Consultar(DateTime? desde = null, DateTime? hasta = null, string tipo = null, int? usuarioId = null, string accion = null)
        {
            lock (_sync)
            {
                Cargar();
                DataTable t = _ds.Tables["Bitacora"];
                var q = t.AsEnumerable();

                if (desde.HasValue) q = q.Where(r => r.Field<DateTime>("FechaHora") >= desde.Value);
                if (hasta.HasValue) q = q.Where(r => r.Field<DateTime>("FechaHora") <= hasta.Value);
                if (!string.IsNullOrWhiteSpace(tipo)) q = q.Where(r => string.Equals(r.Field<string>("Tipo") ?? "", tipo, StringComparison.OrdinalIgnoreCase));
                if (usuarioId.HasValue) q = q.Where(r => !r.IsNull("UsuarioId") && r.Field<int>("UsuarioId") == usuarioId.Value);
                if (!string.IsNullOrWhiteSpace(accion)) q = q.Where(r => string.Equals(r.Field<string>("Accion") ?? "", accion, StringComparison.OrdinalIgnoreCase));

                // Orden por fecha desc
                q = q.OrderByDescending(r => r.Field<DateTime>("FechaHora"));

                DataTable resultado = t.Clone(); // conserva esquema/columnas
                foreach (var r in q) resultado.ImportRow(r);
                return resultado;
            }
        }

        private void Cargar()
        {
            _ds.Reset();
            if (File.Exists(_ruta))
            {
                _ds.ReadXml(_ruta, XmlReadMode.ReadSchema);
                // Si el archivo existe pero no tiene la tabla esperada (esquema antiguo), garantizamos el esquema actual
                if (!_ds.Tables.Contains("Bitacora"))
                {
                    CrearEsquema();
                    Guardar();
                }
            }
            else
            {
                CrearEsquema();
                Guardar();
            }
        }

        private void Guardar()
        {
            _ds.WriteXml(_ruta, XmlWriteMode.WriteSchema);
        }

        private void CrearEsquema()
        {
            DataTable t = new DataTable("Bitacora");
            t.Columns.Add(new DataColumn("FechaHora", typeof(DateTime)));
            t.Columns.Add(new DataColumn("Tipo", typeof(string)));       // InicioSesión, CierreSesión, InicioPartida, FinPartida, Movimiento
            t.Columns.Add(new DataColumn("UsuarioId", typeof(int)) { AllowDBNull = true });
            t.Columns.Add(new DataColumn("Accion", typeof(string)));     // Anotar/Tachar (para Tipo=Movimiento)
            t.Columns.Add(new DataColumn("Jugador", typeof(string)));
            t.Columns.Add(new DataColumn("Categoria", typeof(string)));
            t.Columns.Add(new DataColumn("Puntos", typeof(int)) { AllowDBNull = true });
            t.Columns.Add(new DataColumn("Turno", typeof(int)) { AllowDBNull = true });
            t.Columns.Add(new DataColumn("Mensaje", typeof(string)));
            _ds.Tables.Add(t);
        }
    }
}