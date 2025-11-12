using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;


namespace GUI
{
    public partial class Form2 : Form
    {
        BLL.BitacoraService bitacora = new BLL.BitacoraService();
        public Form2()
        {
            InitializeComponent();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            if ((this.Visible))
            {
                DataTable tabla = bitacora.ConsultarMovimientos();
                dataGridView3.AutoGenerateColumns = true;
                dataGridView3.DataSource = tabla;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
