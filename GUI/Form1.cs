using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form1 : Form
    {
        private Gestor_Partida gestorpartida = new Gestor_Partida();
        private TiradaService tiradaService = new TiradaService();
        private readonly Gestor_Puntaje gestorPuntaje = new Gestor_Puntaje();
        private TableroService tableroService = new TableroService();
        private DadoService dadoService = new DadoService();
        private Tablero tableroJugador1;
        private Tablero tableroJugador2;
        private BE.Tirada tiradaActual;
        private readonly string[] _imagenesPath = new string[6]
        {
            "dado1.png", // Prueba usando "/" en lugar de "\\"
            "dado2.png", // Y elimina los espacios en los nombres
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
                if (Sesion.Get(0) != null) usuariosenpartida.Add(Sesion.Get(0));
                if (Sesion.Get(1) != null) usuariosenpartida.Add(Sesion.Get(1));
                gestorpartida.IniciarPartida(usuariosenpartida);
                tableroJugador1 = tableroService.CrearTablero(Sesion.Get(0));
                tableroJugador2 = tableroService.CrearTablero(Sesion.Get(1));
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
            if (Sesion.Get(0) != null)
            {
                button3.Visible = false;
            }
            if (Sesion.Get(1) != null)
            {
                button4.Visible = false;
            }
            Usuario usuario1 = Sesion.Get(0);
            if (usuario1 != null)
            {
                label1.Text = usuario1.Nombre.ToString();
            }
            Usuario usuario2 = Sesion.Get(1);
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
        
        #region "botones del form"
        private void button6_Click(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Usuario usuario = Sesion.Get(0);
            if (usuario != null)
            {
                // Guardamos el nombre para el mensaje
                string nombreUsuario = usuario.Nombre;

                // Eliminar usuario de la sesión
                Sesion.Eliminar(usuario);

                // Actualizar la interfaz
                label1.Text = "No iniciado";

                MessageBox.Show($"Se cerró la sesión de {nombreUsuario}", "Sesión cerrada",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Verificar si quedan usuarios en sesión
                if (Sesion.Get(0) == null && Sesion.Get(1) == null)
                {
                    // Si no quedan usuarios, volver al formulario de login
                    Log formLogin = new Log();
                    formLogin.Show();
                    this.Hide(); // O this.Hide() si prefieres ocultarlo en lugar de cerrarlo
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
            Usuario usuario = Sesion.Get(1);
            if (usuario != null)
            {
                // Guardamos el nombre para el mensaje
                string nombreUsuario = usuario.Nombre;

                // Eliminar usuario de la sesión
                Sesion.Eliminar(usuario);

                // Actualizar la interfaz
                label2.Text = "No iniciado";

                MessageBox.Show($"Se cerró la sesión de {nombreUsuario}", "Sesión cerrada",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Verificar si quedan usuarios en sesión
                if (Sesion.Get(0) == null && Sesion.Get(1) == null)
                {
                    // Si no quedan usuarios, volver al formulario de login
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
            InicializarDados(); // Inicializar los dados existentes
            btn_Iniciar.Enabled = false;
            btn_Finalizar.Enabled = true;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            // Verificar que la tirada esté inicializada
            if (tiradaActual == null)
            {
                tiradaActual = tiradaService.CrearNuevaTirada(5);
            }

            // Lanzar los dados (excepto los retenidos)
            tiradaService.LanzarDados(tiradaActual);

            // Actualizar la visualización
            dadoControl1.ActualizarImagen();
            dadoControl2.ActualizarImagen();
            dadoControl3.ActualizarImagen();
            dadoControl4.ActualizarImagen();
            dadoControl5.ActualizarImagen();
            
        }

        private void btn_Finalizar_Click(object sender, EventArgs e)
        {

            btn_Finalizar.Enabled = false;
            btn_Iniciar.Enabled = true;
        }
        #endregion

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
                // Obtener las categorías (usamos el primer tablero disponible como referencia)
                Tablero tableroReferencia = tableroJugador1 ?? tableroJugador2;

                // Llenar el DataGridView con los datos
                dataGridView1.Rows.Clear();

                if (tableroJugador1 != null)
                {
                    dataGridView1.Rows.Clear();
                    foreach (   BE.Categoria categoria in tableroJugador1.Categorias)
                    {
                        string[] row = new string[]
                        {
                    categoria.Nombre,
                    categoria.Utilizada ? categoria.Puntuacion.ToString() : "-"
                        };
                        dataGridView1.Rows.Add(row);
                    }

                    // Añadir total
                    dataGridView1.Rows.Add(new string[]
                    {
                "TOTAL",
                tableroService.CalcularTotalPuntos(tableroJugador1).ToString()
                    });
                }

                // Llenar datos para jugador 2
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

        

        private void InicializarDados()
        {
            try
            {
                // Crear instancia de Tirada con 5 dados
                tiradaActual = tiradaService.CrearNuevaTirada(5);
                
                // Referencias directas a los controles de dado
                DadoControl[] dadosControles = new DadoControl[5] {
                    dadoControl1, // Usar directamente los controles nombrados
                    dadoControl2,
                    dadoControl3, 
                    dadoControl4,
                    dadoControl5
                };
                
               
                for (int i = 0; i < dadosControles.Length && i < tiradaActual.Dados.Count; i++)
                {
                    // Asignar el objeto Dado al control visual
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

    
    }
    }
