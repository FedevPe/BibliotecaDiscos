using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibliotecaDiscos
{
    public partial class frmBiblioteca : Form
    {
        private List<Disco> datos = new List<Disco>();
        public frmBiblioteca()
        {
            InitializeComponent();
        }

        private void frmBiblioteca_Load(object sender, EventArgs e)
        {
            DiscoService  discoService = new DiscoService();
            datos = discoService.Listar();
            dgvDatos.DataSource = datos;

            imgDisco.Load();
        }


        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            Disco discoSelect = (Disco)dgvDatos.CurrentRow.DataBoundItem;
            CargarImagen(discoSelect.UrlImagen);
        }
        public void CargarImagen(string imagen)
        {
            try
            {
                imgDisco.Load(imagen);
            }
            catch (Exception )
            {

               imgDisco.Load("https://storage.googleapis.com/proudcity/mebanenc/uploads/2021/03/placeholder-image.png");
            }

        }
    }
}
