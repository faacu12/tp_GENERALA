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
    public partial class Log : Form
    {
        private USUARIO usuario = new USUARIO();
        private Sesion sesion = new Sesion();
        BLL.BitacoraService bitacora = new BLL.BitacoraService();
        public Log()
        {
            InitializeComponent();
        }

        private void Log_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear nuevo usuario
                Usuario nuevoUsuario = new Usuario();
                nuevoUsuario.Nombre = textBox1.Text;
                nuevoUsuario.Contraseña = textBox2.Text;


                // Llamar a BLL para grabar
                usuario.Grabar(nuevoUsuario);
                
                sesion.Agregar(nuevoUsuario);
                MessageBox.Show("Usuario creado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bitacora.RegistrarCreacionUsuario(nuevoUsuario);
                // Abrir formulario del juego
                Form1 formJuego = new Form1();
                formJuego.Show();
                this.Hide();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar credenciales
                Usuario usuarioLogueado = usuario.ValidarLogin(textBox1.Text, textBox2.Text);
                sesion.Agregar(usuarioLogueado);
                BLL.BitacoraService bitacora = new BLL.BitacoraService();
                bitacora.RegistrarLogin(usuarioLogueado);
                MessageBox.Show($"¡Bienvenido {usuarioLogueado.Nombre}!", "Login Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Abrir formulario del juego
                Form1 formJuego = new Form1();
                formJuego.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 formJuego = new Form1();
            formJuego.Show();
            this.Hide();
        }

        private void Log_Shown(object sender, EventArgs e)
        {
            if (Sesion.Get(0) != null || Sesion.Get(1) != null)
            {
                button3.Visible = true;
            }
        }
    }
}
