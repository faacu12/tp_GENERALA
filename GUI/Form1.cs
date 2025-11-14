using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form1 : Form
    {
        private Sesion sesion = new Sesion();
        private Gestor_Partida gestorpartida = new Gestor_Partida();
        private TiradaService tiradaService = new TiradaService();
        private readonly Gestor_Puntaje gestorPuntaje = new Gestor_Puntaje();
        private TableroService tableroService = new TableroService();
        private DadoService dadoService = new DadoService();
        private Tablero tableroJugador1;
        private Tablero tableroJugador2;
        private BE.Tirada tiradaActual;
        BitacoraService bitacora = new BitacoraService();
        private readonly string[] _imagenesPath = new string[6]
        {
            "dado1.png", 
            "dado2.png", 
            "dado3.png",
            "dado4.png",
            "dado5.png",
            "dado6.png"
        };
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
            checkBox2.CheckedChanged += CheckBox2_CheckedChanged;
            checkBox3.CheckedChanged += CheckBox3_CheckedChanged;
            checkBox4.CheckedChanged += CheckBox4_CheckedChanged;
            checkBox5.CheckedChanged += CheckBox5_CheckedChanged;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;

            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView2.ReadOnly = true;

        }

        #region "EVENTOS CHECKBOX"
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (dadoControl1.Dado != null)
            {
                dadoControl1.Dado.Retenido = checkBox1.Checked;
                dadoControl1.ActualizarImagen();
            }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (dadoControl2.Dado != null)
            {
                dadoControl2.Dado.Retenido = checkBox2.Checked;
                dadoControl2.ActualizarImagen();
            }
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (dadoControl3.Dado != null)
            {
                dadoControl3.Dado.Retenido = checkBox3.Checked;
                dadoControl3.ActualizarImagen();
            }
        }

        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (dadoControl4.Dado != null)
            {
                dadoControl4.Dado.Retenido = checkBox4.Checked;
                dadoControl4.ActualizarImagen();
            }
        }

        private void CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (dadoControl5.Dado != null)
            {
                dadoControl5.Dado.Retenido = checkBox5.Checked;
                dadoControl5.ActualizarImagen();
            }
        }
        #endregion
       
        private void Begin()
        {
            try
            {
                List<Usuario> usuariosenpartida = new List<Usuario>();
                if (sesion.Get(0) != null) usuariosenpartida.Add(sesion.Get(0));
                if (sesion.Get(1) != null) usuariosenpartida.Add(sesion.Get(1));
                gestorpartida.IniciarPartida(usuariosenpartida);
                tableroJugador1 = tableroService.CrearTablero(sesion.Get(0));
                tableroJugador2 = tableroService.CrearTablero(sesion.Get(1));
                RefreshTurno();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar partida: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        private void Form1_Shown(object sender, EventArgs e)
        {
            if (sesion.Get(0) != null)
            {
                button3.Visible = false;
            }
            if (sesion.Get(1) != null)
            {
                button4.Visible = false;
            }
            Usuario usuario1 = sesion.Get(0);
            if (usuario1 != null)
            {
                label1.Text = usuario1.Nombre.ToString();
            }
            Usuario usuario2 = sesion.Get(1);
            if (usuario2 != null)
            {
                label2.Text = usuario2.Nombre.ToString();
            }
            
        }
        public void RefreshTurno()
        {
            try
            {
                BE.Usuario jugadorActual = gestorpartida.ObtenerJugadorActual();

                if (jugadorActual != null)
                {
                    label3.Text = $"TURNO DE: {jugadorActual.Nombre}";

                }
                else
                {
                    label3.Text = "No hay jugador activo";
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        private bool TryObtenerGridYTableroActual(out DataGridView gridActual, out Tablero tableroActual)
        {
            gridActual = null;
            tableroActual = null;

            BE.Usuario jugadorActual = gestorpartida.ObtenerJugadorActual();
            if (jugadorActual == null) return false;

            if (tableroJugador1 != null && tableroJugador1.Jugador == jugadorActual)
            {
                gridActual = dataGridView1;
                tableroActual = tableroJugador1;
                return true;
            }
            if (tableroJugador2 != null && tableroJugador2.Jugador == jugadorActual)
            {
                gridActual = dataGridView2;
                tableroActual = tableroJugador2;
                return true;
            }
            return false;
        }

        #region "botones del form"
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (tiradaActual == null || tiradaActual.NumeroLanzamientos <= 0)
                {
                    MessageBox.Show("Debes lanzar al menos una vez antes de anotar.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
               
                if (!TryObtenerGridYTableroActual(out DataGridView gridActual, out Tablero tableroActual))
                {
                    MessageBox.Show("No hay jugador activo o tablero asociado.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DataGridViewRow row = gridActual.CurrentRow;
                if (row == null || row.Index < 0)
                {
                    MessageBox.Show("Selecciona una fila de categoría.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string categoriaNombre = Convert.ToString(row.Cells[0].Value);
                if (string.IsNullOrWhiteSpace(categoriaNombre) ||
                    categoriaNombre.Trim().Equals("TOTAL", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Selecciona una categoría válida (no TOTAL).", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                BE.Categoria categoria = tableroActual.Categorias.FirstOrDefault(c => c.Nombre == categoriaNombre);
                if (categoria == null)
                {
                    MessageBox.Show("Categoría inexistente.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (categoria.Utilizada)
                {
                    MessageBox.Show("Esa categoría ya fue utilizada.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int[] valores = tiradaService.ObtenerValoresDados(tiradaActual);

                
                if (!gestorPuntaje.CumpleCategoria(categoriaNombre, valores, tableroActual))
                {
                    MessageBox.Show("La tirada no cumple con la categoría seleccionada.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int puntos = gestorPuntaje.CalcularPuntajeParaCategoria(categoriaNombre, valores, tableroActual);
                bool ok = tableroService.AnotarPuntuacion(tableroActual, categoriaNombre, puntos);
                if (!ok) { MessageBox.Show("No se pudo anotar la puntuación.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                // 2) Registrar movimiento ANTES de cambiar el turno, con el jugador actual
                BE.Usuario jugadorAccion = gestorpartida.ObtenerJugadorActual();
                bitacora.RegistrarMovimiento("Anotar", jugadorAccion, categoriaNombre, puntos, tiradaActual.NumeroLanzamientos);

                int jugadorIndice = (tableroActual == tableroJugador1) ? 0 : 1;
                gestorpartida.SumarPuntos(jugadorIndice, puntos);

                CrearTablerosVisuales();
                tiradaService.ReiniciarTirada(tiradaActual);
                checkBox1.Checked = checkBox2.Checked = checkBox3.Checked = checkBox4.Checked = checkBox5.Checked = false;
                RefrescarTodasImagenes();
                button6.Enabled = false;

                gestorpartida.CambiarTurno(); // mover después de registrar
                RefreshTurno();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al anotar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Usuario usuario = sesion.Get(0);
            if (usuario != null)
            {
                // Guardamos el nombre para el mensaje
                string nombreUsuario = usuario.Nombre;

                
                Sesion.Eliminar(usuario);

               
                label1.Text = "No iniciado";

                MessageBox.Show($"Se cerró la sesión de {nombreUsuario}", "Sesión cerrada",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                bitacora.RegistrarLogout(usuario);
                dataGridView1.DataSource = null;
                dataGridView1.Visible = false;
                // Verificar si quedan usuarios en sesión
                if (sesion.Get(0) == null && sesion.Get(1) == null)
                {

                    Log formLogin = new Log();
                    formLogin.Show();
                    this.Hide(); 
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Log formLogin = new Log();
            formLogin.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Log formLogin = new Log();
            formLogin.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Usuario usuario;
            if (sesion.Get(1) != null)
            {
                 usuario = sesion.Get(1);
            }
            else
            {
                usuario = sesion.Get(0);
            }
            if (usuario != null)
            {

                string nombreUsuario = usuario.Nombre;
                Sesion.Eliminar(usuario);
                label2.Text = "No iniciado";

                MessageBox.Show($"Se cerró la sesión de {nombreUsuario}", "Sesión cerrada",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                bitacora.RegistrarLogout(usuario);

                if (sesion.Get(0) == null && sesion.Get(1) == null)
                {

                    Log formLogin = new Log();
                    formLogin.Show();
                    this.Hide();
                }
            }
        }
        private void btn_Iniciar_Click(object sender, EventArgs e)
        {
            Begin();
            CrearTablerosVisuales();
            InicializarDados();

            // 1) Crear archivo XML por partida (nombre con fecha y jugadores)
            bitacora.NuevaPartidaBitacora(DateTime.Now, $"{sesion.Get(0)?.Nombre}-{sesion.Get(1)?.Nombre}");

            List<Usuario> jugadores = new List<Usuario>();
            if (sesion.Get(0) != null) jugadores.Add(sesion.Get(0));
            if (sesion.Get(1) != null) jugadores.Add(sesion.Get(1));
            bitacora.RegistrarInicio(jugadores);

            btn_Iniciar.Enabled = false;
            btn_Finalizar.Enabled = true;
            button6.Enabled = false;
        }
        private void button5_Click(object sender, EventArgs e)
        {

            tiradaService.LanzarDados(tiradaActual);
            RefrescarTodasImagenes();
            button6.Enabled = tiradaActual.NumeroLanzamientos > 0;
        }

        private void btn_Finalizar_Click(object sender, EventArgs e)
        {

            gestorpartida.FinalizarPartida();
            BE.Usuario ganador = gestorpartida.Ganador();
            string mensajeGanador = ganador != null ? $"Ganó {ganador.Nombre}" : "La partida ha finalizado en empate.";
            MessageBox.Show($"La partida ha finalizado. {mensajeGanador}", "Partida finalizada");

            List<Usuario> jugadores = new List<Usuario>();
            if (sesion.Get(0) != null) jugadores.Add(sesion.Get(0));
            if (sesion.Get(1) != null) jugadores.Add(sesion.Get(1));
            bitacora.RegistrarFin(jugadores, ganador);

            btn_Finalizar.Enabled = false;
            btn_Iniciar.Enabled = true;

        }
        #endregion
        #region "VISUAL"
        private void CrearTablerosVisuales()
        {
            try
            {
                dataGridView1.Visible = true;
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.Columns.Clear();
                dataGridView1.Columns.Add("categoria", "Categoría");
                dataGridView1.Columns.Add("puntuacion", "Puntuacion");

                dataGridView2.Visible = tableroJugador2 != null;
                dataGridView2.AutoGenerateColumns = false;
                dataGridView2.Columns.Clear();
                dataGridView2.Columns.Add("categoria", "Categoría");
                dataGridView2.Columns.Add("puntuacion", "Puntuacion");

                dataGridView1.Rows.Clear();

                if (tableroJugador1 != null)
                {
                    dataGridView1.Rows.Clear();
                    foreach (BE.Categoria categoria in tableroJugador1.Categorias)
                    {
                        string[] row = new string[]
                        {
                    categoria.Nombre,
                    categoria.Utilizada ? categoria.Puntuacion.ToString() : "-"
                        };
                        dataGridView1.Rows.Add(row);
                    }

                    dataGridView1.Rows.Add(new string[]
                    {
                "TOTAL",
                tableroService.CalcularTotalPuntos(tableroJugador1).ToString()
                    });
                }

                if (tableroJugador2 != null)
                {
                    dataGridView2.Rows.Clear();
                    foreach (var categoria in tableroJugador2.Categorias)
                    {
                        string[] row = new string[]
                        {
                    categoria.Nombre,
                    categoria.Utilizada ? categoria.Puntuacion.ToString() : "-"
                        };
                        dataGridView2.Rows.Add(row);
                    }

                    // Añadir total
                    dataGridView2.Rows.Add(new string[]
                    {
                "TOTAL",
                tableroService.CalcularTotalPuntos(tableroJugador2).ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar tableros: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void RefrescarTodasImagenes()
        {
            dadoControl1.ActualizarImagen();
            dadoControl2.ActualizarImagen();
            dadoControl3.ActualizarImagen();
            dadoControl4.ActualizarImagen();
            dadoControl5.ActualizarImagen();
        }
        #endregion
        private void InicializarDados()
        {
            try
            {
                tiradaActual = tiradaService.CrearNuevaTirada(5);
                DadoControl[] dadosControles = new DadoControl[5] {
                    dadoControl1,
                    dadoControl2,
                    dadoControl3,
                    dadoControl4,
                    dadoControl5
                };
                                    
               
                for (int i = 0; i < dadosControles.Length && i < tiradaActual.Dados.Count; i++)
                {
                    
                    dadosControles[i].Dado = tiradaActual.Dados[i];
                  
                    tiradaActual.Dados[i].Valor = i + 1;
                    dadosControles[i].ActualizarImagen();
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar dados: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dadoControl5_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (tiradaActual == null || tiradaActual.NumeroLanzamientos != 3)
                {
                    MessageBox.Show("Debe quedarse sin tiros para tachar.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if(!TryObtenerGridYTableroActual(out DataGridView gridActual, out Tablero tableroActual))
                {
                    MessageBox.Show("No hay jugador activo o tablero asociado.", "Atención",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                }
                DataGridViewRow row = gridActual.CurrentRow;
                if(row == null || row.Index  < 0)
                {
                    MessageBox.Show("Selecciona una fila de categoría.", "Atención",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                }
                string categoriaNombre = row.Cells[0].Value.ToString();
                if(string.IsNullOrEmpty(categoriaNombre) || categoriaNombre == "Total")
                {
                    MessageBox.Show("Selecciona una categoría válida (no TOTAL).", "Atención",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                }
                BE.Categoria categoria = tableroActual.Categorias.FirstOrDefault(c => c.Nombre == categoriaNombre);
                if (categoria.Utilizada)
                {
                    MessageBox.Show("Esa categoría ya fue utilizada.", "Atención",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                bool ok = tableroService.AnotarPuntuacion(tableroActual, categoriaNombre, 0);
                if (!ok) { MessageBox.Show("No se pudo tachar la categoría.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

                // 2) Registrar movimiento ANTES de cambiar el turno
                BE.Usuario jugadorAccion = gestorpartida.ObtenerJugadorActual();
                bitacora.RegistrarMovimiento("Tachar", jugadorAccion, categoriaNombre, 0, tiradaActual?.NumeroLanzamientos ?? 0);

                int jugadorIndice = (tableroActual == tableroJugador1) ? 0 : 1;
                gestorpartida.SumarPuntos(jugadorIndice, 0);

                CrearTablerosVisuales();
                tiradaService.ReiniciarTirada(tiradaActual);
                checkBox1.Checked = checkBox2.Checked = checkBox3.Checked = checkBox4.Checked = checkBox5.Checked = false;
                RefrescarTodasImagenes();
                button6.Enabled = false;

                gestorpartida.CambiarTurno();
                RefreshTurno();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al tachar: {ex.Message}", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dadoControl1_Load(object sender, EventArgs e)
        {

        }
    }
    }
